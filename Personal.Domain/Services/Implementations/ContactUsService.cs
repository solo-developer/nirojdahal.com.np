using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Personal.Domain.Services.Implementations
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactUsRepository _contactUsRepo;
        public ContactUsService(IContactUsRepository contactUsRepo)
        {
            _contactUsRepo = contactUsRepo;
        }

        public List<ContactUsDto> GetAllLaterThan(DateTime after = default)
        {
            var messages = _contactUsRepo.GetQueryable().Where(a => a.Date > after).ToList();

            List<ContactUsDto> response = new List<ContactUsDto>();
            foreach (var message in messages)
            {
                ContactUsDto dto = new ContactUsDto();
                dto.SetDate(message.Date);
                dto.Name = message.Name;
                dto.Email = message.Email;
                dto.Comment = message.Comment;
                response.Add(dto);
            }
            return response;
        }

        public void Save(ContactUsDto dto)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                ContactUs entity = new ContactUs();
                entity.Name = dto.Name;
                entity.Email = dto.Email;
                entity.Comment = dto.Comment;
                entity.Date = DateTime.Now;
                _contactUsRepo.Insert(entity);
                tx.Complete();
            }
        }
    }
}
