using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterThanMooshak.Services;
using BetterThanMooshak.Models.Entities;
using UnitTest.Tests;

namespace UnitTest.Services
{
    [TestClass]
    public class AssignmentServiceTest
    {
        private AssignmentService service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();

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

    }
}
