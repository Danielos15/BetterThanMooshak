using BetterThanMooshak.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BetterThanMooshak.Models.ViewModel
{
    public class DiscussionViewModel
    {
        public int problemId { get; set; }
        public List<DiscussionTopicViewModel> topics { get; set; }
    }

    public class DiscussionTopicViewModel
    {
        public int topicId { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string userName { get; set; }
        public IQueryable<DiscussionComment> comments { get; set; }
    }
}