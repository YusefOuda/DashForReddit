using DashForReddit.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using static DashForReddit.Reddit.Models.DefaultSubreddits;
using static DashForReddit.Reddit.Models.Token;

namespace DashForReddit.Reddit
{
    class Reddit
    {
        public static string client_id = "BOiQ4k3IpBQqCw";
        private static string base_url = "https://oauth.reddit.com";
        public static bool isLoggedIn
        {
            get
            {
                var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
                var refreshToken = (string)settings.Values["refresh_token"];
                return !string.IsNullOrWhiteSpace(refreshToken);
            }
        }
        public async static Task<bool> getDurableToken(string code)
        {
            var uri = new Uri("https://www.reddit.com/api/v1/access_token");
            var redirect_uri = "com.youda.dashforreddit://test";
            var content = new HttpStringContent($"grant_type=authorization_code&code={code}&redirect_uri={redirect_uri}");
            content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/x-www-form-urlencoded");
            TokenResponse tokenResponse;
            using (var cli = new HttpClient())
            {
                cli.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{client_id}:"))}");
                cli.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpResponseMessage resp = await cli.PostAsync(uri, content);
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("Could not authenticate with Reddit API. Please try again.");
                var jsonString = await resp.Content.ReadAsStringAsync();
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonString);
            }
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["access_token"] = tokenResponse.access_token;
            localSettings.Values["refresh_token"] = tokenResponse.refresh_token;
            localSettings.Values["access_token_expiration"] = DateTime.Now.AddSeconds(tokenResponse.expires_in).ToString();

