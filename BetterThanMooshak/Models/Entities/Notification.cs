using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.Entities
{
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int assignmentId { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }

    }
}