using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace CookieSample.E2ETests;

public abstract class BrowserFixture : IDisposable
{
    private bool _disposedValue = false;

    public BrowserFixture()
    {
        // Ensure driver can start under proxy environment
        Environment.SetEnvironmentVariable("no_proxy", "localhost");

        var logDir = Path.Combine(TestDllDir, "logs");
        Directory.CreateDirectory(logDir);
        LogDir = logDir;

        Driver = CreateDriver();
    }

    ~BrowserFixture()
    {
        Dispose(false);
    }

    public IWebDriver Driver { get; set; }
    public static string TestDllDir => Path.GetDirectoryName(Assembly.GetAssembly(typeof(BrowserFixture))!.Location)!;
    public string DownloadDir { get; set; }
    public string LogDir { get; set; }

    protected abstract IWebDriver CreateDriver();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
            }

            try
            {
                // For trouble shooting purpose
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                var file = Path.Combine(LogDir, $"{DateTime.Now:yyyyMMddss}.png");
                screenshot.SaveAsFile(file);
            }
            catch
            {
            }

            Driver?.Quit();
            Driver?.Dispose();
            _disposedValue = true;
        }
    }
}
