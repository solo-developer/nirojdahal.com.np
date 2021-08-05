using Personal.Domain.Assemblers.Interface;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Linq;

namespace Personal.Domain.Assemblers.Implementations
{
    public class BlogAssembler : IBlogAssembler
    {
        private readonly ISlugGeneratorService _slugGeneratorService;
        private readonly IBlogCategoryRepository _blogCategoryRepo;

        public BlogAssembler(ISlugGeneratorService slugGeneratorService, IBlogCategoryRepository blogCategoryRepo)
        {
            _slugGeneratorService = slugGeneratorService;
            _blogCategoryRepo = blogCategoryRepo;
        }

        public void Copy(BlogDto dto, Blog entity)
        {
            var slug = _slugGeneratorService.Generate(dto.Title);
            while (IsSlugDuplicate(slug, dto))
            {
                var slugParts = slug.Split('-');
                slugParts = slugParts.SkipLast(1).ToArray();
                slug = string.Join('-', slugParts);
            }
            if (!string.IsNullOrWhiteSpace(dto.BannerImage))
            {
                entity.BannerImage = dto.BannerImage;
            }

            entity.Slug = slug;
            entity.CategoryId = dto.CategoryId;
            if (dto.CategoryId.HasValue)
            {
                entity.Category = _blogCategoryRepo.GetById(dto.CategoryId.Value) ?? throw new ItemNotFoundException("Blog category doesnot exist.");
            }
            entity.Title = dto.Title;
            entity.ShortDescription = dto.ShortDescription;
            entity.Content = dto.Content;
        }

        private bool IsSlugDuplicate(string slug, BlogDto dto)
        {
            var serviceWithDuplicateSlug = _blogCategoryRepo.GetQueryable().Where(a => a.Slug.ToLower().Trim().Equals(slug.ToLower().Trim())).SingleOrDefault();

            if (serviceWithDuplicateSlug == null)
            {
                return false;
            }
            return serviceWithDuplicateSlug.Id != dto.Id;
        }
    }
}
