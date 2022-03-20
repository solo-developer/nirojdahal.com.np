using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly IBaseRepository<Tag> _tagRepo;
        public TagService(IBaseRepository<Tag> tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public async Task Delete(long id)
        {
            var tag = await _tagRepo.GetByIdAsync(id) ?? throw new ItemNotFoundException("Tag doesnot exist.");
            tag.MarkDeleted();
            await _tagRepo.UpdateAsync(tag, id);

        }

        public async Task<List<TagDto>> GetAllUndeleted()
        {
            var tags = await _tagRepo.FindAllAsync(a => !a.IsDeleted);

            List<TagDto> response = new List<TagDto>();
            tags.ForEach(tag =>
            {
                response.Add(new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                });
            });

            return response;
        }

        public async Task Save(TagDto dto)
        {
            bool isNameDuplicate = await this.IsNameDuplicate(dto);
            if (isNameDuplicate)
                throw new DuplicateItemException("Tag with same name already exists.");

            var entity = new Tag();
            Copy(entity, dto);
            await _tagRepo.InsertAsync(entity);
        }

        public async Task Update(TagDto dto)
        {
            var tag = await _tagRepo.GetByIdAsync(dto.Id) ?? throw new ItemNotFoundException("Tag doesnot exist.");

            bool isNameDuplicate = await this.IsNameDuplicate(dto);
            if (isNameDuplicate)
                throw new DuplicateItemException("Tag with same name already exists.");
            Copy(tag, dto);
            await _tagRepo.UpdateAsync(tag, dto.Id);
        }

        private void Copy(Tag entity, TagDto dto)
        {
            entity.Name = dto.Name;
        }

        private async Task<bool> IsNameDuplicate(TagDto dto)
        {
            var tagWithSameName = await _tagRepo.FindAsync(a => a.Name.ToLower().Trim().Equals(dto.Name.ToLower().Trim()));

            if (tagWithSameName == null)
                return false;

            return tagWithSameName.Id != dto.Id;
        }
    }
}
