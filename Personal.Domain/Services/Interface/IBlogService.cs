using Personal.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Personal.Domain.Services.Interface
{
    public interface IBlogService
    {
        void Save(BlogDto dto);
        void Update(BlogDto dto);
        void Delete(long id,string performedBy);
        Task Publish(long id,string performedBy);
        void Unpublish(long id, string performedBy);

        Task<PagedResultDto> GetAll(BlogFilterDto dto);

        BlogDto GetById(long id);
        Task<BlogDto> GetBySlug(string slug);

        Task SubscribeNewsletter(NewsletterSubscriptionDto dto);
    }
}
