using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DashForReddit.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var client_id = Reddit.Reddit.client_id;
            var response_type = "code";
            var state = Guid.NewGuid();
            var redirect_uri = "com.youda.dashforreddit://test";//WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            var duration = "permanent";
            var scope = "mysubreddits";
            LoginWebView.Source = new Uri($"https://www.reddit.com/api/v1/authorize?client_id={client_id}&response_type={response_type}&state={state}&redirect_uri={redirect_uri}&scope={scope}&duration={duration}");
        }
    }
}
