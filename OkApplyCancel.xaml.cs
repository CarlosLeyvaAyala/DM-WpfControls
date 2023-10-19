using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DM_WpfControls;
/// <summary>
/// Interaction logic for OkApplyCancel.xaml
/// </summary>
public partial class OkApplyCancel : UserControl {
  public event RoutedEventHandler? OkClick;
  public event RoutedEventHandler? ApplyClick;
  public event RoutedEventHandler? CancelClick;
  public OkApplyCancel() => InitializeComponent();
  private void OnOk(object sender, RoutedEventArgs e) => OkClick?.Invoke(this, e);
  private void OnApply(object sender, RoutedEventArgs e) => ApplyClick?.Invoke(this, e);
  private void OnCancel(object sender, RoutedEventArgs e) => CancelClick?.Invoke(this, e);

  #region Property : ShowApply
  public bool ShowApply {
    get { return (bool)GetValue(ShowApplyProperty); }
    set { SetValue(ShowApplyProperty, value); }
  }

  public static readonly DependencyProperty ShowApplyProperty =
      DependencyProperty.Register(
        nameof(ShowApply),
        typeof(bool),
        typeof(OkApplyCancel),
        new PropertyMetadata(false, ShowApplyPropertyChanged));
  static void ShowApplyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
    ((OkApplyCancel)d).ShowApplyPropertyChanged((bool)e.NewValue);
  void ShowApplyPropertyChanged(bool value) => btnApply.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
  #endregion

  #region Property : OkBackground
  public Brush OkBackground {
    get { return (Brush)GetValue(OkBackgroundProperty); }
    set { SetValue(OkBackgroundProperty, value); }
  }

  public static readonly DependencyProperty OkBackgroundProperty =
      DependencyProperty.Register(
        nameof(OkBackground),
        typeof(Brush),
        typeof(OkApplyCancel),
        new PropertyMetadata(default(Brush), OkBackgroundPropertyChanged));
  static void OkBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
    ((OkApplyCancel)d).OkBackgroundPropertyChanged((Brush)e.NewValue);
  void OkBackgroundPropertyChanged(Brush value) => btnOk.Background = value;
  #endregion
}
