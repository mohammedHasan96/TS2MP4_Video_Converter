using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS2MP4_Video_Converter.Models
{
    public class MediaInfo
    {
        public VideoInfo VideoInfo { get; set; } = new VideoInfo();
        public AudioInfo AudioInfo { get; set; } = new AudioInfo(); 
    }
}
