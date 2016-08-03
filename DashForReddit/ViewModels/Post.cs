using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashForReddit.ViewModels
{
    public class Post
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Ups { get; set; }
        public string Subreddit { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
    }

    public class DefaultSubreddit
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
