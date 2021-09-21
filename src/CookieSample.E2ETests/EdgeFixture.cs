using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace CookieSample.E2ETests;

public class EdgeFixture : BrowserFixture
{
    protected override IWebDriver CreateDriver()
    {
        var opts = new EdgeOptions();

        // Ignore self-signed certificate warnings
        opts.AddArgument("--ignore-certificate-errors");

        // 「...が次の許可を求めています」ダイアログを非表示
        opts.AddArgument("--disable-notifications");

        opts.AddArguments("-inprivate");

        // Comment this out if you want to watch or interact with the browser (e.g. for debugging)
        if (!Debugger.IsAttached)
        {
            opts.AddArgument("headless");
        }

        opts.AddUserProfilePreference("download.default_directory", DownloadDir);
        opts.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);

        var driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), opts, TimeSpan.FromSeconds(60));
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        return driver;
    }
}
