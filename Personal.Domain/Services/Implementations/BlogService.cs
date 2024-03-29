﻿using Personal.Domain.Assemblers.Interface;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Enums;
using Personal.Domain.Exceptions;
using Personal.Domain.Helpers;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace Personal.Domain.Services.Implementations
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepo;
        private readonly IBlogAssembler _blogAssembler;
        private readonly IBlogCategoryRepository _blogCategoryRepo;

        private readonly IFileHelper _fileHelper;
        private readonly IBaseRepository<Tag> _tagRepo;
        private readonly IBaseRepository<BlogTagMap> _blogTagMapRepo;
        private readonly IBaseRepository<Newsletter> _newsLetterRepo;
        private readonly IBaseRepository<AppSetting> _appsettingRepo;
        private readonly IEmailSenderService _emailSenderService;

        private const string IMAGE_FOLDER = "uploads/blog-img";

        public BlogService(IBlogRepository blogRepo, IBlogAssembler blogAssembler, IBlogCategoryRepository blogCategoryRepo, IFileHelper fileHelper, IBaseRepository<Tag> tagRepo, IBaseRepository<BlogTagMap> blogTagMapRepo, IBaseRepository<Newsletter> newsLetterRepo, IEmailSenderService emailSenderService, IBaseRepository<AppSetting> appsettingRepo)
        {
            _blogRepo = blogRepo;
            _blogAssembler = blogAssembler;
            _blogCategoryRepo = blogCategoryRepo;
            _fileHelper = fileHelper;
            _tagRepo = tagRepo;
            _blogTagMapRepo = blogTagMapRepo;
            _newsLetterRepo = newsLetterRepo;
            _emailSenderService = emailSenderService;
            _appsettingRepo = appsettingRepo;
        }

        public void Delete(long id, string performedBy)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var blog = _blogRepo.GetById(id) ?? throw new ItemNotFoundException("Blog not found.");

                blog.Delete(performedBy);
                _blogRepo.Update(blog);

                tx.Complete();
            }
        }

        public async Task<PagedResultDto> GetAll(BlogFilterDto dto)
        {
            PagedResultDto result = new PagedResultDto();
            var blogs = _blogRepo.GetQueryable().Where(a => !a.IsDeleted);
            if (dto.OnlyPublished)
                blogs = blogs.Where(a => a.IsPublished);

            if (!string.IsNullOrEmpty(dto.Category))
                blogs = blogs.Where(a => string.Equals(dto.Category.ToLower().Trim(), a.Category.Title.ToLower().Trim()));

            result.TotalRecords = blogs.Count();
            result.Take = 6;
            result.Skip = dto.Skip;
            blogs = blogs.Skip(dto.Skip);
            if (dto.Take.HasValue)
                blogs = blogs.Take(dto.Take.Value);
            blogs = blogs.OrderByDescending(a => a.Rate).ThenByDescending(a=>a.CreatedDate).AsQueryable();

            var allBlogCategoryIds = blogs.Where(a => a.CategoryId.HasValue).Select(a => a.CategoryId).Distinct();

            var allCategories = _blogCategoryRepo.GetQueryable().Where(a => allBlogCategoryIds.Contains(a.Id)).ToList();

            var response = new List<BlogDto>();

            foreach (var blog in blogs.ToList())
            {
                BlogCategory category = null;
                if (blog.CategoryId.HasValue)
                {
                    category = allCategories.SingleOrDefault(a => a.Id == blog.CategoryId);
                }
                var blogDto = new BlogDto();
                Copy(blog, blogDto, category);
                response.Add(blogDto);
            }
            result.Data = response;
            return result;
        }

        public BlogDto GetById(long id)
        {
            var blog = _blogRepo.GetById(id) ?? throw new ItemNotFoundException("Blog not found.");
            var dto = new BlogDto();
            Copy(blog, dto, blog.Category);
            return dto;
        }

        public void Unpublish(long id, string performedBy)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var service = _blogRepo.GetById(id) ?? throw new ItemNotFoundException("Blog not found.");

                service.HideFromView(performedBy);

                _blogRepo.Update(service);

                tx.Complete();
            }
        }

        public void Save(BlogDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var blog = new Entities.Blog();
                _blogAssembler.Copy(dto, blog);
                blog.CreatedBy = dto.PerformedBy;
                blog.CreatedDate = DateTime.Now;
                blog.ModifiedBy = dto.PerformedBy;
                blog.ModifiedDate = DateTime.Now;

                dto.Tags.ForEach(tag =>
                {
                    blog.Tags.Add(new BlogTagMap
                    {
                        TagId = tag,
                        Tag = _tagRepo.Find(a => a.Id == tag)
                    });
                });
                _blogRepo.Insert(blog);

                if (!string.IsNullOrEmpty(dto.BannerImage))
                    _fileHelper.MoveImageFromTempPathToDestination(dto.BannerImage, IMAGE_FOLDER);
                tx.Complete();
            }
        }

        public async Task Publish(long id, string performedBy)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var blog = _blogRepo.GetById(id) ?? throw new ItemNotFoundException("Blog not found.");

                blog.ShowInView(performedBy);

                _blogRepo.Update(blog);

                var subscribers = _newsLetterRepo.GetQueryableWithNoTracking().Select(a => a.Email).Distinct();
                if (subscribers.Any())
                {
                    var websiteUrl = _appsettingRepo.GetQueryable().SingleOrDefault(a => a.Key == AppSettingKeys.Website.ToString())?.Value;
                    string emailContent = $"<p>There is a new blog post from <strong>Niroj Dahal</strong> you might be interested in.</p> <a href='{websiteUrl}blogs/{blog.Slug}'>{blog.Slug}</a>";
                    _emailSenderService.SendEmail(new EmailMessageDto(blog.Title, emailContent, subscribers.ToArray()));
                }
                tx.Complete();
            }
        }

        public void Update(BlogDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var blog = _blogRepo.GetById(dto.Id) ?? throw new ItemNotFoundException("Blog doesnot exist.");
                _blogTagMapRepo.DeleteRange(blog.Tags.ToList());
                _blogAssembler.Copy(dto, blog);
                blog.ModifiedBy = dto.PerformedBy;
                blog.ModifiedDate = DateTime.Now;
                _blogRepo.Update(blog);

                dto.Tags.ForEach(tag =>
                {
                    _blogTagMapRepo.Insert(new BlogTagMap
                    {
                        Blog = blog,
                        Tag = _tagRepo.Find(a => a.Id == tag)
                    });
                });

                if (!string.IsNullOrEmpty(dto.BannerImage))
                    _fileHelper.MoveImageFromTempPathToDestination(dto.BannerImage, IMAGE_FOLDER);
                tx.Complete();
            }
        }

        private void Copy(Blog entity, BlogDto dto, BlogCategory category)
        {
            dto.Id = entity.Id;
            dto.Title = entity.Title;
            dto.ShortDescription = entity.ShortDescription;
            dto.Content = entity.Content;
            dto.IsPublished = entity.IsPublished;
            dto.CategoryId = category?.Id;
            dto.Rating = entity.Rate;
            dto.CategoryName = category?.Title;
            dto.BannerImage = entity.BannerImage;
            dto.TagNames = entity.Tags.Select(a => a.Tag.Name).ToList();
            dto.Tags = entity.Tags.Select(a => a.Tag.Id).ToList();
            dto.SetSlug(entity.Slug);
            dto.SetDate(entity.CreatedDate);
        }

        public async Task<BlogDto> GetBySlug(string slug)
        {
            var blog = await _blogRepo.FindAsync(a => a.Slug.ToLower().Trim().Equals(slug.ToLower().Trim())) ?? throw new ItemNotFoundException("Blog not found.");
            var dto = new BlogDto();
            Copy(blog, dto, blog.Category);
            return dto;
        }

        public async Task SubscribeNewsletter(NewsletterSubscriptionDto dto)
        {
            var newsletterWithSameEmail = await _newsLetterRepo.FindAsync(a => a.Email.ToLower().Trim().Equals(dto.Email.ToLower().Trim()));

            if (newsletterWithSameEmail != null)
                throw new DuplicateItemException("User has already subscribed the newsletter.");

            var entity = new Newsletter()
            {
                Email = dto.Email,
                SubscribedDate = DateTime.Now
            };

            await _newsLetterRepo.InsertAsync(entity);
        }

        public async Task SaveRating(BlogRatingDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var blog = _blogRepo.GetById(dto.BlogId) ?? throw new ItemNotFoundException("Blog doesnot exist.");
                blog.Rate = dto.Rating;
                _blogRepo.Update(blog);
                tx.Complete();
            }
        }
    }
}
