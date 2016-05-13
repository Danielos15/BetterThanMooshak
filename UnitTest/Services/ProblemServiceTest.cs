using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.Tests;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Services;
using System.Linq;
using BetterThanMooshak.Models.ViewModel;

namespace UnitTest.Services
{
    [TestClass]
    public class ProblemServiceTest
    {
        private ProblemService service;
        private MockDataContext mockDb = new MockDataContext();

        [TestInitialize]
        public void Initialize()
        {
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
            var problem4 = new Problem
            {
                id = 4,
                assignmentId = 1,
                description = "This is problem 4",
                maxAttempts = 40,
                name = "Gagnaskipan problem 4",
                percentOfGrade = 40
            };

            // Act:
            service.AddProblem(problem4);

            //Assert:
            Assert.AreEqual(4, mockDb.Problems.Count());
        }

        [TestMethod]
        public void AddProblemTest()
        {
            // Arrange:
            const int problemId = 1;

            // Act:
            var result = service.GetProblemById(problemId);
            service.deleteProblem(result);

            //Assert:
            Assert.AreNotEqual(3, mockDb.Problems.Count());
        }

        [TestMethod]
        public void EditProblemTest()
        {
            // Arrange:
            var problem5 = new ProblemAddViewModel
            {
                assignmentId = 5,
                description = "This is problem 5",
                maxAttempts = 50,
                name = "Gagnaskipan problem 5",
                percentOfGrade = 50,
                assignmentName = "Skilaverkefni 5",
                courseName = "Gagnaskipan"
            };

            // Act:
            service.Edit(1, ref problem5);
            var result = service.GetProblemById(1);

            //Assert:
            Assert.AreEqual(3, mockDb.Problems.Count());
            Assert.AreEqual(50, result.maxAttempts);
        }

    }
}
