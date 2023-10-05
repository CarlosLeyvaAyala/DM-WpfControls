using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DM_WpfControls.Controls;

public enum IconPosition {
  Top,
  Left
}

public partial class IconRadioButton : UserControl {
  public IconRadioButton() => InitializeComponent();

  #region DependencyProperty : IconPosition
  /// <summary>
  /// Indicates icon placement.
  /// </summary>
  public IconPosition IconPosition {
    get => (IconPosition)GetValue(IconPositionProperty);
    set {
      IconPosPropertyChanged(value);
      SetValue(IconPositionProperty, value);
    }
  }

  [Category("Icon")]
  public static readonly DependencyProperty IconPositionProperty
      = DependencyProperty.Register(
        nameof(IconPosition),
        typeof(IconPosition),
        typeof(IconRadioButton),
        new PropertyMetadata(IconPosition.Top, IconPosPropertyChanged));

  private static void IconPosPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
    ((IconRadioButton)d).IconPosPropertyChanged((IconPosition)e.NewValue);

  void IconPosPropertyChanged(IconPosition pos) {
    (var pO, var icHA, var icVA, var edM) = pos switch {
      IconPosition.Top => (Orientation.Vertical, HorizontalAlignment.Center, VerticalAlignment.Top, new Thickness(0, 4, 0, 0)),
      IconPosition.Left => (Orientation.Horizontal, HorizontalAlignment.Left, VerticalAlignment.Bottom, new Thickness(4, 0, 0, 0)),
      _ => throw new ArgumentOutOfRangeException(null, nameof(IconPosition)),
    };

    panel.Orientation = pO;
    piIcon.HorizontalAlignment = icHA;
    piIcon.VerticalAlignment = icVA;
    edtName.Margin = edM;
  }
  #endregion

  #region DependencyProperty : IconKind
  public PackIconKind IconKind {
    get => (PackIconKind)GetValue(IconKindProperty);
    set {
      IconKindPropertyChanged(value);
      SetValue(IconKindProperty, value);
    }
  }

  [Category("Icon")]
  public static readonly DependencyProperty IconKindProperty
      = DependencyProperty.Register(
        nameof(IconKind),
        typeof(PackIconKind),
        typeof(IconRadioButton),
        new PropertyMetadata(default(PackIconKind), propertyChangedCallback: IconKindPropertyChanged));

  static void IconKindPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
    ((IconRadioButton)d).IconKindPropertyChanged((PackIconKind)e.NewValue);
  void IconKindPropertyChanged(PackIconKind v) => piIcon.Kind = v;
  #endregion

  #region DependencyProperty : Text
  /// <summary>
  /// Text.
  /// </summary>
  public string Text {
    get => (string)GetValue(TextProperty);
    set {
      TextPropertyChanged(value);
      SetValue(TextProperty, value);
    }
  }

  [Category("Text")]
  public static readonly DependencyProperty TextProperty
      = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(IconRadioButton),
        new PropertyMetadata(default(string), propertyChangedCallback: TextPropertyChanged));

  static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
    ((IconRadioButton)d).TextPropertyChanged((string)e.NewValue);
  void TextPropertyChanged(string v) => edtName.Text = v;
  #endregion

  #region DependencyProperty : GroupName
  public string GroupName {
    get => (string)GetValue(GroupNameProperty);
    set {
      GroupNamePropertyChanged(value);
      SetValue(GroupNameProperty, value);
    }
  }

  [Category("Common")]
  public static readonly DependencyProperty GroupNameProperty
      = DependencyProperty.Register(
        nameof(GroupName),
        typeof(string),
        typeof(IconRadioButton),
        new PropertyMetadata(default(string), propertyChangedCallback: GroupNamePropertyChanged));

  static void GroupNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
    ((IconRadioButton)d).GroupNamePropertyChanged((string)e.NewValue);
  void GroupNamePropertyChanged(string v) => rb.GroupName = v;
  #endregion

  #region DependencyProperty : IsChecked
  public bool IsChecked {
    get => (bool)GetValue(IsCheckedProperty);
    set {
      IsCheckedPropertyChanged(value);
      SetValue(IsCheckedProperty, value);
    }
  }

  [Category("Common")]
  public static readonly DependencyProperty IsCheckedProperty
      = DependencyProperty.Register(
        nameof(IsChecked),
        typeof(bool),
        typeof(IconRadioButton),
        new PropertyMetadata(default(bool), propertyChangedCallback: IsCheckedPropertyChanged));

  static void IsCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
    ((IconRadioButton)d).IsCheckedPropertyChanged((bool)e.NewValue);
  void IsCheckedPropertyChanged(bool v) => rb.IsChecked = v;
  #endregion

  #region Event : ClickEvent
  public event RoutedEventHandler Click {
    add => AddHandler(ClickEvent, value);
    remove => RemoveHandler(ClickEvent, value);
  }

  public static readonly RoutedEvent ClickEvent
      = EventManager.RegisterRoutedEvent(
        nameof(Click),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(IconRadioButton));

  protected virtual void InternalClick() => RaiseEvent(new RoutedEventArgs(ClickEvent, this));

  private void OnClick(object sender, RoutedEventArgs e) {
    InternalClick();
    e.Handled = true;
  }
  #endregion
}
