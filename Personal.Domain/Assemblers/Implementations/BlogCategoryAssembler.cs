using Personal.Domain.Assemblers.Interface;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Linq;

namespace Personal.Domain.Assemblers.Implementations
{
    public class BlogCategoryAssembler : IBlogCategoryAssembler
    {
        private readonly ISlugGeneratorService _slugGeneratorService;
        private readonly IBlogCategoryRepository _blogCategoryRepo;

        public BlogCategoryAssembler(ISlugGeneratorService slugGeneratorService, IBlogCategoryRepository blogCategoryRepo)
        {
            _slugGeneratorService = slugGeneratorService;
            _blogCategoryRepo = blogCategoryRepo;
        }

        public void Copy(BlogCategoryDto dto, BlogCategory entity)
        {
            var slug = _slugGeneratorService.Generate(dto.Title);
            while (IsSlugDuplicate(slug, dto))
            {
                var slugParts = slug.Split('-');
                slugParts = slugParts.SkipLast(1).ToArray();
                slug = string.Join('-', slugParts);
            }
            entity.Slug = slug;
            entity.Title = dto.Title;
            entity.Description = dto.Description;
        }
        private bool IsSlugDuplicate(string slug, BlogCategoryDto dto)
        {
            var categoryWithDuplicateSlug = _blogCategoryRepo.GetQueryable().Where(a => a.Slug.ToLower().Trim().Equals(slug.ToLower().Trim())).SingleOrDefault();

            if (categoryWithDuplicateSlug == null)
            {
                return false;
            }
            return categoryWithDuplicateSlug.Id != dto.Id;
        }
    }
}
