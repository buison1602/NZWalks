using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepositopy : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NZWalksDbContext _dbContext;

        public LocalImageRepositopy(IWebHostEnvironment webHostEnvironment, 
            IHttpContextAccessor httpContextAccessor,
            NZWalksDbContext dbContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._httpContextAccessor = httpContextAccessor;
            this._dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            // ContentRootPath: đường dẫn gốc của ứng dụng trên máy chủ
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", 
                $"{image.FileName}{image.FileExtension}");

            // Upload Image to Local Path
            // Mở một stream đến file đích. Ghi nội dung của file vào stream.
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // Tạo đường dẫn truy cập công khai (URL) https://localhost:1234/images/HAHA.jpg
            // Thêm builder.Services.AddHttpContextAccessor(); vào program.cs 
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;


            // Add Image to the Images table 
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;
        }
    }
}
