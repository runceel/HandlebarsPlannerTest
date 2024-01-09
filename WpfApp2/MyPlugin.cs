using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Diagnostics;

namespace WpfApp2;
public class MyPlugin
{
    [KernelFunction]
    [Description("Sleep")]
    public async Task SleepAsync([Description("The number of seconds to sleep.")]int seconds)
    {
        Debug.WriteLine($"SleepAsync(seconds: {seconds}) started.");
        await Task.Delay(seconds * 1000).ConfigureAwait(false);
        Debug.WriteLine($"SleepAsync(seconds: {seconds}) finished.");
    }
}
