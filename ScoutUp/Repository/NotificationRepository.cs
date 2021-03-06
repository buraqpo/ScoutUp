﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ScoutUp.DAL;
using ScoutUp.Models;
using ScoutUp.ViewModels;

namespace ScoutUp.Repository
{
    public class NotificationRepository
    {

        public UserNotifications AddNotification(string userid, string message,string notifiyDirection,string notifyLink)
        {
            var notify = new UserNotifications()
            {
                UserId = userid,
                UserNotificationsMessage = message,
                UserNotificationsDate = DateTime.Now,
                UserNotificationsRead = false,
                NotificationLink = notifyLink
            };
            using (var context = new ScoutUpDB())
            {
                context.UserNotifications.Add(notify);
                context.SaveChanges();
            }
            return notify;
        }

        public List<UserNotifications> GetUserNotifications(string userid)
        {
            List<UserNotifications> notifications =new List<UserNotifications>();
            List<UserNotifications> notifications2 = new List<UserNotifications>();
            using (var context = new ScoutUpDB())
            {
                 notifications= context.UserNotifications.Include(e => e.User)
                     .Where(e => e.UserId == userid)
                    .Where(r => r.UserNotificationsRead==false)
                    .ToList();
                foreach (var notify in notifications)
                {
                    notifications2.Add(new UserNotifications {UserNotificationsID = notify.UserNotificationsID,
                        UserId = notify.UserId,
                        UserNotificationsMessage = notify.UserNotificationsMessage,
                        UserNotificationsDate = notify.UserNotificationsDate,
                        NotificationLink = notify.NotificationLink
                    });
                }
            }

            return notifications2;
        }
        public void UpdateNotification(List<UserNotifications> notifyList)
        {
            using (var context = new ScoutUpDB())
            {
                foreach (var notify in notifyList)
                {
                    notify.UserNotificationsRead = true;
                    context.Entry(notify).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public OnlineUsersViewModel GetUserFromPostId(int postid)
        {
            var model = new OnlineUsersViewModel();
            using (var context = new ScoutUpDB())
            {
                model =context.Posts.Where(e=> e.PostID==postid).Select(e=> new OnlineUsersViewModel
                {
                    UserId = e.UserId
                    ,UserProfilePhoto = e.User.UserProfilePhoto
                }).FirstOrDefault();
            }

            return model;
        }
    }
}