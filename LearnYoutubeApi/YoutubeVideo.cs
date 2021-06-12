using System;
using System.Collections.Generic;
using System.Text;

namespace LearnYoutubeApi
{
    class YoutubeVideo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string UrlVideo { get; set; }
        public string PublishedAt { get; set; }
        public string ChannelId { get; set; }
        public string Hours { get; set; } = "0";
        public string Minutes { get; set; } = "0";
        public string Seconds { get; set; } = "0";
        public string Duration { get; set; } = "0";

        public string ToString()
        {
            return "Video Id=\t\t" + Id + "\n" +
                   "Title=\t\t" + Title + "\n" +
                   "Hour=\t\t" + Hours + " Minute=" + Minutes + " Second=" + Seconds + "\n" +
                   "Duration=\t\t" + Duration + "\n" +
                   "UrlVideo=\t\t" + UrlVideo + "\n" +
                   "PublishedAt=\t\t" + PublishedAt + "\n" +
                   "ChannelId=\t\t" + ChannelId;
        }
        public static YoutubeVideo FromApi(dynamic ResponseApi)
        {
            return new YoutubeVideo()
            {
                Id = ResponseApi.id.videoId,
                Title = ResponseApi.snippet.title,
                UrlVideo = "https://www.youtube.com/watch?v=" + ResponseApi.id.videoId,
                PublishedAt = "" + ResponseApi.snippet.publishedAt,
                ChannelId = ResponseApi.snippet.channelId,
                Hours = "0",
                Minutes = "0",
                Seconds = "0",
                Duration = "0"
            };
        }
    }
}
