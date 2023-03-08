using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using niroj.website.Controllers;
using niroj.website.Helpers;
using niroj.website.Logging;
using Personal.Domain.Dto;
using Personal.Domain.Entities;
using Personal.Domain.Enums;
using Personal.Domain.Exceptions;
using Personal.Domain.Repository.Interface;
using Personal.Domain.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace niroj.website.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/messages")]
    public class MessageController : BaseController
    {
        private readonly IBaseRepository<LastRead> _lastReadRepo;
        private readonly ILastReadService _lastReadService;
        private readonly IContactUsService _contactUsService;
        private readonly ILog _logService;

        public MessageController(IBaseRepository<LastRead> lastReadRepo, ILastReadService lastReadService, IContactUsService contactUsService, ILog logService)
        {
            _lastReadRepo = lastReadRepo;
            _lastReadService = lastReadService;
            _contactUsService = contactUsService;
            _logService = logService;
        }

        [Route("")]
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var lastMessagesReadByUser = _lastReadRepo.GetQueryable().OrderByDescending(a => a.ReadDate).FirstOrDefault(a => a.ReadBy == getLoggedInUserId() && a.Key == ReadableTableKeys.ContactUs);

                DateTime lastRead = DateTime.MinValue;
                if (lastMessagesReadByUser != null)
                {
                    lastRead = lastMessagesReadByUser.ReadDate;
                }

                var datas = _contactUsService.GetAllLaterThan();
                datas.ForEach(data =>
                {
                    if (data.GetDate() > lastRead)
                    {
                        data.SetUnread();
                    }
                });

                _lastReadService.Record(ReadableTableKeys.ContactUs, getLoggedInUserId());
                return View(datas);
            }
            catch (CustomException ex)
            {
                AlertHelper.setMessage(this, ex.Message, MessageType.error);
            }
            catch (System.Exception ex)
            {
                AlertHelper.setMessage(this, "Failed to get messages.", MessageType.error);
                _logService.Error($"Failed to get messages , {ex}");
            }
            return View(new List<ContactUsDto>());
        }
    }
}
