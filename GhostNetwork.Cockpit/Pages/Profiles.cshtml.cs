using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GhostNetwork.Cockpit.Pages;

public class ProfilesModel : PageModel
{
    private readonly ProfilesService service;
    public IEnumerable<Profile> Profiles { get; set; }
    public long TotalCount { get; set; }
    public long Skip = 0;
    public int Take = 20;

    public ProfilesModel(ProfilesService service)
    {
        this.service = service;
        Profiles = Enumerable.Empty<Profile>();
    }

    public async Task OnGetAsync([FromQuery] long skip = 0, [FromQuery] int take = 20)
    {
        Skip = skip;
        Take = take;
        (Profiles, TotalCount) = await service.SearchAsync(Skip, Take);
    }
}