using VideoLibrary;

namespace VideoLibraryExample;

public class App
{
    private const string M4aExtension = ".m4a";

    private readonly AppSettings _appSettings;
    private readonly IDataSource _source;
    private readonly IFileWrapper _fileWrapper;

    public App(IConfiguration config, IDataSource bookmarksDataSource, IFileWrapper fileWrapper)
    {
        _appSettings = config == null ? throw new ArgumentNullException(nameof(config)) : config.Get<AppSettings>();
        _source = bookmarksDataSource ?? throw new ArgumentNullException(nameof(bookmarksDataSource));
        _fileWrapper = fileWrapper ?? throw new ArgumentNullException(nameof(fileWrapper));
    }

    public void Run()
    {
        var items = GetItems(_source.Get());

        Console.WriteLine($"{items.Count} item(s) to process.");

        foreach ((string name, string url) in items)
        {
            var audio = YouTube.Default.GetAllVideos(url);
            var audios = audio.Where(_ => _.AudioFormat == AudioFormat.Aac && _.AdaptiveKind == AdaptiveKind.Audio).ToList();
            var bytes = audios.FirstOrDefault(x => x.AudioBitrate > 0)?.GetBytes();

            if (bytes != null)
            {
                var path = $"{_appSettings.DestinationPath}\\{name}";
                _fileWrapper.WriteAllBytes(path, bytes);
            }
        }
    }

    private IDictionary<string, string> GetItems(IDictionary<string, string> dataSource)
    {
        var newItems = new Dictionary<string, string>();

        var existingItems = Directory
            .EnumerateFiles(_appSettings.DestinationPath, $"*{M4aExtension}")
            .Select(path => Path.GetFileName(path) ?? string.Empty)
            .ToDictionary(nameWithExtension => nameWithExtension, null);

        foreach ((string name, string url) in dataSource)
        {
            var nameWithExtension = $"{name}{M4aExtension}";

            if (string.IsNullOrWhiteSpace(nameWithExtension) || existingItems.ContainsKey(nameWithExtension))
            {
                continue;
            }

            newItems.Add(nameWithExtension, url);
        }

        return newItems;
    }
}