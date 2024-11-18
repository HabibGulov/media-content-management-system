public interface IFileService
{
    Task<string> CreateFile(IFormFile file,string folder);
    bool DeleteFile(string file,string folder);
}