namespace APBD;

public class FileWorker
{
    private string _path;
    
    public FileWorker(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("File not found ", path);
        _path = path;
       
    }

    public IEnumerable<string> GetFileContents()
    {
         var contents = File.ReadLines(_path);
         return contents;
    }
}