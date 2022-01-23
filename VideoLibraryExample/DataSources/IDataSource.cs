namespace VideoLibraryExample.DataSources;

public interface IDataSource
{
    IDictionary<string, string> Get();
}