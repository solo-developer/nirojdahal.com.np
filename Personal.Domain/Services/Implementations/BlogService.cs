﻿using Personal.Domain.Assemblers.Interface;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Helpers;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

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

        private const string IMAGE_FOLDER = "uploads/blog-img";

        public BlogService(IBlogRepository blogRepo, IBlogAssembler blogAssembler, IBlogCategoryRepository blogCategoryRepo, IFileHelper fileHelper, IBaseRepository<Tag> tagRepo, IBaseRepository<BlogTagMap> blogTagMapRepo, IBaseRepository<Newsletter> newsLetterRepo)
        {
            _blogRepo = blogRepo;
            _blogAssembler = blogAssembler;
            _blogCategoryRepo = blogCategoryRepo;
            _fileHelper = fileHelper;
            _tagRepo = tagRepo;
            _blogTagMapRepo = blogTagMapRepo;
            _newsLetterRepo = newsLetterRepo;
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

        public async Task<PagedResultDto> GetAll(int skip, int? take = null)
        {
            PagedResultDto result = new PagedResultDto();
            var blogs = _blogRepo.GetQueryable().Where(a => a.IsPublished);

            result.TotalRecords = blogs.Count();
            result.Take = 6;
            result.Skip = skip;
            blogs = blogs.Skip(skip);
            if (take.HasValue)
                blogs = blogs.Take(take.Value);
            blogs = blogs.OrderByDescending(a => a.CreatedDate).AsQueryable();

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
                var dto = new BlogDto();
                Copy(blog, dto, category);
                response.Add(dto);
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
                    _fileHelper.moveImageFromTempPathToDestination(dto.BannerImage, IMAGE_FOLDER);
                tx.Complete();
            }
        }

        public void Publish(long id, string performedBy)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var blog = _blogRepo.GetById(id) ?? throw new ItemNotFoundException("Blog not found.");

                blog.ShowInView(performedBy);

                _blogRepo.Update(blog);

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
                    _fileHelper.moveImageFromTempPathToDestination(dto.BannerImage, IMAGE_FOLDER);
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
            dto.CategoryName = category?.Title;
            dto.BannerImage = entity.BannerImage;
            dto.TagNames = entity.Tags.Select(a => a.Tag.Name).ToList();
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
    }
}
