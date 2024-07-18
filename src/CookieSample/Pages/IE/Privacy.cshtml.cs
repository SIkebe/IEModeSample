using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CookieSample.Pages.IE;

public class PrivacyModel() : PageModel
{
    public string Name { get; set; }

    public void OnGet()
    {
    }

    public void OnPost([FromForm] string name)
    {
        Name = name;
    }
}
