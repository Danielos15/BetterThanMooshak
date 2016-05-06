using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
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

        public CourseViewModel GetCourseById(int id)
        {
            var course = (from c in db.Courses
                          where c.id == id
                          select c).SingleOrDefault();

            CourseViewModel result = new CourseViewModel();
            result.course = course;

            return result;
        }

        public CourseViewModel GetAllCourses()
        {
            var courses = (from c in db.Courses
                           select c).ToList();

            CourseViewModel result = new CourseViewModel();
            result.courses = courses;

            return result;
        }

        public bool Add(CourseAddViewModel newCourse)
        {
            Course temp = new Course() { name = newCourse.name, startDate = newCourse.startDate, endDate = newCourse.endDate };
            db.Courses.Add(temp);
            return Convert.ToBoolean(db.SaveChanges());
        }

        public bool RemoveCourseById (int id)
        {
            db.Courses.Remove(GetCourseById(id).course);

            return Convert.ToBoolean(db.SaveChanges());
        }
    }
}
