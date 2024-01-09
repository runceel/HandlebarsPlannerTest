using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.Planning.Handlebars;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer = new();
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _timer.Tick += TimerTick;
        _timer.Interval = TimeSpan.FromMilliseconds(10);
        _timer.Start();
    }

    private void TimerTick(object? sender, EventArgs e)
    {
        var r = Random.Shared;
        rect.Fill = new SolidColorBrush(
            Color.FromRgb(
                (byte)r.Next(256), 
                (byte)r.Next(256), 
                (byte)r.Next(256)));
    }

    private async void ExecuteHandlebarsPlannerButtonClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var kernel = CreateKernel();
            var planner = new HandlebarsPlanner();
            textBlockStatusMessage.Text = "Planning...";
            var plan = await planner.CreatePlanAsync(kernel, "Speep 10 seconds.");
            textBlockStatusMessage.Text = $"""
            Executing...

            {plan}
            """;
            await Task.Delay(100); // to see the status message

            textBlockStatusMessage.Text = await plan.InvokeAsync(kernel);
        }
        catch (Exception ex)
        {
            textBlockStatusMessage.Text = ex.ToString();
        }
    }

    private Kernel CreateKernel()
    {
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(textBoxDeployName.Text, textBoxEndpoint.Text, textBoxApiKey.Text);
        builder.Plugins.AddFromType<MyPlugin>();
        return builder.Build();
    }

    private void Window_Unloaded(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
    }

    private async void ExecuteFunctionCallingStepwisePlannerButtonClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var kernel = CreateKernel();
            var planner = new FunctionCallingStepwisePlanner();
            textBlockStatusMessage.Text = "Executing...";
            var result = await planner.ExecuteAsync(kernel, "Speep 10 seconds.");
            textBlockStatusMessage.Text = result.FinalAnswer;
        }
        catch (Exception ex)
        {
            textBlockStatusMessage.Text = ex.ToString();
        }
    }
}
