using System;
using System.Collections.Generic;
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
using Windows.Web.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using DashForReddit.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DashForReddit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Post> Posts { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            Posts = new ObservableCollection<Post>();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Reddit.Reddit.getAll(Posts);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavPane.IsPaneOpen = !NavPane.IsPaneOpen;
        }

        private void PostListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //var post = (Post)e.OriginalSource;
            //navigate to post details
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var verticalOffsetValue = PostScrollViewer.VerticalOffset;
            var maxVerticalOffsetValue = PostScrollViewer.ExtentHeight - PostScrollViewer.ViewportHeight;
            if (maxVerticalOffsetValue < 0 || verticalOffsetValue == maxVerticalOffsetValue)
            {
                // Scrolled to bottom
                Reddit.Reddit.getAll(Posts, Posts.Last().Name);
            }
        }
    }
}
