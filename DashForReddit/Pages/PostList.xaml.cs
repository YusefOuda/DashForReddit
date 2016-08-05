using DashForReddit.Pages;
using DashForReddit.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DashForReddit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PostList : Page
    {
        private ObservableCollection<ViewModels.Post> Posts { get; set; }
        private string subreddit { get; set; }
        private string sort { get; set; }

        public PostList()
        {
            this.InitializeComponent();
            Posts = new ObservableCollection<ViewModels.Post>();
        }

        private void PostListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var post = (ViewModels.Post)e.ClickedItem;
            //Frame.Navigate(typeof(PostDetail), post.URL);
            Frame.Navigate(typeof(CommentsView), post.Permalink);
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var verticalOffsetValue = PostScrollViewer.VerticalOffset;
            var maxVerticalOffsetValue = PostScrollViewer.ExtentHeight - PostScrollViewer.ViewportHeight;
            if (maxVerticalOffsetValue < 0 || verticalOffsetValue == maxVerticalOffsetValue)
            {
                // Scrolled to bottom
                Reddit.Reddit.getPosts(Posts, Posts.Last().Name, subreddit, false, sort);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as dynamic;
            this.subreddit = param.sub;
            this.sort = param.sort;
            Reddit.Reddit.getPosts(Posts, null, subreddit, true, sort);
        }
    }
}
