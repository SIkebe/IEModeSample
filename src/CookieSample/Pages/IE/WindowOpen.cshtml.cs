using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CookieSample.Pages.IE;

public class WindowOpenModel : PageModel
{
    private readonly ILogger<WindowOpenModel> _logger;

    public WindowOpenModel(ILogger<WindowOpenModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}
