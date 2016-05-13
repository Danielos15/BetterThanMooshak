using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterThanMooshak.Services;
using UnitTest.Tests;
using BetterThanMooshak.Models.Entities;
using System.Linq;

namespace UnitTest.ServicesUnitTests
{
    [TestClass]
    public class CourseServiceTest
    {
        private CourseService service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
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
        public void TestMethod1()
        {
            // Arrange:


            // Act:
            var result = service.GetAllCourses().ToList();

            //Assert:
            Assert.AreEqual(3, result.Count);

        }
    }
}
