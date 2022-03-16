using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Entities;
using Personal.Domain.Enums;
using Personal.Domain.Repository.Interface;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace niroj.website.ViewComponents
{
    [ViewComponent(Name = "NavbarView")]
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IContactUsRepository _contactUsRepo;
        private readonly IBaseRepository<LastRead> _lastReadRepo;
        public NavbarViewComponent(IBaseRepository<LastRead> lastReadRepo,IContactUsRepository contactUsRepo)
        {
            _contactUsRepo = contactUsRepo;
            _lastReadRepo = lastReadRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loggedInUserId = Request.HttpContext.User.FindFirst("user_id")?.Value;
            var lastMessagesReadByUser = _lastReadRepo.GetQueryable().OrderByDescending(a => a.ReadDate).FirstOrDefault(a => a.ReadBy == loggedInUserId && a.Key == ReadableTableKeys.ContactUs);

            DateTime lastRead = DateTime.MinValue;
            if (lastMessagesReadByUser != null)
            {
                lastRead = lastMessagesReadByUser.ReadDate;
            }

            var unReadMessages = _contactUsRepo.GetQueryable().Where(a => a.Date >= lastRead).ToList();
            return View(unReadMessages);
        }
    }
}
