using BetterThanMooshak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Services
{
    public class CourseService
    {
        private ApplicationDbContext db;

        public CourseService()
        {
            db = new ApplicationDbContext();
        }


    }
}