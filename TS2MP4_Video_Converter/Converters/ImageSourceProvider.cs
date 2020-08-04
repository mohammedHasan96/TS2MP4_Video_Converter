using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace TS2MP4_Video_Converter.Converters
{
    class ImageSourceProvider : MarkupExtension, IValueConverter
    {
        public Uri Uri { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!((string)value.ToString()).EndsWith(".png"))
                value = $"{value}.png".ToLower();
            Uri = new Uri($"pack://application:,,,/TS2MP4_Video_Converter;component/Resources/{value}");
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = Uri;
            image.EndInit();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Uri == null)
                return this;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = Uri;
            image.EndInit();
            return image;
        }
    }
}
