using System.Collections.Generic;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CookieSample.Pages.Edge;

[IgnoreAntiforgeryToken]
public class PrivacyModel() : PageModel
{
    public string Name { get; set; }

    private IEnumerable<Customer> Customers { get; set; } =
    [
        new Customer(1, "田中太郎", "東京", "日本"),
        new Customer(2, "山田花子", "熊本", "日本"),
    ];

    public void OnGet()
    {
    }

    public void OnPost([FromForm] string name)
    {
        Name = name;
    }

    public IActionResult OnGetDownload()
    {
        var dt = new DataTable("Grid");
        dt.Columns.AddRange(
        [
            new DataColumn("CustomerId"),
            new DataColumn("ContactName"),
            new DataColumn("City"),
            new DataColumn("Country")
        ]);

        foreach (var customer in Customers)
        {
            dt.Rows.Add(customer.CustomerID, customer.ContactName, customer.City, customer.Country);
        }

        using var wb = new XLWorkbook();
        wb.Worksheets.Add(dt);
        using var stream = new MemoryStream();
        wb.SaveAs(stream);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
    }
}

record Customer(int CustomerID, string ContactName, string City, string Country);
