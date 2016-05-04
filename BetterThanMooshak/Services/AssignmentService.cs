using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Services
{
    public class AssignmentService
    {
        private ApplicationDbContext db;

        public AssignmentService()
        {
            db = new ApplicationDbContext();
        }

        public AssignmentViewModel GetAssignmentsByCourse (int courseId)
        {
            return null;
        }

        public AssignmentViewModel GetAssignmentById (int? assignmentId)
        {
            return null;
        }
    }
}