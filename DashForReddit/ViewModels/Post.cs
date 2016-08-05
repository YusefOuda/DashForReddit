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
        public string Permalink { get; set; }
        public string Created { get; set; }
    }

    public class Subreddit
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class SettingNav
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
