﻿using System.Data.Entity;
using System.Linq;
using ScoutUp.DAL;
using ScoutUp.Models;

namespace ScoutUp.Classes
{
    public static class Extensions
    {
        public static IQueryable<Post> CompletePosts(this ScoutUpDB context)
        {
            return context.Posts.
                Include(ph => ph.PostPhotos).
                Include(pl => pl.PostLikes).
                Include(pc => pc.PostComments);
        }

        public static Post PostById(this ScoutUpDB context, int postId)
        {
            return CompletePosts(context).FirstOrDefault(e => e.PostID == postId);
        }

        public static IQueryable<User> CompleteUsers(this ScoutUpDB context)
        {
            return context.Users.
                Include(e => e.UserFollow).
                Include(p => p.UserPosts).
                Include("UserPosts.PostPhotos").
                Include("UserPosts.PostLikes").
                Include("UserPosts.PostComments").
                Include(h => h.UserHobbies).
                Include(ph => ph.UserPhotos).
                Include(nt => nt.UserNotifications);
        }

        public static User UserById(this ScoutUpDB context, string userId)
        {
            return CompleteUsers(context).FirstOrDefault(e => e.Id == userId);
        }

        public static IQueryable<User> GetAllUsers(this ScoutUpDB context)
        {
            return context.CompleteUsers();
        }
    }
}