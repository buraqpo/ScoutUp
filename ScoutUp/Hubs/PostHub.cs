﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ScoutUp.DAL;
using ScoutUp.Models;

namespace ScoutUp.Hubs
{
    [HubName("postHub")]
    public class PostHub : Hub
    {
        private readonly ScoutUp.DAL.ScoutUpDB _db=new ScoutUpDB();
        public Task Like(int? postId,string userid)
        {
            var likePost = SaveLike(postId,userid);
            var _connections = NotificationHub._connections;
            return Clients.All.updateLikeCount(likePost);
        }

        private LikePost SaveLike(int? postid,string userid)
        {
            var post = _db.Posts.Find(postid);

            var user = _db.Users.Find(userid);
            var isLiked = _db.PostLikes.Where(e => e.UserId == user.Id).FirstOrDefault(e => e.PostID == postid);
            if (isLiked != null)
            {
                using (var context = new ScoutUpDB())
                {
                    _db.Dispose();

                    context.PostLikes.Attach(isLiked);
                    context.Entry(isLiked).State = EntityState.Deleted;
                    context.SaveChanges();
                    return new LikePost
                       {
                            LikeCount = post.PostLikes.Count,
                            Liked=false,
                           PostId = (int)postid
                    };
                }
            }

            var postLike = new PostLikes
            {
                UserId = user.Id,
                PostID = (int) postid,
                IsLiked = true
            };

            try
            {
                using (var context = new ScoutUpDB())
                {
                    context.PostLikes.Add(postLike);
                    context.UsersLastMoves.Add(new UsersLastMoves { MoveDate = DateTime.Now, UserId = user.Id, UsersLastMoveText = " bir gönderiyi beğendi.", UsersMoveLink = "/users/index/" + post.UserId + "#post" + post.PostID });
                    context.SaveChanges();
                    return new LikePost
                    {
                        LikeCount = post.PostLikes.Count,
                        Liked = true
                        ,PostId = (int)postid
                    };
                }
            }
            catch (Exception ex)
            {

                return new LikePost
                {
                    LikeCount = post.PostLikes.Count,
                    Liked = false
                };
            }
            //    var postId = int.Parse(id);
            //    var baseContext = Context.Request.GetHttpContext();
            //    var postRepository = _db;
            //    var item = _db.Posts.Find(postId);
            //    var liked = new PostLike
            //    {
            //        IPAddress = baseContext.Request.UserHostAddress,
            //        PostId = item.Id,
            //        UserAgent = baseContext.Request.UserAgent,
            //        UserLike = true
            //    };
            //    var dupe = item.PostLikes.FirstOrDefault(e => e.IPAddress == liked.IPAddress);
            //    if (dupe == null)
            //    {
            //        item.PostLikes.Add(liked);
            //    }
            //    else
            //    {
            //        dupe.UserLike = !dupe.UserLike;
            //    }
            //    postRepository.SaveChanges();
            //    var post = postRepository.GetById(postId);

            //    return new LikePost
            //    {
            //        LikeCount = post.PostLikes.Count(e => e.UserLike)
            //    };
            //}
        }

        private class LikePost
        {
            public int PostId { get; set; }
            public int LikeCount { get; set; }
            public bool Liked { get; set; }
        }
        
    }
}