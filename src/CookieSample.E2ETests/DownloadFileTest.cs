using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;
using Xunit.Abstractions;

namespace CookieSample.E2ETests
{
    public class DownloadFileTest : IClassFixture<ChromeFixture>, IClassFixture<EdgeFixture>
    {
        private readonly BrowserFixture _chromeFixture;
        private readonly BrowserFixture _edgeFixture;
        private readonly ITestOutputHelper _helper;

        public DownloadFileTest(ChromeFixture chromeFixture, EdgeFixture edgeFixture, ITestOutputHelper helper)
        {
            _chromeFixture = chromeFixture;
            _edgeFixture = edgeFixture;
            _helper = helper;
        }

        public static IEnumerable<object[]> AllBrowsers()
        {
            yield return new[] { nameof(ChromeFixture) };
            yield return new[] { nameof(EdgeFixture) };
        }

        [Theory, MemberData(nameof(AllBrowsers))]
        public async Task Should_Download_Excel_File(string fixtureName)
        {
            var fixture = fixtureName is nameof(ChromeFixture) ? _chromeFixture : _edgeFixture;
            var wait = new WebDriverWait(fixture.Driver, TimeSpan.FromSeconds(10));

            fixture.Driver.Navigate().GoToUrl("https://localhost:5001/");

            // "Edge/Privacy" リンク押下
            fixture.Driver.FindElement(By.XPath("/html/body/div/main/div[2]/div[3]/a/div/p")).Click();
            wait.Until(webDriver => webDriver.FindElement(By.LinkText("Download")).Displayed);

            fixture.Driver.FindElement(By.LinkText("Download")).Click();
            await Task.Delay(2000);

            var filePath = Path.Combine(fixture.DownloadDir, "Grid.xlsx");
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
    }
}
