using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Services;
using CMS.Services.RepositoriesBase;
using CMS.Data.ModelEntity;

namespace CMS.Website.NotiHub
{
    public class NotificationHubs : Hub
    {
        private RepositoryWrapper Repository;
        public NotificationHubs(RepositoryWrapper _repository)
        {
            Repository = _repository;
        }
        public async Task SendAllNotification(string userId, string subject)
        {
            var profile = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(userId);
            if (profile != null)
            {
                if ((bool)profile.AllowNotifyApp)
                {
                    await Clients.All.SendAsync("ReceiveMessage", userId, subject);
                }
                if ((bool)profile.AllowNotifyEmail)
                {
                    await Repository.Setting.SendMail("Thông báo từ hệ thống", profile.Email, profile.FullName, subject, subject);
                }
                if ((bool)profile.AllowNotifySms)
                {

                }
            }

        }
        public async Task SendNotification(string userId, string subject,string content, string url, string imageUrl)
        {            
            var profile = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(userId);
            if (profile != null)
            {
                //save in user noti
                var model = new UserNotify();
                model.AspNetUsersId = userId;
                model.Subject = subject;
                model.Content = content;
                model.Url = url;
                model.ImageUrl = imageUrl;
                await Repository.UserNoti.UserNotiCreateNew(model);
                if ((bool)profile.AllowNotifyApp)
                {
                    await Clients.Caller.SendAsync("ReceiveMessage", userId, subject);
                }
                if ((bool)profile.AllowNotifyEmail)
                {
                    await Repository.Setting.SendMail("Thông báo từ hệ thống", profile.Email, profile.FullName, subject, content);
                }
                if ((bool)profile.AllowNotifySms)
                {

                }
            }

        }
        public async Task SendGroupNotification(string userId, string subject)
        {
            var profile = await Repository.AspNetUsers.AspNetUserProfilesGetByUserId(userId);
            if (profile != null)
            {
                if ((bool)profile.AllowNotifyApp)
                {
                    await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", userId, subject);
                }
                if ((bool)profile.AllowNotifyEmail)
                {
                    await Repository.Setting.SendMail("Thông báo từ hệ thống", profile.Email, profile.FullName, subject, subject);
                }
                if ((bool)profile.AllowNotifySms)
                {

                }
            }

        }       
    }
}

