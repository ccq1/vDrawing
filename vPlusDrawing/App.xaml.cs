using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using vPlusDrawing.ViewModels;
using vPlusDrawing.Views;

namespace vPlusDrawing;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Services = ConfigureServices();
    }
    public new static App Current => (App)Application.Current;
    public IServiceProvider Services { get; set; }
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>(sp => new MainWindow { DataContext = sp.GetService<MainWindowViewModel>() });
        services.AddSingleton<ImageUtils>();
        return services.BuildServiceProvider();
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = Services.GetService<MainWindow>();
        mainWindow.Show();
    }
}

