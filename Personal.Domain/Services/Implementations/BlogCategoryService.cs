using Personal.Domain.Assemblers.Interface;
using Personal.Domain.Dto;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Personal.Domain.Services.Implementations
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IBlogCategoryRepository _blogCategoryRepo;
        private readonly IBlogCategoryAssembler _blogCategoryAssembler;

        public BlogCategoryService(IBlogCategoryRepository blogCategoryRepo,IBlogCategoryAssembler blogCategoryAssembler)
        {
            _blogCategoryRepo = blogCategoryRepo;
            _blogCategoryAssembler = blogCategoryAssembler;
        }

        public void Delete(long id)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var category = _blogCategoryRepo.GetById(id) ?? throw new ItemNotFoundException("Blog Category not found.");

                if (category.HasBlogs())
                {
                    throw new ChildCollectionsPresentException("Specified blog category contains blogs.");
                }
                _blogCategoryRepo.Delete(category);

                tx.Complete();
            }
        }

        public List<BlogCategoryDto> GetAll()
        {
            var categories = _blogCategoryRepo.GetAll();

            var response = new List<BlogCategoryDto>();

            foreach (var category in categories)
            {
                var dto = new BlogCategoryDto();
                Copy(category, dto);
                response.Add(dto);
            }
            return response;
        }

        public BlogCategoryDto GetById(long id)
        {
            var category = _blogCategoryRepo.GetById(id) ?? throw new ItemNotFoundException("Blog Category not found.");
            var dto = new BlogCategoryDto();
            Copy(category, dto);
            return dto;
        }

        public void Save(BlogCategoryDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var entity = new Entities.BlogCategory();
                _blogCategoryAssembler.Copy(dto, entity);
                entity.CreatedBy = dto.PerformedBy;
                entity.CreatedDate = DateTime.Now;
                _blogCategoryRepo.Insert(entity);
                tx.Complete();
            }
        }

        public void Update(BlogCategoryDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var category = _blogCategoryRepo.GetById(dto.Id) ?? throw new ItemNotFoundException("Blog Category doesnot exist.");
                _blogCategoryAssembler.Copy(dto, category);
                category.ModifiedBy = dto.PerformedBy;
                category.ModifiedDate = DateTime.Now;
                _blogCategoryRepo.Update(category);
                tx.Complete();
            }
        }

        private void Copy(Entities.BlogCategory entity, BlogCategoryDto dto)
        {
            dto.Id = entity.Id;
            dto.Title = entity.Title;
            dto.Description = entity.Description;
            dto.SetSlug(entity.Slug);
        }
    }
}
