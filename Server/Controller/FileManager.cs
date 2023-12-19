public class FileManager
{
    private readonly IWebHostEnvironment _environment;

    public FileManager(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveUserPhotoAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return "/uploads/" + uniqueFileName;
    }
}
