﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoutUp.Models
{
    public class UserPhotos
    {
        public int UserPhotosID { get; set; }
        public string UserId { get; set; }
        public string UserPhotoSmall { get; set; }
        public string UserPhotoBig { get; set; }
        public virtual User User { get; set; }
    }
}