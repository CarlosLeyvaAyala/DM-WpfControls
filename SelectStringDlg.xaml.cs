using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DM_WpfControls;

public partial class SelectStringDlg : Window {
  public SelectStringDlg() => InitializeComponent();

  public static void Execute(Window owner, SelectStringDlgParams p) {
    var dlg = new SelectStringDlg {
      Owner = owner,
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
    ctx.Items = p.Values;
    Title = p.Title ?? Title;
    lstSelect.SelectionMode = p.SelectionMode ?? SelectionMode.Single;
    tbFilterByRegex.Visibility = p.RegexButton_Show == true ? Visibility.Visible : Visibility.Collapsed;
    tbFilterByRegex.IsChecked = p.RegexButton_Checked == true;
    SetFilterByRegex(p.RegexButton_Checked == true);
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
}


class SelectStrDlgCtx : INotifyPropertyChanged {
  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
  public void OnPropertyChanged(string e) => OnPropertyChanged(new PropertyChangedEventArgs(e));

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

      return
        string.IsNullOrEmpty(filter) ? items.OrderBy(i => i.Display).ToList() :
        items.AsParallel()
        .Where(i => compare(i.Display))
        .OrderBy(i => i.Display)
        .ToList();
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