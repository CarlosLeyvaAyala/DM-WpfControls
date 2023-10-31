using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DM_WpfControls.Converters;

/// <summary>
/// Loads images from string in a non-locking way. 
/// </summary>
[ValueConversion(typeof(string), typeof(BitmapImage))]
public class StringToImgConverter : IValueConverter {
  public object? Convert(object value, Type targetType, object parameter, CultureInfo culture) {
    var fn = (string)value;
    if (!File.Exists(fn)) return null;
    var uri = new Uri(fn, UriKind.Absolute);
    var img = new BitmapImage();
    img.BeginInit();
    img.CacheOption = BitmapCacheOption.OnLoad;
    img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
    img.UriSource = uri;
    img.EndInit();
    return img;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => "";
}