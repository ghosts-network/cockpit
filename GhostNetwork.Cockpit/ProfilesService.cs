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

public class NewsFeedService
{
    private readonly HttpClient client;

    public NewsFeedService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<IEnumerable<Publication>> GetByUserAsync(string userId)
    {
        var response = await client.GetAsync($"/{userId}");
        var publications = await response.Content.ReadFromJsonAsync<IEnumerable<Publication>>();

        return publications!;
    }
}

public record Publication(string Id, string Content, UserInfo Author);

public record UserInfo(string Id, string FullName);
