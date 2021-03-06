﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ScoutUp.DAL;
using ScoutUp.ViewModels;

namespace ScoutUp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ScoutUpDB _db = new ScoutUpDB();
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Categories()
        {// kullanıcının puanladıkları
            /*select UserRatings.UserId,UserRatings.UserRating,CategoryItems.CategoryItemID,
CategoryItems.CategoryItemPhoto,CategoryItems.CategoryItemName
from UserRatings
inner join 
CategoryItems on dbo.UserRatings.CategoryItemID = dbo.CategoryItems.CategoryID
where UserId=1*/
            //Kullanıcının puanlamadığı itemler
            /*select * from CategoryItems where CategoryItemID not in(
select CategoryItemID
from UserRatings where UserId=1)*/
            var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            var itemViewModel = _db.CategoryItems.SqlQuery(
                "select * from CategoryItems where CategoryItemID not in(select CategoryItemID from UserRatings where UserId = '"+userid +"')")
                .Select(e => new CategoryItemViewModel { CategoryItemName = e.CategoryItemName, CategoryItemId = e.CategoryItemID, CategoryId = e.CategoryID ,CategoryItemPhoto = e.CategoryItemPhoto});
            var viewModel = _db.Categories.Select(e => new ViewModels.CategoryViewModel {CategoryId = e.CategoryID,CategoryName = e.CategoryName});
            //var itemViewModel = _db.CategoryItems.Select(e => new CategoryItemViewModel { CategoryItemName =e.CategoryItemName,CategoryItemId = e.CategoryItemID,CategoryId = e.CategoryID});
            //var userRatings = _db.UserRatings.Where(e => e.UserId==userid).Select(t => new )
            Dictionary<CategoryViewModel,List<CategoryItemViewModel>> model = new Dictionary<CategoryViewModel,List<CategoryItemViewModel>>();
            foreach (var categoryViewModel in viewModel)
            {
                List<CategoryItemViewModel> tempList = new List<CategoryItemViewModel>();
                foreach (var categoryItemViewModel in itemViewModel)
                {
                    if (categoryItemViewModel.CategoryId == categoryViewModel.CategoryId)
                    {
                        tempList.Add(categoryItemViewModel);
                    }
                }
                model.Add(categoryViewModel, tempList);
            }
            return PartialView("Categories", model);
        }
        public ActionResult CategoriesLiked(string id)
        {// kullanıcının puanladıkları
            /*select UserRatings.UserId,UserRatings.UserRating,CategoryItems.CategoryItemID,
CategoryItems.CategoryItemPhoto,CategoryItems.CategoryItemName
from UserRatings
inner join 
CategoryItems on dbo.UserRatings.CategoryItemID = dbo.CategoryItems.CategoryID
where UserId=1*/
            //Kullanıcının puanlamadığı itemler
            /*select * from CategoryItems where CategoryItemID not in(
select CategoryItemID
from UserRatings where UserId=1)*/
            var userid = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(id))
            {
                userid = id;
            }
            
            var itemViewModel = _db.CategoryItems.SqlQuery(
                "select * from CategoryItems where CategoryItemID in(select CategoryItemID from UserRatings where UserId = '" + userid + "')")
                .Select(e => new CategoryItemViewModel { CategoryItemName = e.CategoryItemName, CategoryItemId = e.CategoryItemID, CategoryId = e.CategoryID, CategoryItemPhoto = e.CategoryItemPhoto });
            var viewModel = _db.Categories.Select(e => new ViewModels.CategoryViewModel { CategoryId = e.CategoryID, CategoryName = e.CategoryName });
            //var itemViewModel = _db.CategoryItems.Select(e => new CategoryItemViewModel { CategoryItemName =e.CategoryItemName,CategoryItemId = e.CategoryItemID,CategoryId = e.CategoryID});
            //var userRatings = _db.UserRatings.Where(e => e.UserId==userid).Select(t => new )
            var ratings = _db.UserRatings.Where(e => e.UserId == userid).ToDictionary(e => e.CategoryItemID);
            Dictionary<CategoryViewModel, List<CategoryItemViewModel>> model = new Dictionary<CategoryViewModel, List<CategoryItemViewModel>>();
            foreach (var categoryViewModel in viewModel)
            {
                List<CategoryItemViewModel> tempList = new List<CategoryItemViewModel>();
                foreach (var categoryItemViewModel in itemViewModel)
                {
                    if (categoryItemViewModel.CategoryId == categoryViewModel.CategoryId)
                    {
                        categoryItemViewModel.UserRating = ratings[categoryItemViewModel.CategoryItemId].UserRating;
                        tempList.Add(categoryItemViewModel);
                    }
                }
                model.Add(categoryViewModel, tempList);
            }
            return PartialView("Categories", model);
        }
    }
}