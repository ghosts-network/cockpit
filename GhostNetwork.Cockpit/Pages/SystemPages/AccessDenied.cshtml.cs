using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GhostNetwork.Cockpit.Pages.Account;

[AllowAnonymous]
public class AccessDenied : PageModel
{
    public void OnGet()
    {
        
    }
}