using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using LearnYoutubeApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Media;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LearnProgram
{
    public class ServerClass
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public static void StartRecording(string DurationRecord)
        {
            Console.WriteLine("Recording ....");
            record("open new Type waveaudio Alias recsound", "", 0, 0);
            record("record recsound", "", 0, 0);
            Thread.Sleep(int.Parse(DurationRecord) + 3);
            StopRecording();
            Thread.Sleep(5000);
            Console.WriteLine("Recording End StartRecording....");
        }

        public static void StopRecording()
        {
            Console.WriteLine("Stop Record ....");
            record("save recsound D:\\mic.wav", "", 0, 0);
            record("close recsound", "", 0, 0);
            Console.WriteLine("Stop Record End StopRecording....");
        }

        public static void PlayRecording()
        {
            Console.WriteLine("Play record ....");
            new SoundPlayer("D:\\mic.wav").Play();
            Console.WriteLine("Play record End PlayRecording....");
        }

    }

    class Program
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        static HttpClient serverAPI = new HttpClient();

        static string keyApi = "AIzaSyDsRio2jTT4MLCVsu2zpKXJNqCp-WXB5-4";

        [STAThread]
        static async Task Main(string[] args)
        {

            //Thread InstanceCaller = new Thread(
            //    new ThreadStart(ServerClass.StartRecording));
            //// Start the thread.
            //InstanceCaller.Start
            serverAPI.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");

            try
            {
                //await GetDetailVideoYoutubeApi(new YoutubeVideo()
                //{
                //    Id = "ZEGrw_2Tl6E"
                //});
                //await GetResponseFromYoutubeApi();
                Run(new YoutubeVideo()
                {
                    Id = "paDsZzR5KeI",
                    Title = "TestVide_paDsZzR5KeI",
                    Duration = "10"
                }).Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            Console.WriteLine("Press any key to continue...5");
            Console.ReadKey();
        }


        public static async Task GetResponseFromYoutubeApi()
        {
            var channelId = "UCgR3SrM8T6GCFUTuxSFlR7A";
            string urlApi = serverAPI.BaseAddress + "search?key=" + keyApi +
                "&channelId=" + channelId + "&part=snippet,id&order=date&maxResults=20";

            HttpResponseMessage reuqestResponseAPI = await serverAPI.GetAsync(urlApi);
            var results = reuqestResponseAPI.Content.ReadAsStringAsync().Result;
            dynamic jsonResp = JsonConvert.DeserializeObject<ExpandoObject>(results, new ExpandoObjectConverter());

            if (jsonResp != null && jsonResp.items != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    YoutubeVideo vid = YoutubeVideo.FromApi(jsonResp.items[i]);
                    await GetDetailVideoYoutubeApi(vid);
                    Run(vid).Wait();
                    Console.WriteLine("---------------------------------------------------------------");

                }
            }
        }

        public static async Task GetDetailVideoYoutubeApi(YoutubeVideo Video)
        {
            string urlApi = serverAPI.BaseAddress + "videos?key=" + keyApi +
                "&id=" + Video.Id + "&part=contentDetails";

            HttpResponseMessage reuqestResponseAPI = await serverAPI.GetAsync(urlApi);
            var results = reuqestResponseAPI.Content.ReadAsStringAsync().Result;
            dynamic jsonResp = JsonConvert.DeserializeObject<ExpandoObject>(results, new ExpandoObjectConverter());

            if (jsonResp != null && jsonResp.items != null && jsonResp.items[0].contentDetails != null)
            {
                string Duration = jsonResp.items[0].contentDetails.duration;
                int StartIndex = 2;

                int IndexHour = Duration.IndexOf('H');
                //Console.WriteLine("Value Duration " + Duration);
                if (IndexHour != -1)
                {
                    //Console.WriteLine("Start Index " + StartIndex + " Index Hour " + IndexHour);
                    Video.Hours = Duration.Substring(StartIndex, IndexHour - StartIndex);
                    StartIndex = IndexHour + 1;
                }

                int IndexMinute = Duration.IndexOf('M');
                if (IndexMinute != -1)
                {
                    //Console.WriteLine("Start Index " + StartIndex + " Index Minute " + IndexMinute);
                    Video.Minutes = Duration.Substring(StartIndex, IndexMinute - StartIndex);
                    StartIndex = IndexMinute + 1;
                }

                int IndexSecond = Duration.IndexOf('S');
                if (IndexSecond != -1)
                {
                    //Console.WriteLine("Start Index " + StartIndex + " Index Second " + IndexSecond);
                    Video.Seconds = Duration.Substring(StartIndex, IndexSecond - StartIndex);
                }
                Video.Duration = "" + ((int.Parse(Video.Hours) * 3600) + (int.Parse(Video.Minutes) * 60) + int.Parse(Video.Seconds));
            }

            Console.WriteLine(Video.ToString());
        }

        private static async Task Run(YoutubeVideo Video)
        {

            Console.WriteLine("Start Recording .... vid " + Video.Title);
            record("open new Type waveaudio Alias recsound", "", 0, 0);
            record("record recsound", "", 0, 0);

            int recordDuree = int.Parse(Video.Duration) * 1000 + 3;
            Console.WriteLine("Durree = " + recordDuree);
            Thread.Sleep(recordDuree);

            Console.WriteLine("Stop Record ....");
            record("save recsound D:\\video_" + Video.Id + ".wav", "", 0, 0);
            record("close recsound", "", 0, 0);
            Thread.Sleep(5000);
            Console.WriteLine("End Recording.... vid " + Video.Title);

            //var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            //{
            //    ApiKey = "AIzaSyDsRio2jTT4MLCVsu2zpKXJNqCp-WXB5-4",
            //    ApplicationName = this.GetType().ToString()
            //});

            //var searchListRequest = youtubeService.Videos.List("snippet");
            ////searchListRequest.Q = "LiveCoder"; // Replace with your search term.
            //searchListRequest.MaxResults = 50;

            //// Call the search.list method to retrieve results matching the specified query term.
            //var searchListResponse = await searchListRequest.ExecuteAsync();

            //List<string> videos = new List<string>();
            //List<string> channels = new List<string>();
            //List<string> playlists = new List<string>();

            //Console.WriteLine("----------------------------------------------------");

            //foreach (var searchResult in searchListResponse.Items)
            //{
            //    Console.WriteLine(searchResult.ToString());
            //}
            //Console.WriteLine("----------------------------------------------------");

            //// Add each result to the appropriate list, and then display the lists of
            //// matching videos, channels, and playlists.
            //foreach (var searchResult in searchListResponse.Items)
            //{
            //    switch (searchResult.Id.Kind)
            //    {
            //        case "youtube#video":
            //            videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
            //            break;

            //        case "youtube#channel":
            //            channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
            //            break;

            //        case "youtube#playlist":
            //            playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
            //            break;
            //    }
            //}

            //Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            //Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            //Console.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
        }
    }
}
