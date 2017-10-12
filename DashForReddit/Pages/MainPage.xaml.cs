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
using DashForReddit.Pages;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using System.Text.RegularExpressions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DashForReddit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Subreddit> Subreddits { get; set; }
        private ObservableCollection<SettingNav> Settings { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            Subreddits = new ObservableCollection<Subreddit>();
            Settings = new ObservableCollection<SettingNav>();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavPane.IsPaneOpen = !NavPane.IsPaneOpen;
            HamburgerButton.Width = NavPane.IsPaneOpen? NavPane.OpenPaneLength : NavPane.CompactPaneLength;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateNavList(false);
            dynamic param = new
            {
                sub = "",
                sort = ((ComboBoxItem)(SortComboBox.SelectedItem)).Name.ToLower()
            };
            ContentFrame.Navigate(typeof(PostList), param);
        }

        private void UpdateNavList(bool overrideSubs)
        {
            Reddit.Reddit.getListOfSubs(Subreddits, null, overrideSubs);

            Settings.Clear();
            if (!Reddit.Reddit.isLoggedIn)
            {
                Settings.Add(new SettingNav()
                {
                    Name = "LoginItem",
                    Text = "Login"
                });
            }
            else
            {
                Settings.Add(new SettingNav()
                {
                    Name = "LogoutItem",
                    Text = "Logout"
                });
            }
        }

        private void NavListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked = e.ClickedItem as Subreddit;
            dynamic param = new {
                sub = clicked.Name ?? "",
                sort = ((ComboBoxItem)(SortComboBox.SelectedItem)).Name.ToLower()
            };
            ContentFrame.Navigate(typeof(PostList), param);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Uri)
            {
                var uri = e.Parameter as Uri;
                var paramResult = GetParams(uri.AbsoluteUri);
                await Reddit.Reddit.getDurableToken(paramResult["code"]);
                UpdateNavList(true);
            }
        }

        private static Dictionary<string, string> GetParams(string uri)
        {
            var matches = Regex.Matches(uri, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);
            return matches.Cast<Match>().ToDictionary(
                m => Uri.UnescapeDataString(m.Groups[2].Value),
                m => Uri.UnescapeDataString(m.Groups[3].Value)
            );
        }

        private async void SettingsNav_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clicked = e.ClickedItem as SettingNav;
            if (clicked.Name == "LoginItem")
            {
                ContentFrame.Navigate(typeof(LoginPage));
            }
            else if (clicked.Name == "LogoutItem")
            {
                await Reddit.Reddit.logOut();
                UpdateNavList(true);
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContentFrame != null)
            {
                var sub = NavListView != null && NavListView.SelectedItem != null ? ((Subreddit)(NavListView.SelectedItem)).Name : "";
                dynamic param = new
                {
                    sub = sub,
                    sort = ((ComboBoxItem)(SortComboBox.SelectedItem)).Name.ToLower()
                };
                ContentFrame.Navigate(typeof(PostList), param);
            }
        }
    }
}
