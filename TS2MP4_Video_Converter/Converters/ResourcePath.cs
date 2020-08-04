using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TS2MP4_Video_Converter.Converters
{
    class ResourcePath : MarkupExtension
    {
        public string Path { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new Uri($"pack://application:,,,/TS2MP4_Video_Converter;component/Resources/{Path}");
        }
    }
}
