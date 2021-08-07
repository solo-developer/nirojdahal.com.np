using Personal.Domain.Dto;
using System;
using System.Collections.Generic;

namespace Personal.Domain.Services.Interface
{
    public interface IContactUsService
    {
        void Save(ContactUsDto dto);

        List<ContactUsDto> GetAllLaterThan(DateTime after=default);
    }
}
