using Medialnfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medialnfo
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    return 4;

                var mi = new MediaInfo();
                mi.Open(args[0]);

                var videoInfo = new VideoInfo(mi);
                var audioInfo = new AudioInfo(mi);
                mi.Close();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new { VideoInfo = videoInfo, AudioInfo = audioInfo });
                Console.WriteLine(json);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 5;
            }
        }


        class VideoInfo
        {
            public string Codec { get; private set; }
            public int Width { get; private set; }
            public int Heigth { get; private set; }
            public double FrameRate { get; private set; }
            public string FrameRateMode { get; private set; }
            public string ScanType { get; private set; }
            public TimeSpan Duration { get; private set; }
            public int Bitrate { get; private set; }
            public string AspectRatioMode { get; private set; }
            public double AspectRatio { get; private set; }

            public VideoInfo(MediaInfo mi)
            {
                try { Codec = mi.Get(StreamKind.Video, 0, "Format"); } catch { };
                try { Width = int.Parse(mi.Get(StreamKind.Video, 0, "Width")); } catch { };
                try { Heigth = int.Parse(mi.Get(StreamKind.Video, 0, "Height")); } catch { };
                try { Duration = TimeSpan.FromMilliseconds(int.Parse(mi.Get(StreamKind.Video, 0, "Duration"))); } catch { };
                try { Bitrate = int.Parse(mi.Get(StreamKind.Video, 0, "BitRate")); } catch { };
                try { AspectRatioMode = mi.Get(StreamKind.Video, 0, "AspectRatio/String"); } catch { };//as formatted string 
                try { AspectRatio = double.Parse(mi.Get(StreamKind.Video, 0, "AspectRatio")); } catch { };
                try { FrameRate = double.Parse(mi.Get(StreamKind.Video, 0, "FrameRate")); } catch { };
                try { FrameRateMode = mi.Get(StreamKind.Video, 0, "FrameRate_Mode"); } catch { };
                try { ScanType = mi.Get(StreamKind.Video, 0, "ScanType"); } catch { };
            }
        }

        class AudioInfo
        {
            public string Codec { get; private set; }
            public string CompressionMode { get; private set; }
            public string ChannelPositions { get; private set; }
            public TimeSpan Duration { get; private set; }
            public int Bitrate { get; private set; }
            public string BitrateMode { get; private set; }
            public int SamplingRate { get; private set; }

            public AudioInfo(MediaInfo mi)
            {
                try { Codec = mi.Get(StreamKind.Audio, 0, "Format"); } catch { };
                try { Duration = TimeSpan.FromMilliseconds(int.Parse(mi.Get(StreamKind.Audio, 0, "Duration"))); } catch { };
                try { Bitrate = int.Parse(mi.Get(StreamKind.Audio, 0, "BitRate")); } catch { };
                try { BitrateMode = mi.Get(StreamKind.Audio, 0, "BitRate_Mode"); } catch { };
                try { CompressionMode = mi.Get(StreamKind.Audio, 0, "Compression_Mode"); } catch { };
                try { ChannelPositions = mi.Get(StreamKind.Audio, 0, "ChannelPositions"); } catch { };
                try { SamplingRate = int.Parse(mi.Get(StreamKind.Audio, 0, "SamplingRate")); } catch { };
            }
        }
    }
}
