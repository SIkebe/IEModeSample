using System;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace CookieSample.E2ETests;

public class InternetExplorerFixture : BrowserFixture
{
    protected override IWebDriver CreateDriver()
    {
        var opts = new InternetExplorerOptions
        {
            AttachToEdgeChrome = true,
            EdgeExecutablePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
            IgnoreZoomLevel = true,
            IntroduceInstabilityByIgnoringProtectedModeSettings = true,
            UnhandledPromptBehavior = UnhandledPromptBehavior.Accept,
        };

        opts.SetLoggingPreference("driver", LogLevel.Debug);
        opts.SetLoggingPreference("browser", LogLevel.Debug);

        var driver = new InternetExplorerDriver(InternetExplorerDriverService.CreateDefaultService(), opts, TimeSpan.FromSeconds(3));
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        return driver;
    }
}
