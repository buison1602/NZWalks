using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext: IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed data for Roles

            var readerRoleId = "716a11c7-c2dd-4f89-814f-c624ab3704dc";
            var writerRoleId = "ffedc9d1-2ed5-4452-9db5-6c38a7151812";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    // Id của Role
                    Id = readerRoleId,
                    // ConcurrencyStamp là một mã xác thực để đảm bảo rằng dữ liệu không bị xung đột
                    ConcurrencyStamp = readerRoleId,
                    // Tên Role 
                    Name = "Reader",
                    // Sử dụng trường này để tìm kiếm Role mà không phân biệt chữ hoa chữ thường
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            // Thêm Roles vào DB 
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
