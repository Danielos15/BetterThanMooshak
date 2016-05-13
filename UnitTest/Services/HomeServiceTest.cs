using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BetterThanMooshak.Services;
using BetterThanMooshak.Models.Entities;
using UnitTest.Tests;
using BetterThanMooshak.Models;

namespace UnitTest.Services
{
    [TestClass]
    public class HomeServiceTest
    {
        private HomeService service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDataContext();

        }
    }
}
