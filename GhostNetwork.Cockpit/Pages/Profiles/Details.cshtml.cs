using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GhostNetwork.Cockpit.Pages;

public class DetailsModel : PageModel
{
    private readonly ProfilesService service;
    private readonly NewsFeedService newsFeedService;
    public Profile? Profile { get; set; }
    public IEnumerable<Publication> News { get; set; }

    public DetailsModel(
        ProfilesService service,
        NewsFeedService newsFeedService)
    {
        this.service = service;
        this.newsFeedService = newsFeedService;
        News = Enumerable.Empty<Publication>();
    }

    public async Task OnGetAsync([FromRoute] string id)
    {
        Profile = await service.GetByIdAsync(id);
        News = await newsFeedService.GetByUserAsync(id);
    }
}
