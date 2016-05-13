using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterThanMooshak.Services;
using UnitTest.Tests;
using BetterThanMooshak.Models.Entities;
using System.Linq;
using BetterThanMooshak.Models.ViewModel;

namespace UnitTest.ServicesUnitTests
{
    [TestClass]
    public class CourseServiceTest
    {
        private CourseService service;
        private MockDataContext mockDb = new MockDataContext();

        [TestInitialize]
        public void Initialize()
        {
            var course1 = new Course
            {
                id = 1,
                name = "Gagnaskipan",
                startDate = DateTime.Now,
                endDate = DateTime.Now
            };
            mockDb.Courses.Add(course1);

            var course2 = new Course
            {
                id = 2,
                name = "Forritun",
                startDate = DateTime.Now,
                endDate = DateTime.Now
            };
            mockDb.Courses.Add(course2);

            var course3 = new Course
            {
                id = 3,
                name = "Verkefnalausnir",
                startDate = DateTime.Now,
                endDate = DateTime.Now
            };
            mockDb.Courses.Add(course3);

            service = new CourseService(mockDb);
        }

        [TestMethod]
        public void GetAllCoursesTest()
        {
            // Arrange:
            const int courseId = 3;

            // Act:
            var result = service.GetAllCourses().ToList();

            //Assert:
            Assert.AreEqual(courseId, result.Count);
        }

        [TestMethod]
        public void GetCourseByIdTest()
        {
            // Arrange:
            const int courseId = 3;

            // Act:
            var result = service.GetCourseById(courseId);

            //Assert:
            Assert.AreEqual("Verkefnalausnir", result.name);
        }

        [TestMethod]
        public void AddCourseTest()
        {
            // Arrange:
            var course4 = new CourseAddViewModel
            {
                name = "Reiknirit",
                endDate = DateTime.Now,
                startDate = DateTime.Now
            };

            // Act:
            service.Add(course4);
            var result = service.GetAllCourses().ToList();

            //Assert:
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public void EditCourseTest()
        {
            // Arrange:
            var courseId = 3;
            var course5 = new CourseAddViewModel
            {
                name = "Stærðfræði",
                endDate = DateTime.Now,
                startDate = DateTime.Now
            };

            // Act:
            service.Edit(courseId, course5);
            var editCourse = service.GetCourseById(courseId);

            //Assert:
            Assert.AreNotEqual("Verkefnalausnir", editCourse.name);
        }

        [TestMethod]
        public void DeleteCourseTest()
        {
            // Arrange:
            const int courseId = 1;

            // Act:
            service.RemoveCourseById(courseId);
            var result = service.GetAllCourses();

            //Assert:
            foreach(var x in result)
            {
                Assert.AreNotEqual("Gagnaskipan", x.name);
            }
        }
    }
}
