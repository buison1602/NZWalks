using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using System.Text;
using Microsoft.OpenApi.Models;
using Azure.Core;
using static System.Net.WebRequestMethods;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.FileProviders;

// Mục đích của builder.Services:
//  - Khai báo các dịch vụ mà ứng dụng cần.
//  - Dịch vụ này sẽ được sử dụng trong Middleware hoặc Controller sau này.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Để sử dụng IHttpContextAccessor trong các lớp khác như repository, service, ...
builder.Services.AddHttpContextAccessor(); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Khai báo phiên bản tài liệu Swagger. Tài liệu này chứa:
    //      Danh sách các API
    //      Mô tả endpoints.
    //      Tên, version, ...
    // Tạo "version" cho tài liệu API trong Swagger. (v1, v2, ...)
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "NZ Walks API", Version = "v1" });


    // Khai báo rằng API dùng token kiểu Bearer trong header.
    // Bạn nói với Swagger rằng:
    //      - Sẽ có một token trong HTTP header(thường là Authorization: Bearer<token>).
    //      - Kiểu SecurityScheme là ApiKey, nhưng ở đây nó giả lập token để Swagger hiểu cách gửi.   
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        // Tên của header chứa token. Luôn là "Authorization" trong JWT.
        Name = "Authorization",
        // Vị trí gửi token — nằm trong phần Header của HTTP request.
        In = ParameterLocation.Header,
        // Đừng nhầm với API key thật.Đây là cách "giả lập" JWT để Swagger chấp nhận bạn nhập token
        Type = SecuritySchemeType.ApiKey,
        // Tên của schema — quan trọng khi Swagger hiển thị giao diện.
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    // --> Swagger không cần biết JWT là gì, nó chỉ hiểu là: “Tôi sẽ gửi một chuỗi vào header Authorization mỗi khi gọi API.”


    // Gắn việc gửi token này vào tất cả endpoint
    // Swagger sẽ tự động thêm token bạn nhập vào header mỗi lần gọi API
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        // cú pháp khởi tạo dictionary theo kiểu "inline initializer" 
        {
            // định nghĩa cách gửi token
            new OpenApiSecurityScheme
            {
                // Trỏ đến security schema "Bearer" bạn đã định nghĩa trước đó
                Reference = new OpenApiReference
                {
                    // Swagger biết bạn đang tham chiếu đến một schema bảo mật đã được định nghĩa.
                    Type = ReferenceType.SecurityScheme, 
                    // Phải trùng với Id trong AddSecurityDefinition(...), thường là "Bearer"
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme, // Tên của schema để Swagger hiển thị
                In = ParameterLocation.Header,
            },
            // Danh sách scopes (nếu bạn dùng OAuth2)
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<NZWalksDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepositopy>();


builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


// * Setting Up Identity 
// AddIdentityCore: chỉ đăng ký các dịch vụ cần thiết, không bao gồm các tính năng giao diện hoặc middleware mặc định
//
// IdentityUser: đại diện cho một người dùng trong hệ thống, chứa thuộc tính cơ bản như Id, Name, Email, ... 
//      - Gọi AddIdentityCore<IdentityUser>(): chỉ định răng hệ thống Identity sẽ sử dụng lớp IdentityUser làm kiểu dữ
//      liệu chính để quản lý thông tin người dùng. Có thể tùy chỉnh IdentityUser nếu muốn 
//
// AddRole: Thêm hỗ trợ Role với <IdentityRole> là lớp mặc định đại diện cho Role 
//
// AddTokenProvider: Thêm nhà cung cấp token để tùy chỉnh vào hệ thống Identity 
//      - DataProtectorTokenProvider<IdentityUser>: TokenProvider sử dụng DataProtector để mã hóa token
//      - "NZWalks": Tên của TokenProvider
//
// AddEntityFrameworkStores: Thêm EntityFrameworkStores để lưu trữ thông tin người dùng và Role vào Database 
//
// AddDefaultTokenProviders: Thêm các TokenProvider mặc định vào hệ thống Identity.
//      - Khác với AddTokenProvider vì nó cung cấp provider sẵn có mà không cần chỉ định tên cụ thể 
//
// AddIdentityCore trả ra 1 đối tượng IdentityBuilder và các phương thức như AddRoles, AddTokenProvider,
// AddEntityFrameworkStores, AddDefaultTokenProviders được gọi liên tiếp trên đối tượng này
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();


// Cấu hình cho IdentityOptions
// Trong đoạn mã này, ta cấu hình các yêu cầu mật khẩu:
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});



// * ĐĂNG KÝ Authentication
// Scheme là một cơ chế để xác định cách ứng dụng sẽ xác thực người dùng
// JwtBearerDefaults.AuthenticationScheme tương đương với chuỗi "Bearer", nghĩa là ứng dụng sẽ sử dụng Bearer Token để xác thực.
// AddJwtBearer() : Đăng ký JWT Bearer Authentication với các thông số cấu hình kiểm tra token.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Kiểm tra Issuer (người phát hành token)
            ValidateIssuer = true,
            // Kiểm tra Audience (người dùng token)
            ValidateAudience = true,
            // Kiểm tra thời gian hết hạn của token
            ValidateLifetime = true,
            // Kiểm tra khóa ký token
            ValidateIssuerSigningKey = true,
            // Giá trị Issuer hợp lệ
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            // Giá trị Audience hợp lệ
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // Khóa bí mật để xác thực chữ ký
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        });


//  Mục đích của app:
//      - Thêm Middleware vào pipeline xử lý request.
//      - Middleware chạy theo thứ tự khai báo.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Kích hoạt Middleware Authentication sau khi đăng ký Authentication
app.UseAuthentication();

app.UseAuthorization();


// Dòng code này nhằm cấu hình ASP.NET Core để phục vụ (serve) các file tĩnh từ thư mục Images, nằm trong gốc của dự án.
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
    // https://Localhost:1234/Images

});


app.MapControllers();

app.Run();
