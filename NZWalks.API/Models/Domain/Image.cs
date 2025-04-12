using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        [NotMapped] // Thuộc tính này không nên được ánh xạ (mapped) vào cột trong CSDL.
        // IFormFile là kiểu dữ liệu đại diện cho file được upload từ client (qua form HTML hoặc API).
        public IFormFile File { get; set; }

        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension { get; set; } // ví dụ jpg, png, ... 
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}
