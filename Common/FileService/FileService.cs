public class FileService(IWebHostEnvironment hostEnvironment) : IFileService
{
    private const long MaxFileSize = 10 * 1024 * 1024;

    private readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        { ".jpg", ".png", ".pdf", ".txt" };

    public async Task<string> CreateFile(IFormFile file, string folder)
    {
        if (_allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) == false) //test.dll = take .dll
            throw new InvalidOperationException("Invalid file type.");

        if (file.Length > MaxFileSize)
            throw new InvalidOperationException("File size exceeds the maximum allowed size.");

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; //io.jpg-> fjsdfs-fsfjsf-fsfj.jpg
        string folderPath = Path.Combine(hostEnvironment.WebRootPath, folder); //wwwroot  test-> wwwroot/test

        if (Directory.Exists(folderPath) == false)
        {
            Directory.CreateDirectory(folderPath);
        }

        string fullPath = Path.Combine(folderPath, fileName);

        try
        {
            await using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync(ex.Message);
            throw new InvalidOperationException("An error occurred while saving the file.");
        }
    }

    public bool DeleteFile(string file, string folder)
    {
        string folderPath = Path.Combine(hostEnvironment.WebRootPath, folder);
        string fullPath = Path.Combine(folderPath, file);

        try
        {
            if (Directory.Exists(folderPath) == false) return false;

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            throw new InvalidOperationException("An error occurred while delete the file.");
        }
    }
}