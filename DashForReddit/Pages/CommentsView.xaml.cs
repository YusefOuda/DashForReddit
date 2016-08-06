using DashForReddit.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TreeViewControl;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static DashForReddit.Reddit.Models.PostDetailsAndComments;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DashForReddit.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommentsView : Page
    {
        public CommentsView()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var post = await Reddit.Reddit.getPostDetails(e.Parameter as string);
            PostTitle.Text = post.First().data.children.First().data.title;
            PostBody.Text = post.First().data.children.First().data.selftext;
            foreach (var comment in post.Last().data.children.Where(x => x.kind != "more"))
            {
                bool hasReplies = comment.data != null && comment.data.replies != null && comment.data.replies.data != null && comment.data.replies.data.children.Count(x => x.kind != "more") > 0;
                var parentComment = new Comment()
                {
                    Body = comment.data.body,
                    HasReplies = hasReplies,
                    Author = comment.data.author,
                    Ups = $"{comment.data.ups} points",
                    Created = Helpers.GetElapsedTime(comment.data.created_utc)
                };
                TreeNode parent = new TreeNode()
                {
                    Data = parentComment,
                    IsExpanded = hasReplies
                };
                if (hasReplies)
                    GetReplies(parent, comment);
                commentTreeView.RootNode.Add(parent);
            }
            commentTreeView.ToString();
        }

        private void GetReplies(TreeNode parent, Child comment)
        {
            foreach (var child in comment.data.replies.data.children.Where(x => x.kind != "more"))
            {
                if (child.kind != "more")
                {
                    bool hasReplies = child.data != null && child.data.replies != null && child.data.replies.data != null && child.data.replies.data.children.Count(x => x.kind != "more") > 0;
                    var childComment = new Comment()
                    {
                        Body = child.data.body,
                        HasReplies = hasReplies,
                        Author = child.data.author,
                        Ups = $"{child.data.ups} points",
                        Created = Helpers.GetElapsedTime(comment.data.created_utc)
                    };
                    var childNode = new TreeNode()
                    {
                        Data = childComment,
                        IsExpanded = hasReplies
                    };
                    if (hasReplies)
                        GetReplies(childNode, child);
                    parent.Add(childNode);
                }
            }
        }
    }
}
