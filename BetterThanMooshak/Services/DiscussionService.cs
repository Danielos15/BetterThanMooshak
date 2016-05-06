using BetterThanMooshak.Models;
using BetterThanMooshak.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Services
{
    public class DiscussionService
    {
        private ApplicationDbContext db;

        public DiscussionService()
        {
            db = new ApplicationDbContext();
        }

        /*public AddDiscussion(DiscussionViewModel addDisc)
        {
            return null;
        }

        public EditDiscussion(DiscussionViewModel editDisc)
        {
            return null;
        }

        public AddComment(string comment)
        {
            return null;
        }*/
    }
}