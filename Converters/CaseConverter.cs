using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace DM_WpfControls.Converters;
//internal class CaseConverter {
//}

//<!--
//Source - https://stackoverflow.com/a/29841405
//Posted by Max, modified by community.See post 'Timeline' for change history
//Retrieved 2026-07-18, License - CC BY-SA 3.0
//-->

public class CaseConverter : IValueConverter {
    public CharacterCasing Case { get; set; }

    public CaseConverter() {
        Case = CharacterCasing.Upper;
    }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
        if (value is string str) {
            return Case switch {
                CharacterCasing.Lower => str.ToLower(),
                CharacterCasing.Normal => str,
                CharacterCasing.Upper => str.ToUpper(),
                _ => str,
            };
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
        throw new NotImplementedException();
    }
}
