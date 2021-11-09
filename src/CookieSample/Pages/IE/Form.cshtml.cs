using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CookieSample.Pages.IE;

public class IEFormModel : PageModel
{
    public string Name { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }
}
