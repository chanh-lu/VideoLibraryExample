namespace VideoLibraryExample.Commons;

public class FileWrapper : IFileWrapper
{
    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    public void WriteAllBytes(string path, byte[] bytes)
    {
        File.WriteAllBytes($"{path}", bytes);
    }
}