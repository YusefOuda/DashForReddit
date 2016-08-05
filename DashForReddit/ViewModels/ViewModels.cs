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
        public string Thumbnail { get; set; }
    }

    public class Subreddit : IComparable
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public int CompareTo(object o)
        {
            Subreddit a = this;
            Subreddit b = (Subreddit)o;
            return string.Compare(a.DisplayName, b.DisplayName);
        }
    }

    public class SettingNav
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }

    public class Comment
    {
        public string Body { get; set; }
        public bool HasReplies { get; set; }
        public bool HasNoReplies
        {
            get
            {
                return !HasReplies;
            }
        }
    }
}
