using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DM_WpfControls;

public partial class SelectStringDlg : Window {
  public SelectStringDlg() => InitializeComponent();
  private void OnOk(object sender, RoutedEventArgs e) {
    if (lstSelect.SelectedItems == null) return;
    DialogResult = true;
  }

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

  void Init(SelectStringDlgParams p) {
    ctx.Items = p.Values;
    Title = p.Title ?? Title;
    lstSelect.SelectionMode = p.SelectionMode ?? SelectionMode.Single;
  }
}

public record SelectStringDlgParams {
  public required List<DisplayStrings> Values { get; init; }
  public required Action<List<string>> OnOk { get; init; }
  public Action? OnCancel { get; init; }
  public string? Title { get; init; }
  public SelectionMode? SelectionMode { get; init; }
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

  List<DisplayStrings> items = new();
  public List<DisplayStrings> Items {
    get => string.IsNullOrEmpty(filter) ? items.OrderBy(i => i.Display).ToList() :
      items.AsParallel()
      .Where(i => i.Display.Contains(filter, StringComparison.CurrentCultureIgnoreCase))
      .OrderBy(i => i.Display)
      .ToList();
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