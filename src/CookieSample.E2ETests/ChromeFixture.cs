using System;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CookieSample.E2ETests;

public class ChromeFixture : BrowserFixture
{
    protected override IWebDriver CreateDriver()
    {
        var opts = new ChromeOptions();

        // Ignore self-signed certificate warnings
        opts.AddArgument("--ignore-certificate-errors");

        // 「...が次の許可を求めています」ダイアログを非表示
        opts.AddArgument("--disable-notifications");

        opts.AddArguments("--incognito");

        // Comment this out if you want to watch or interact with the browser (e.g. for debugging)
        if (!Debugger.IsAttached)
        {
            opts.AddArgument("--headless=new");

            // ヘッドレスの規定値は800 x 600。リサイズ不可でログインボタン等が押せないので初期値を指定。
            opts.AddArgument("--window-size=1920,1080");
        }

        var downloadDir = Path.Combine(TestDllDir, "download", "chrome");
        Directory.CreateDirectory(downloadDir);
        DownloadDir = downloadDir;

        opts.AddUserProfilePreference("download.default_directory", DownloadDir);
        opts.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);

        var driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), opts, TimeSpan.FromSeconds(60));
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        return driver;
    }
}
