using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CookieSample.Pages.Edge;

public class EdgeFormModel : PageModel
{
    public string Name { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }
}
