namespace VideoLibraryExample.Commons;

public interface IFileWrapper
{
    public string ReadAllText(string path);
    public void WriteAllBytes(string path, byte[] bytes);
}