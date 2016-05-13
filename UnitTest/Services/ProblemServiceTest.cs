using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Tests;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Services;
using System.Linq;

namespace UnitTest.Services
{
    [TestClass]
    public class ProblemServiceTest
    {
        private ProblemService service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();
            var problem1 = new Problem
            {
                id = 1,
                assignmentId = 1,
                description = "This is problem 1",
                maxAttempts = 10,
                name = "Gagnaskipan problem 1",
                percentOfGrade = 10
            };
            mockDb.Problems.Add(problem1);

            var problem2 = new Problem
            {
                id = 2,
                assignmentId = 1,
                description = "This is problem 2",
                maxAttempts = 20,
                name = "Gagnaskipan problem 2",
                percentOfGrade = 20
            };
            mockDb.Problems.Add(problem2);

            var problem3 = new Problem
            {
                id = 3,
                assignmentId = 1,
                description = "This is problem 3",
                maxAttempts = 30,
                name = "Gagnaskipan problem 3",
                percentOfGrade = 30
            };
            mockDb.Problems.Add(problem3);

            service = new ProblemService(mockDb);
        }

        [TestMethod]
        public void GetProblemByIdTest() 
        {
            // Arrange:
            const int problemId = 1;

            // Act:
            var result = service.GetProblemById(problemId);

            //Assert:
            Assert.AreEqual("Gagnaskipan problem 1", result.name);
        }

        
        [TestMethod]
        public void DeleteProblemTest()
        {
            // Arrange:

            // Act:

            //Assert:
        }
        
    }
}
