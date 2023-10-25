using HandyControl.Controls;
using HandyControl.Template.Controls;
using HandyControl.Template.Controls.Contracts;
using HandyControl.Template.ViewModels;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HandyControl.Themes;

namespace HandyControl.Template;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INavigationWindow 
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow(MainWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);
    }

    #region INavigationWindow methods

    public INavigationView GetNavigation() => RootNavigation;

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService) =>
        RootNavigation.SetPageService(pageService);

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    #endregion INavigationWindow methods

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }

    #region 切换主题

    private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;

    private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is Button button)
        {
            PopupConfig.IsOpen = false;
            if (button.Tag is ApplicationTheme tag)
            {
                ((App)Application.Current).UpdateTheme(tag);
            }
            else if (button.Tag is Brush accentTag)
            {
                ((App)Application.Current).UpdateAccent(accentTag);
            }
            else if (button.Tag is "Picker")
            {
                var picker = SingleOpenHelper.CreateControl<ColorPicker>();
                var window = new PopupWindow
                {
                    PopupElement = picker,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    AllowsTransparency = true,
                    WindowStyle = WindowStyle.None,
                    MinWidth = 0,
                    MinHeight = 0,
                    Title = "选择主题颜色"
                };

                picker.SelectedColorChanged += delegate
                {
                    ((App)Application.Current).UpdateAccent(picker.SelectedBrush);
                    window.Close();
                };
                picker.Canceled += delegate { window.Close(); };
                window.Show();
            }
        }
    }

    #endregion
}
