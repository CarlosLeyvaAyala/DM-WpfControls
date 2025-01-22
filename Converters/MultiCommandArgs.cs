using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DM_WpfControls.Converters;
public class MultiCommandArgs : IMultiValueConverter {
    public object Convert(object[] values, Type _, object __, CultureInfo ___) => values.Clone();
    public object[] ConvertBack(
        object value,
        Type[] targetTypes,
        object parameter,
        CultureInfo culture) => throw new NotImplementedException();
}

