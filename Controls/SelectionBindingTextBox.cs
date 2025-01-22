using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DM_WpfControls.Controls;

public class SelectionBindingTextBox : TextBox {
    public static readonly DependencyProperty BindableSelectionStartProperty =
        DependencyProperty.Register(
        "BindableSelectionStart",
        typeof(int),
        typeof(SelectionBindingTextBox),
        new PropertyMetadata(OnBindableSelectionStartChanged));

    public static readonly DependencyProperty BindableSelectionLengthProperty =
        DependencyProperty.Register(
        "BindableSelectionLength",
        typeof(int),
        typeof(SelectionBindingTextBox),
        new PropertyMetadata(OnBindableSelectionLengthChanged));

    public static readonly DependencyProperty BindableVerticalOffsetProperty =
        DependencyProperty.Register(
        "BindableVerticalOffset",
        typeof(double),
        typeof(SelectionBindingTextBox),
        new PropertyMetadata(OnBindableVerticalOffsetChanged));

    public SelectionBindingTextBox() : base() => SelectionChanged += OnSelectionChanged;

    public int BindableSelectionStart {
        get => (int)GetValue(BindableSelectionStartProperty);
        set => SetValue(BindableSelectionStartProperty, value);
    }

    public int BindableSelectionLength {
        get => (int)GetValue(BindableSelectionLengthProperty);
        set => SetValue(BindableSelectionLengthProperty, value);
    }

    public double BindableVerticalOffset {
        get => (double)GetValue(BindableVerticalOffsetProperty);
        set => SetValue(BindableVerticalOffsetProperty, value);
    }

    private static void OnBindableVerticalOffsetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) {
        Console.WriteLine("OnBindableVerticalOffsetChanged");
        if (dependencyObject is not SelectionBindingTextBox textBox) return;
        textBox.ScrollToVerticalOffset((double)args.NewValue);
        Console.WriteLine($"New value {(double)args.NewValue}");
    }

    private static void OnBindableSelectionStartChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) {
        if (dependencyObject is not SelectionBindingTextBox textBox) return;

        int newValue = (int)args.NewValue;
        textBox.SelectionStart = newValue;
    }

    private static void OnBindableSelectionLengthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) {
        if (dependencyObject is not SelectionBindingTextBox textBox) return;

        int newValue = (int)args.NewValue;
        textBox.SelectionLength = newValue;
    }

    private void OnSelectionChanged(object sender, RoutedEventArgs e) {
        BindableSelectionStart = SelectionStart;
        BindableSelectionLength = SelectionLength;
        BindableVerticalOffset = VerticalOffset;
    }
}