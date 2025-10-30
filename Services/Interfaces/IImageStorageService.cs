public interface IImageStorageService
{
    Task<string> SaveImageAsync(string base64Image, string folder);
    Task<bool> DeleteImageAsync(string imageUrl);
}