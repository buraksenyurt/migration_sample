using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ClassicGames.Dashboard
{
    //Kaynak: https://github.com/PacktPublishing/Adopting-.NET-5--Architecture-Migration-Best-Practices-and-New-Features/blob/master/Chapter05/netframework472/BookApp/AdminDesktop/ImageConverter.cs
    public class ImageConverter
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var byteArray = (byte[])value;

                if (byteArray == null)
                    return null;

                var image = new BitmapImage();
                using (var imageStream = new MemoryStream())
                {
                    imageStream.Write(byteArray, 0, byteArray.Length);
                    imageStream.Seek(0, SeekOrigin.Begin);
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = imageStream;
                    image.EndInit();
                    image.Freeze();
                }
                return image;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
