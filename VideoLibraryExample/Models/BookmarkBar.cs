namespace VideoLibraryExample.Models;

public class BookmarkBar
{
    [JsonProperty("children")]
    public IEnumerable<BookmarkBarChild> Children { get; set; } = Array.Empty<BookmarkBarChild>();
}