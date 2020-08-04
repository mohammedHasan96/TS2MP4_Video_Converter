using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS2MP4_Video_Converter.Models
{
    public class VideoInfo
    {
        public string Codec { get; private set; }
        public int Width { get; private set; }
        public int Heigth { get; private set; }
        public double FrameRate { get; private set; }
        public string FrameRateMode { get; private set; }
        public string ScanType { get; private set; }
        public TimeSpan Duration { get; private set; }
        public int Bitrate { get; private set; } = 2_000_000;
        public string AspectRatioMode { get; private set; }
        public double AspectRatio { get; private set; }
    }
}
