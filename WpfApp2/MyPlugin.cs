using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace WpfApp2;
public class MyPlugin
{
    [KernelFunction]
    [Description("Sleep")]
    public async Task SleepAsync([Description("The number of seconds to sleep.")]int seconds)
    {
        await Task.Delay(seconds * 1000).ConfigureAwait(false);
    }
}
