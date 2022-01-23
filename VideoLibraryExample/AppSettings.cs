namespace VideoLibraryExample;

public record class AppSettings
{
    public string TargetBookmarkName { get; set; } = string.Empty;
    public string TargetBookmarkFile { get; set; } = string.Empty;
    public string DestinationPath { get; set; } = string.Empty;
}