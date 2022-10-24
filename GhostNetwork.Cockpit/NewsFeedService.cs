namespace GhostNetwork.Cockpit.Pages;

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