using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LearnProgram
{
    public class ServerClass
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public static void StartRecording()
        {
            Console.WriteLine("Recording ....");
            record("open new Type waveaudio Alias recsound", "", 0, 0);
            record("record recsound", "", 0, 0);
            Thread.Sleep(10000);
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
        // The method that will be called when the thread is started.
        public void InstanceMethod()
        {
            Console.WriteLine(
                "ServerClass.InstanceMethod is running on another thread.");

            // Pause for a moment to provide a delay to make
            // threads more apparent.
            Thread.Sleep(10000);
            Console.WriteLine(
                "The instance method called by the worker thread has ended.");
        }

        public static void StaticMethod()
        {
            Console.WriteLine(
                "ServerClass.StaticMethod is running on another thread.");

            // Pause for a moment to provide a delay to make
            // threads more apparent.
            Thread.Sleep(3000);
            Console.WriteLine(
                "The static method called by the worker thread has ended.");
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            ServerClass serverObject = new ServerClass();

            // Create the thread object, passing in the
            // serverObject.InstanceMethod method using a
            // ThreadStart delegate.

            Console.WriteLine("welcome to record input");

            Thread InstanceCaller = new Thread(
                new ThreadStart(ServerClass.StartRecording));

            // Start the thread.
            InstanceCaller.Start();

            //Thread.Sleep(10000);
            //// Create the thread object, passing in the
            //// serverObject.StaticMethod method using a
            //// ThreadStart delegate.
            //Thread StaticCaller = new Thread(
            //    new ThreadStart(ServerClass.StopRecording));

            //// Start the thread.
            //StaticCaller.Start();
            //Thread.Sleep(5000);
            //Thread StaticCallerPlay = new Thread(
            //    new ThreadStart(ServerClass.PlayRecording));

            //// Start the thread.
            //StaticCallerPlay.Start();

            Console.WriteLine("The Main() thread calls this after "
                + "starting the new StaticCaller thread.");


            //Console.WriteLine("YouTube Data API: Search");
            //Console.WriteLine("========================");

            //try
            //{
            //    new Program().Run().Wait();
            //}
            //catch (AggregateException ex)
            //{
            //    foreach (var e in ex.InnerExceptions)
            //    {
            //        Console.WriteLine("Error: " + e.Message);
            //    }
            //}

            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
        }




        private async Task Runs()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDsRio2jTT4MLCVsu2zpKXJNqCp-WXB5-4",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Videos.List("snippet");
            //searchListRequest.Q = "LiveCoder"; // Replace with your search term.
            searchListRequest.MaxResults = 50;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            Console.WriteLine("----------------------------------------------------");

            foreach (var searchResult in searchListResponse.Items)
            {
                Console.WriteLine(searchResult.ToString());
            }
            Console.WriteLine("----------------------------------------------------");

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
