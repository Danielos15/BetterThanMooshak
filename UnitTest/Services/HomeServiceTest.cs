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
        private MockDataContext mockDb = new MockDataContext();

        [TestInitialize]
        public void Initialize()
        {
            //We had troubles with this unit test and couldn't create a fake user with ApplicationUser.
            //And since HomeService class only has one public function that needes an user id as a parameter
            //we got stuck.
            //If we could have create a fake user, then we could add fake courses, assignments, etc. and then
            //test out all the private functions in HomeService
        }
    }
}
