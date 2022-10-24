namespace GhostNetwork.Cockpit.Pages;

public class ProfilesService
{
    private readonly HttpClient client;

    public ProfilesService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<Profile> GetByIdAsync(string id)
    {
        var response = await client.GetAsync($"/profiles/{id}");
        var profile = await response.Content.ReadFromJsonAsync<Profile>();
        
        return profile!;
    }
    
    public async Task<(IEnumerable<Profile>, long)> SearchAsync(long skip, int take)
    {
        var response = await client.GetAsync($"/profiles?skip={skip}&take={take}");
        var profiles = await response.Content.ReadFromJsonAsync<IEnumerable<Profile>>();
        
        return (profiles!, long.Parse(response.Headers.GetValues("X-Total-Count").First()));
    }
}