namespace VideoLibraryExample.DataSources;

public class BookmarksDataSource : IDataSource
{
    private const string TargetBookmarkType = "folder";

    private readonly AppSettings _appSettings;
    private readonly IFileWrapper _fileWrapper;

    public BookmarksDataSource(IConfiguration config, IFileWrapper fileWrapper)
    {
        _appSettings = config == null ? throw new ArgumentNullException(nameof(config)) : config.Get<AppSettings>();
        _fileWrapper = fileWrapper ?? throw new ArgumentNullException(nameof(fileWrapper));
    }

    public IDictionary<string, string> Get()
    {
        var jsonString = _fileWrapper.ReadAllText(_appSettings.TargetBookmarkFile);
        var bookmark = JsonConvert.DeserializeObject<Bookmark>(jsonString);

        if (bookmark == null || bookmark.Root == null || bookmark.Root.BookmarkBar == null)
        {
            throw new Exception($"Failed to deserialize json string to object of type '{nameof(Bookmark)}'");
        }

        var bookmarkFolder = bookmark.Root.BookmarkBar.Children
            .FirstOrDefault(bookmarkBarChild => bookmarkBarChild.Type == TargetBookmarkType && bookmarkBarChild.Name.ToLower() == _appSettings.TargetBookmarkName.ToLower());

        if (bookmarkFolder == null)
        {
            throw new Exception($"Cannot find bookmark '{TargetBookmarkType}' named '{_appSettings.TargetBookmarkName}'");
        }

        var namesAndUrls = bookmarkFolder.Children.Where(item => !string.IsNullOrWhiteSpace(item.Name) && !string.IsNullOrWhiteSpace(item.Url))
            .ToDictionary(item => item.Name,
                          item => item.Url);

        return namesAndUrls;
    }
}