            return true;
        }

        private async static Task<bool> refreshAccessToken()
        {
            var uri = new Uri("https://www.reddit.com/api/v1/access_token");
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var refresh_token = localSettings.Values["refresh_token"];
            var content = new HttpStringContent($"grant_type=refresh_token&refresh_token={refresh_token}");
            content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/x-www-form-urlencoded");
            TokenResponse tokenResponse;
            using (var cli = new HttpClient())
            {
                cli.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{client_id}:"))}");
                cli.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpResponseMessage resp = await cli.PostAsync(uri, content);
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("Could not authenticate with Reddit API. Please try again.");
                var jsonString = await resp.Content.ReadAsStringAsync();
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonString);
            }
            localSettings.Values["access_token"] = tokenResponse.access_token;
            localSettings.Values["access_token_expiration"] = DateTime.Now.AddSeconds(tokenResponse.expires_in).ToString();
            return true;
        }

        private async static Task<bool> getToken()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var uri = new Uri("https://www.reddit.com/api/v1/access_token");
            var deviceId = Windows.Storage.ApplicationData.Current.LocalSettings.Values["deviceid"];
            var content = new HttpStringContent($"grant_type=https://oauth.reddit.com/grants/installed_client&device_id={deviceId}");
            content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/x-www-form-urlencoded");
            TokenResponse tokenResponse;
            using (var cli = new HttpClient())
            {
                cli.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{client_id}:"))}");
                cli.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpResponseMessage resp = await cli.PostAsync(uri, content);
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("Could not authenticate with Reddit API. Please try again.");
                var jsonString = await resp.Content.ReadAsStringAsync();
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonString);
            }
            localSettings.Values["access_token"] = tokenResponse.access_token;
            localSettings.Values["access_token_expiration"] = DateTime.Now.AddSeconds(tokenResponse.expires_in).ToString();
            return true;
        }

        private async static Task<bool> ensureTokenExists()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            DateTime expiration;
            DateTime.TryParse((string)localSettings.Values["access_token_expiration"], out expiration);
            if (!string.IsNullOrWhiteSpace((string)localSettings.Values["refresh_token"]))
            {
                if (expiration < DateTime.Now)
                    await refreshAccessToken();
            }
            else if (string.IsNullOrWhiteSpace((string)localSettings.Values["access_token"]))
            {
                await getToken();
            }
            else if (localSettings.Values["access_token_expiration"] != null && DateTime.TryParse((string)localSettings.Values["access_token_expiration"], out expiration))
            {
                if (expiration < DateTime.Now.AddSeconds(-120))
                {
                    await getToken();
                }
            }

            return true;
        }

        public async static void getPosts(ObservableCollection<ViewModels.Post> posts, string after = null, string subreddit = null, bool overrideColl = false, string sort = null)
        {
            var x = await ensureTokenExists();
            var url = base_url;
            if (!string.IsNullOrWhiteSpace(subreddit))
                url = $"{url}/r/{subreddit}";
            if (!string.IsNullOrWhiteSpace(sort))
                url = $"{url}/{sort}";
            if (!string.IsNullOrWhiteSpace(after))
                url = $"{url}/?after={after}";
            var uri = new Uri(url);
            using (var cli = new HttpClient())
            {
                var token = Windows.Storage.ApplicationData.Current.LocalSettings.Values["access_token"];
                cli.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                cli.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("win10:DashForReddit (by /u/chubbyshrimp"));
                HttpResponseMessage resp = await cli.GetAsync(uri);
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("Could not connect to Reddit API. Please try again.");
                var jsonString = await resp.Content.ReadAsStringAsync();
                var postsResponse = JsonConvert.DeserializeObject<Models.PostList.RootObject>(jsonString);
                if (overrideColl)
                    posts.Clear();
                foreach (var post in postsResponse.data.children)
                {
                    var postVM = new ViewModels.Post()
                    {
                        Title = post.data.title,
                        Author = post.data.author,
                        Ups = post.data.ups,
                        Subreddit = $"/r/{post.data.subreddit}",
                        Name = post.data.name,
                        URL = post.data.url,
                        Permalink = post.data.permalink,
                        Created = String.Format("{0:F}", DateTimeOffset.FromUnixTimeSeconds(post.data.created_utc).DateTime)
                    };

                    Uri thumbnail;
                    Uri.TryCreate(post.data.thumbnail, UriKind.RelativeOrAbsolute, out thumbnail);
                    if (thumbnail != null && thumbnail.IsAbsoluteUri)
                        postVM.Thumbnail = post.data.thumbnail;
                    else
                        postVM.Thumbnail = "Assets/placeholder.png";

                    posts.Add(postVM);
                }
            }
        }

        public async static void getDefaultSubs(ObservableCollection<Subreddit> subs, string after = null)
        {
            if (string.IsNullOrWhiteSpace(after))
            {
                subs.Add(new Subreddit()
                {
                    Name = "All",
                    DisplayName = "all",
                    ID = "All"
                });
            }
            var url = $"https://www.reddit.com/subreddits/default.json";
            if (!string.IsNullOrWhiteSpace(after))
                url = $"{url}?after={after}";
            var uri = new Uri(url);
            using (var cli = new HttpClient())
            {
                HttpResponseMessage resp = await cli.GetAsync(uri);
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("Could not connect to Reddit API. Please try again.");
                var jsonString = await resp.Content.ReadAsStringAsync();
                var postsResponse = JsonConvert.DeserializeObject<DefaultSubredditsRoot>(jsonString);
                foreach (var post in postsResponse.data.children)
                {
                    subs.Add(new ViewModels.Subreddit()
                    {
                        Name = post.data.name,
                        DisplayName = post.data.display_name,
                        ID = post.data.id
                    });
                }
                if (!string.IsNullOrWhiteSpace(postsResponse.data.after))
                    getDefaultSubs(subs, postsResponse.data.after);
            }
        }

        public async static Task<List<Models.PostDetailsAndComments.RootObject>> getPostDetails(string link)
        {
            var x = await ensureTokenExists();
            var url = $"{base_url}{link}";
            var uri = new Uri(url);
            using (var cli = new HttpClient())
            {
                var token = Windows.Storage.ApplicationData.Current.LocalSettings.Values["access_token"];
                cli.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                cli.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("win10:DashForReddit (by /u/chubbyshrimp"));
                HttpResponseMessage resp = await cli.GetAsync(uri);
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("Could not connect to Reddit API. Please try again.");
                var jsonString = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Models.PostDetailsAndComments.RootObject>>(jsonString);
            }

        }

        private async static void getSubscribedSubs()
        {

        }
    }
}
