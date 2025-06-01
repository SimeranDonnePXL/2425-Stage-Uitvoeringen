using System.Diagnostics;

namespace MFrontend;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }

    protected override void OnSleep()
    {
        base.OnSleep();
        for (int i = 0; i < 100; i++)
        {
            Debug.WriteLine(i + 1 + " seconde");
            Task.Delay(1000);
        }
    }

    protected override void OnStart()
    {
        base.OnStart();

        // Start a new task on a background thread
        Task.Run(async () =>
        {
            for (int i = 0; i < 100; i++)
            {
                Debug.WriteLine($"{i + 1} seconde");
                await Task.Delay(1000); // Wait for 1 second
            }
            Debug.WriteLine("Counting finished!");
        });

        // The OnStart method will return immediately,
        // allowing the UI to load and remain responsive.
    }
}