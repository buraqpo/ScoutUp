﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ScoutUp.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ScoutUp.DAL
{
    public class ScoutUpDB : IdentityDbContext<User>
    {
        public ScoutUpDB() : base("ScoutUpDB") { }

      //  public new DbSet<ScoutUp.Models.User> Users {get;set;}
    public DbSet<Post> Posts {get;set;}
    public DbSet<Hobbies> Hobbies {get;set;}
    public DbSet<PostComments> PostComments {get;set;}
    public DbSet<PostLikes> PostLikes {get;set;}
    public DbSet<PostPhotos> PostPhotos {get;set;}
    public DbSet<UserFollow> UserFollow {get;set;}
    public DbSet<UserHobbies> UserHobbies {get;set;}
    public DbSet<UserPhotos> UserPhotos {get;set;}
    public DbSet<UserRatings> UserRatings { get;set;}
    public DbSet<Categories> Categories {get;set;}
    public DbSet<CategoryItems> CategoryItems {get;set;}
    public DbSet<ChatMessages> ChatMessages { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<UsersLastMoves> UsersLastMoves { get; set; }

        public DbSet<UserNotifications> UserNotifications { get; set; }
       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

        }
    }
}