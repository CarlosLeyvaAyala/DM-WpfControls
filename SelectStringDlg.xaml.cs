using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM_WpfControls;

public partial class SelectStringDlg : Window {
  public SelectStringDlg() => InitializeComponent();

  public static void Execute(Window owner, SelectStringDlgParams p) {
    var dlg = new SelectStringDlg {
      Owner = owner,
      Width = p.Width ?? 400,
      Height = p.Height ?? 500
    };
    dlg.Init(p);

    var result = dlg.ShowDialog();

    if (result == true)
      p.OnOk(dlg.lstSelect.SelectedItems.OfType<DisplayStrings>().Select(i => i.Value).ToList());
    else
      p.OnCancel?.Invoke();
  }

  #region Setup
  void Init(SelectStringDlgParams p) {
    ctx.Sorted = p.Sorted ?? true;
    ctx.Items = p.Values;
    Title = p.Title ?? Title;
    lstSelect.SelectionMode = p.SelectionMode ?? SelectionMode.Single;
    tbFilterByRegex.Visibility = p.RegexButton_Show == true ? Visibility.Visible : Visibility.Collapsed;
    tbFilterByRegex.IsChecked = p.RegexButton_Checked == true;
    SetFilterByRegex(p.RegexButton_Checked == true);
    // Assume the filter will be shown if the regex button is explicitly asked for
    var showSearch = p.RegexButton_Show == true ? true : p.ShowSearchFilter;
    grdSearch.Visibility = showSearch == true ? Visibility.Visible : Visibility.Collapsed;
    btnOk.Background = p.OkButton_Color ?? btnOk.Background;
  }

  void SetFilterByRegex(bool value) {
    regexRule.IsActive = value;
    edtFilter.GetBindingExpression(TextBox.TextProperty).UpdateSource();
    ctx.UseRegex = value;
  }
  #endregion

  #region Dialog events
  private void OnFilterNameByRegexClick(object sender, RoutedEventArgs e) => SetFilterByRegex(tbFilterByRegex.IsChecked == true);

  private void OnOk(object sender, RoutedEventArgs e) {
    if (lstSelect.SelectedItems == null) return;
    DialogResult = true;
  }

  private void OnSelectDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
    if (lstSelect.SelectedItems == null) return;
    DialogResult = true;
  }
  #endregion
}

public record SelectStringDlgParams {
  public required List<DisplayStrings> Values { get; init; }
  public required Action<List<string>> OnOk { get; init; }
  public Action? OnCancel { get; init; }
  public string? Title { get; init; }
  public SelectionMode? SelectionMode { get; init; }
  public bool? RegexButton_Show { get; init; }
  public bool? RegexButton_Checked { get; init; }
  public bool? ShowSearchFilter { get; init; }
  public bool? Sorted { get; init; }
  public Brush? OkButton_Color { get; init; }
  public double? Height { get; init; }
  public double? Width { get; init; }
}


class SelectStrDlgCtx : INotifyPropertyChanged {
  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
  public void OnPropertyChanged(string e) => OnPropertyChanged(new PropertyChangedEventArgs(e));
  public bool Sorted { get; set; }

  string filter = "";
  public string Filter {
    get => filter;
    set {
      filter = value;
      OnPropertyChanged(nameof(Filter));
      OnPropertyChanged(nameof(Items));
    }
  }

  private bool useRegex;
  public bool UseRegex {
    get => useRegex;
    set {
      useRegex = value;
      OnPropertyChanged(nameof(UseRegex));
      OnPropertyChanged(nameof(Items));
    }
  }

  List<DisplayStrings> items = new();
  public List<DisplayStrings> Items {
    get {
      Func<string, bool> GetFilterByRegex() {
        var rx = new Regex(filter, RegexOptions.IgnoreCase);
        return s => rx.Match(s).Success;
      }

      var compare = useRegex ? GetFilterByRegex() : s => s.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
      var itms = string.IsNullOrEmpty(filter) ? items : items.AsParallel().Where(i => compare(i.Display)).ToList();
      return Sorted ? itms.OrderBy(i => i.Display).ToList() : itms;
    }
    set {
      items = value;
      OnPropertyChanged(nameof(Items));
    }
  }
}

public class DisplayStrings {
  public string Display { get; set; } = "";
  public string Value { get; set; } = "";
  public string CenterRightDetail { get; set; } = "";
  public bool HasCRD => !string.IsNullOrEmpty(CenterRightDetail);
  public override string ToString() => Display;
  public DisplayStrings(string display, string value, string centerRightDetail = "") {
    Display = display;
    Value = value;
    CenterRightDetail = centerRightDetail;
  }
}