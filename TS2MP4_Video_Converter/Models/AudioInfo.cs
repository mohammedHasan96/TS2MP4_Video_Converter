using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS2MP4_Video_Converter.Models
{
    public class AudioInfo
    {
        public string Codec { get; private set; }
        public string CompressionMode { get; private set; }
        public string ChannelPositions { get; private set; }
        public TimeSpan Duration { get; private set; }
        public int Bitrate { get; private set; }
        public string BitrateMode { get; private set; }
        public int SamplingRate { get; private set; }
    }
}
