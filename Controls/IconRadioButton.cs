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

public class IconRadioButton : RadioButton {
  readonly StackPanel panel = new();
  readonly PackIcon icon = new();
  readonly TextBlock txt = new();

  public IconRadioButton() : base() {
    icon.Kind = PackIconKind.Square;
    txt.Text = "OPTION";

    panel.Orientation = Orientation.Vertical;
    panel.Children.Add(icon);
    panel.Children.Add(txt);
    panel.UpdateLayout();
    AddChild(panel);

    IconPosition = IconPosition.Top;
  }

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
      IconPosition.Left => (Orientation.Horizontal, HorizontalAlignment.Left, VerticalAlignment.Center, new Thickness(4, 0, 0, 0)),
      _ => throw new ArgumentOutOfRangeException(null, nameof(IconPosition)),
    };

    panel.Orientation = pO;
    icon.HorizontalAlignment = icHA;
    icon.VerticalAlignment = icVA;
    txt.Margin = edM;
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
  void IconKindPropertyChanged(PackIconKind v) => icon.Kind = v;
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
  void TextPropertyChanged(string v) => txt.Text = v;
  #endregion
}
