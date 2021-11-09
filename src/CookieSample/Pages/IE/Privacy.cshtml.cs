using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CookieSample.Pages.IE;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public string Name { get; set; }

    public void OnGet()
    {
    }

    public void OnPost([FromForm] string name)
    {
        Name = name;
    }
}
