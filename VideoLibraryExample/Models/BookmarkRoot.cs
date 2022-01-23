namespace VideoLibraryExample.Models;

public class BookmarkRoot
{
    [JsonProperty("bookmark_bar")]
    public BookmarkBar? BookmarkBar { get; set; } = null;
}