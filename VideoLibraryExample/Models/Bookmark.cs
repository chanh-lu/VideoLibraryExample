namespace VideoLibraryExample.Models;

public class Bookmark
{
    [JsonProperty("roots")]
    public BookmarkRoot? Root { get; set; } = null;
}