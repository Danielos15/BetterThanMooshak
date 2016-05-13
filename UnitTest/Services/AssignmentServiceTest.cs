using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterThanMooshak.Services;
using BetterThanMooshak.Models.Entities;
using UnitTest.Tests;
using BetterThanMooshak.Models.ViewModel;
using System.Linq;
using BetterThanMooshak.Models;
using BetterThanMooshak;

namespace UnitTest.Services
{
    [TestClass]
    public class AssignmentServiceTest
    {
        private AssignmentService service;
        private MockDataContext mockDb = new MockDataContext();

        [TestInitialize]
        public void Initialize()
        {
            var assignment1 = new Assignment
            {
                id = 1,
                courseId = 1,
                name = "Assignment 1",
                startDate = DateTime.Now,
                endDate = DateTime.Now,
                description = "Includes 1 problem"
            };
            mockDb.Assignments.Add(assignment1);

            var assignment2 = new Assignment
            {
                id = 2,
                courseId = 1,
                name = "Assignment 2",
                startDate = DateTime.Now,
                endDate = DateTime.Now,
                description = "Includes 2 problem"
            };
            mockDb.Assignments.Add(assignment2);

            var assignment3 = new Assignment
            {
                id = 3,
                courseId = 1,
                name = "Assignment 3",
                startDate = DateTime.Now,
                endDate = DateTime.Now,
                description = "Includes 3 problem"
            };
            mockDb.Assignments.Add(assignment3);

            var course1 = new Course
            {
                id = 1,
                name = "Gagnaskipan",
                startDate = DateTime.Now,
                endDate = DateTime.Now
            };
            mockDb.Courses.Add(course1);

            service = new AssignmentService(mockDb);
        }

        [TestMethod]
        public void GetAssignmentByIdTest()
        {
            // Arrange:
            const int assignmentId = 3;

            // Act:
            var result = service.GetAssignmentById(assignmentId);

            //Assert:
            Assert.AreEqual("Assignment 3", result.name);
        }

        [TestMethod]
        public void EditAssignmentTest()
        {
            // Arrange:
            const int assignmentId = 2;
            var assignment4 = new AssignmentAddViewModel
            {
                name = "Verkefni II",
                endDate = DateTime.Now,
                startDate = DateTime.Now,
                description = "erfiður áfangi"
            };

            // Act:
            service.Edit(assignmentId, assignment4);
            var editCourse = service.GetAssignmentById(assignmentId);

            //Assert:
            Assert.AreNotEqual("Assignment 2", editCourse.name);
        }

        [TestMethod]
        public void AddAssignmet()
        {
            // Arrange:
            var mockDb = new MockDataContext();
            var assignment4 = new AssignmentAddViewModel
            {
                name = "Assignment 4",
                startDate = DateTime.Now,
                endDate = DateTime.Now,
                description = "Includes 4 problem"
            };

            // Act:
            var result = service.AddAssignmet(1, assignment4);

            //Assert:
            Assert.AreNotEqual(4, mockDb.Assignments.Count());
        }
    }
}
