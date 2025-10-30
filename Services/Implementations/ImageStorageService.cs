public class ImageStorageService : IImageStorageService
{
    private readonly string _storagePath;

    public ImageStorageService(IConfiguration configuration)
    {
        _storagePath = configuration["ImageStorage:Path"] ?? "wwwroot/uploads";
    }

    public async Task<string> SaveImageAsync(string base64Image, string folder)
    {
        try
        {
            // Remove data URI prefix if present
            var base64Data = base64Image.Contains(",")
                ? base64Image.Split(',')[1]
                : base64Image;

            var imageBytes = Convert.FromBase64String(base64Data);
            var fileName = $"{Guid.NewGuid()}.jpg";
            var folderPath = Path.Combine(_storagePath, folder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            await File.WriteAllBytesAsync(filePath, imageBytes);

            // Return relative URL
            return $"/uploads/{folder}/{fileName}";
        }
        catch (Exception ex)
        {
            // Log error
            throw new Exception("Failed to save image", ex);
        }
    }

    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        try
        {
            if (string.IsNullOrEmpty(imageUrl))
                return false;

            var filePath = Path.Combine(_storagePath, imageUrl.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
}