using DashForReddit.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using static DashForReddit.Reddit.Models;

namespace DashForReddit.Reddit
{
    class Reddit
    {
        private static string client_id = "BOiQ4k3IpBQqCw";
        private static string base_url = "https://oauth.reddit.com/";
        public async static Task<Tuple<string, int>> getToken()
        {
            var uri = new Uri("https://www.reddit.com/api/v1/access_token");
            var deviceId = Windows.Storage.ApplicationData.Current.LocalSettings.Values["deviceid"];
            var content = new HttpStringContent($"grant_type=https://oauth.reddit.com/grants/installed_client&device_id={deviceId}");
            content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/x-www-form-urlencoded");
            using (var cli = new HttpClient())
            {
                cli.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{client_id}:"))}");
                cli.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpResponseMessage resp = await cli.PostAsync(uri, content);
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("Could not authenticate with Reddit API. Please try again.");
                var jsonString = await resp.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonString);
                return new Tuple<string, int>(tokenResponse?.access_token, tokenResponse.expires_in);
            }
        }

        public async static void getAll(ObservableCollection<Post> posts)
        {
            var uri = new Uri(base_url);
            using (var cli = new HttpClient())
            {
                var token = Windows.Storage.ApplicationData.Current.LocalSettings.Values["access_token"];
                cli.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                cli.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("win10:DashForReddit (by /u/chubbyshrimp"));
                HttpResponseMessage resp = await cli.GetAsync(uri);
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("Could not connect to Reddit API. Please try again.");
                var jsonString = await resp.Content.ReadAsStringAsync();
                var postsResponse = JsonConvert.DeserializeObject<RootObject>(jsonString);
                foreach (var post in postsResponse.data.children)
                {
                    posts.Add(new ViewModels.Post()
                    {
                        Title = post.data.title,
                        Author = post.data.author,
                        Ups = post.data.ups,
                        Subreddit = $"/r/{post.data.subreddit}"
                    });
                }
            }
        }
    }
}
