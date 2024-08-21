using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Learning_Words.Converters
{

    public class CorrectAnswerToColorConverter : IValueConverter
    {
       public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
{
    if (value is bool isCorrect)
    {
        return isCorrect ? (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00481C")) // Green
                         : (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#640007")); // Red
    }
    return Brushes.Transparent;
}


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
