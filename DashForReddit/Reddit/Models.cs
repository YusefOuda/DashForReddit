﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashForReddit.Reddit
{
    class Models
    {
        public class TokenResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string device_id { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
        }

        public class MediaEmbed
        {
        }

        public class Source
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Resolution
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Source2
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Resolution2
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Gif
        {
            public Source2 source { get; set; }
            public List<Resolution2> resolutions { get; set; }
        }

        public class Source3
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Resolution3
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Mp4
        {
            public Source3 source { get; set; }
            public List<Resolution3> resolutions { get; set; }
        }

        public class Variants
        {
            public Gif gif { get; set; }
            public Mp4 mp4 { get; set; }
        }

        public class Image
        {
            public Source source { get; set; }
            public List<Resolution> resolutions { get; set; }
            public Variants variants { get; set; }
            public string id { get; set; }
        }

        public class Preview
        {
            public List<Image> images { get; set; }
        }

        public class SecureMediaEmbed
        {
        }

        public class Data2
        {
            public string domain { get; set; }
            public object banned_by { get; set; }
            public MediaEmbed media_embed { get; set; }
            public string subreddit { get; set; }
            public string selftext_html { get; set; }
            public string selftext { get; set; }
            public object likes { get; set; }
            public string suggested_sort { get; set; }
            public List<object> user_reports { get; set; }
            public object secure_media { get; set; }
            public string link_flair_text { get; set; }
            public string id { get; set; }
            public object from_kind { get; set; }
            public int gilded { get; set; }
            public bool archived { get; set; }
            public bool clicked { get; set; }
            public object report_reasons { get; set; }
            public string author { get; set; }
            public object media { get; set; }
            public int score { get; set; }
            public object approved_by { get; set; }
            public bool over_18 { get; set; }
            public bool hidden { get; set; }
            public Preview preview { get; set; }
            public int num_comments { get; set; }
            public string thumbnail { get; set; }
            public string subreddit_id { get; set; }
            public bool hide_score { get; set; }
            public object edited { get; set; }
            public string link_flair_css_class { get; set; }
            public string author_flair_css_class { get; set; }
            public int downs { get; set; }
            public SecureMediaEmbed secure_media_embed { get; set; }
            public bool saved { get; set; }
            public object removal_reason { get; set; }
            public string post_hint { get; set; }
            public bool stickied { get; set; }
            public object from { get; set; }
            public bool is_self { get; set; }
            public object from_id { get; set; }
            public string permalink { get; set; }
            public bool locked { get; set; }
            public string name { get; set; }
            public double created { get; set; }
            public string url { get; set; }
            public string author_flair_text { get; set; }
            public bool quarantine { get; set; }
            public string title { get; set; }
            public double created_utc { get; set; }
            public string distinguished { get; set; }
            public List<object> mod_reports { get; set; }
            public bool visited { get; set; }
            public object num_reports { get; set; }
            public int ups { get; set; }
        }

        public class Child
        {
            public string kind { get; set; }
            public Data2 data { get; set; }
        }

        public class Data
        {
            public string modhash { get; set; }
            public List<Child> children { get; set; }
            public string after { get; set; }
            public object before { get; set; }
        }

        public class RootObject
        {
            public string kind { get; set; }
            public Data data { get; set; }
        }
    }
}
