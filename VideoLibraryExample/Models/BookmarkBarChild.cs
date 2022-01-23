namespace VideoLibraryExample.Models;

public class BookmarkBarChild
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;
    [JsonProperty("children")]
    public IEnumerable<BookmarkBarChild> Children { get; set; } = Array.Empty<BookmarkBarChild>();
}