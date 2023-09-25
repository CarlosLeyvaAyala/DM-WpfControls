using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DM_WpfControls;

public partial class QueryDlg : UserControl {
  public string DialogHostIdentifier { get; set; } = "MainDlgHost";
  public Style TextBoxStyle { set => edtDlgHostText.Style = value; }
  public Style ButonOkStyle { set => btnOk.Style = value; }
  public Style ButonCancelStyle { set => btnCancel.Style = value; }

  public QueryDlg() => InitializeComponent();

  async Task<string?> ExecuteDlg(QueryDlgParams p) {
    ctx.Validators = p.Validators;
    ctx.Value = p.Text ?? "";
    ctx.Hint = p.Hint;
    Focus();
    edtDlgHostText.Focus();
    var result = await DialogHost.Show(this, DialogHostIdentifier);

    return (bool?)result == true ? edtDlgHostText.Text : null;
  }

  public async void Execute(QueryDlgParams p) {
    var s = await ExecuteDlg(p);

    if (!string.IsNullOrEmpty(s)) p.OnOk(s);
    else p.OnCancel?.Invoke();
  }

}

public record QueryDlgParams {
  public required string Hint { get; init; }
  public required Action<string> OnOk { get; init; }
  public string? Text { get; init; }
  public ValidationRule[]? Validators { get; init; }
  public Action? OnCancel { get; init; }
}

class QueryDlgCtx : INotifyPropertyChanged {
  public event PropertyChangedEventHandler? PropertyChanged;
  public void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
  public void OnPropertyChanged(string e) => OnPropertyChanged(new PropertyChangedEventArgs(e));

  string v = "";
  static readonly Thickness defaultButtonMarginTop = new(0, 5, 0, 0);
  public ValidationRule[]? Validators { get; set; } = null;
  bool isValid = true;

  public bool IsValid => !string.IsNullOrEmpty(Value) && isValid;
  public bool HasError => !isValid;
  public Thickness ButtonMarginTop { get; set; } = defaultButtonMarginTop;

  string hint = "";
  public string Hint {
    get => hint; set {
      hint = value;
      OnPropertyChanged(nameof(Hint));
    }
  }
  public string Value {
    get => v;
    set {
      v = value;
      var r = Validators?
        .Select(v => v.Validate(value, null))
        .Where(r => !r.IsValid)
        .ToArray();

      isValid = r == null || r.Length == 0;
      ButtonMarginTop = isValid ? defaultButtonMarginTop : new(0, 13 * (r?.Length ?? 1), 0, 0);
      OnPropertyChanged("");

      if (!isValid && r != null) {
        string m = r.Length > 1 ? r
                                    .Select(v => $"{v.ErrorContent}.")
                                    .Aggregate("", (a, b) => $"{a}\n{b}")
                                    .Trim()
                    : r[0].ErrorContent.ToString() ?? "Unknown error.\nThis should have not being displayed.";
        throw new InvalidEnumArgumentException(m);
      }
    }
  }
}