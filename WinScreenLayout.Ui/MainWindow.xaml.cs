using System.Windows;
using WinScreenLayout.Core;
using System.Windows.Controls;


namespace WinScreenLayout.Ui;

public partial class MainWindow : Window
{
    private readonly LayoutService _service = new();

    public MainWindow()
    {
        InitializeComponent();
        ReloadLayouts();
    }

    private void ReloadLayouts()
    {
        LayoutsList.ItemsSource = _service.GetLayouts();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var name = Prompt("Layout name:");
        if (string.IsNullOrWhiteSpace(name)) return;

        _service.Save(name);
        ReloadLayouts();
    }

    private void Apply_Click(object sender, RoutedEventArgs e)
    {
        if (LayoutsList.SelectedItem is not string name) return;

        _service.Apply(name, RestoreCheck.IsChecked == true);
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (LayoutsList.SelectedItem is not string name) return;

        if (MessageBox.Show($"Delete layout '{name}'?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes)
            return;

        _service.Delete(name);
        ReloadLayouts();
    }

    private static string? Prompt(string text)
    {
        var dlg = new Window
        {
            Title = text,
            Width = 300,
            Height = 140,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ResizeMode = ResizeMode.NoResize
        };

        var panel = new StackPanel { Margin = new Thickness(10) };
        var box = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
        var ok = new Button { Content = "OK", IsDefault = true, Width = 70, HorizontalAlignment = HorizontalAlignment.Right };

        ok.Click += (_, _) => dlg.DialogResult = true;

        panel.Children.Add(box);
        panel.Children.Add(ok);
        dlg.Content = panel;

        return dlg.ShowDialog() == true ? box.Text : null;
    }
}
