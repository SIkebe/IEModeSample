using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
using Xunit.Abstractions;

namespace CookieSample.E2ETests;

public class InternetExplorerTest : IClassFixture<InternetExplorerFixture>
{
    private readonly IWebDriver _driver;
    private readonly ITestOutputHelper _helper;

    public InternetExplorerTest(InternetExplorerFixture internetExplorerFixture, ITestOutputHelper helper)
    {
        _driver = internetExplorerFixture.Driver;
        _helper = helper;
    }

    [Fact]
    public async Task Should_Download_Excel_File()
    {
        _driver.Navigate().GoToUrl("https://localhost:5001/");
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        // "Edge/Privacy" リンク押下
        _driver.FindElement(By.XPath("/html/body/div/main/div[2]/div[3]/a/div/p")).Click();
        wait.Until(webDriver => webDriver.FindElement(By.LinkText("Download")).Displayed);

#pragma warning disable 4014
        Task.Run(() =>
        {
            try
            {
                _driver.FindElement(By.LinkText("Download")).Click();
            }
            catch (Exception)
            {
            }
        }).ConfigureAwait(false);
#pragma warning restore 4014

        await Task.Delay(2000);

        // Alt + s
        SendKeys.SendWait("%(s)");
        await Task.Delay(2000);

        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "CookieSample.Pages.Edge.PrivacyModel.Grid.xlsx");
        var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheets.Single(s => s.Name == "Grid");

        Assert.Equal("1", worksheet.Cell("A2").Value);
        Assert.Equal("田中太郎", worksheet.Cell("B2").Value);
        Assert.Equal("東京", worksheet.Cell("C2").Value);
        Assert.Equal("日本", worksheet.Cell("D2").Value);

        Assert.Equal("2", worksheet.Cell("A3").Value);
        Assert.Equal("山田花子", worksheet.Cell("B3").Value);
        Assert.Equal("熊本", worksheet.Cell("C3").Value);
        Assert.Equal("日本", worksheet.Cell("D3").Value);

        File.Delete(filePath);
    }

    [Fact]
    public void Should_Show_Name()
    {
        using var fixture = new InternetExplorerFixture();
        var driver = fixture.Driver;
        driver.Navigate().GoToUrl("https://localhost:5001/");

        // "IE/Form" リンク押下
        driver.FindElement(By.XPath("/html/body/div/main/div[2]/div[6]/a/div/p")).Click();

        driver.FindElement(By.Id("Name")).Clear();
        driver.FindElement(By.Id("Name")).SendKeys("マイクロ太郎");

        driver.FindElement(By.Id("submit")).Click();

        var message = _driver.FindElement(By.Id("name")).Text;
        Assert.Equal("Hello マイクロ太郎 !", message);
    }
}
