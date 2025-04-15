1. Model validations
	- Xác thực các model mà user gửi đến xem nội dung model đó có hợp lệ không (VD: Email, Password, ...)
	- Adding model validations to Endpoints 

	- Custom validate model attribute
		+ Tạo folder CustomActionFilters 
		+ Tạo class ValidateModelAttribute.cs 
		+ Thêm [ValidateModel] vào action trong controller
		--> Lúc này ở controller sẽ không phải kiểm tra điều kiện if(ModelState.IsValid) {} nữa


2. Filtering, Sorting, Pagination 
	- Filtering: Lọc dữ liệu theo điều kiện
		+ Thêm các parameter vào action trong controller 
			--> Thêm [FromQuery] string? filterOn, [FromQuery] string? filterQuery
			--> Sửa lại method GetAllAsync: thêm logic lọc theo điều kiện filterOn và filterQuery
		+ VD: GET /api/users?name=John&age=25
			--> Lọc ra các user có tên là John tuổi bằng 25  
																				

	- Sorting: Sắp xếp dữ liệu

	- Pagination: Phân trang dữ liệu
		+ Thêm các parameter vào action trong controller 
			--> Thêm [FromQuery] int pageNumber, [FromQuery] int pageSize
			--> Sửa lại method GetAllAsync: thêm logic phân trang dữ liệu
		+ VD: GET /api/users?pageNumber=1&pageSize=10
			--> Lấy ra 10 user đầu tiên của trang 1


3. Authentication Flow (JWT)
	- Cấu hình appsettings.json:
		+ Key: gen ra từ Guid.NewGuid() 
		+ Issuer: 
			. Vào properties của dự án. 
			. Vào Debug/General/Open debug launch profiles UI
			. Tại mục https -> kéo xuống dưới tìm app url 
	
	- Thêm và cấu hình dịch vụ AddAuthentication trong program.cs 

	- Seed Role 
		PM> Add-Migration "Creating Auth Database"
		Build started...
		Build succeeded.
		More than one DbContext was found. Specify which one to use. Use the '-Context' parameter for PowerShell commands and the '--context' parameter for dotnet commands.
		PM> Add-Migration "Creating Auth Database" -Context "NZWalksAuthDbContext"

		+ Khi chạy Add-Migration "Creating Auth Database" do có nhiều DbContext được tạo nên ta phải chỉ định sử dụng DbContext nào 
		+ Sau đó update database ta cũng cần chỉ định rõ update DbContext nào 
			PM> Update-Database -Context "NZWalksAuthDbContext"

	- Setting Up Identity: Thiết lập danh tính 
	- Cấu hình cho IdentityOptions


	- Tạo Interface ITokenRepository và Class TokenRepository
		+ Có method CreateJWTToken


4. Image Upload 
	- Tạo domain model Image 
	
	- Mặc định, ASP.NET Core không thể phục vụ(serve) các static files như là image, html, css, ... 
		+ Thêm app.UseStaticFiles() vào program.cs


5. Add Logging to Console in ASP.NET 
	- Cài các Nuget Serilog, Serilog.AspNetCore, Serilog.Sinks.Console, Serilog.Sinks.File
	- Cấu hình cho Serilog trong program.cs

	- Thêm logging vào text file trong ASP.NET CORE

	- Viết middleware trong ExceptionMiddleware.cs
		+ Tạo folder Middleware
		+ Tạo class ExceptionMiddleware.cs
		+ Thêm vào middleware pipeline ở trong program.cs 
			--> app.UseMiddleware<ExceptionHandlerMiddleware>();
		

6. Versioning in ASP.NET CORE WEB API
	- Khi ứng dụng của bạn phát triển, bạn có thể:
		+ Thêm/bớt field trong response.
		+ Đổi logic xử lý.
		+ Thay đổi input/output format.

	➡ Nếu không versioning, client cũ có thể lỗi vì không tương thích.

	- Ví dụ:
		+ Client A dùng v1: GET /api/products → trả tên sản phẩm
		+ Client B dùng v2: GET /api/products → trả tên + giá + trạng thái

	- Các cách định nghĩa version trong Web API
		+ URL-based (phổ biến)		/api/v1/products
		+ Query string				/api/products?api-version=1.0
		+ Header-based				Gửi header: api-version: 1.0
		+ Media-type				Gửi header: Accept: application/json;v=1.0


	- Sửa đổi lại DTO, gồm V1(version 1) và V2
	- Cài đặt Nuget Microsoft.AspNetCore.Mvc.Versioning
	- Tại program.cs thêm 
		builder.Services.AddApiVersioning(options => 
		{
			options.AssumeDefaultVersionWhenUnspecified = true;
		})  

	- Sửa đổi lại các controller để sử dụng versioning
		+ Tại Controller
			* Thêm [ApiVersion("1.0")] --> Phiên bản chính
			* Thêm [ApiVersion("2.0")] --> Phiên bản mới 
		+ Tạo action V2
		+ Thêm [MapToApiVersion("1.0")] vào action

	- Lúc này đã có thể thực hiện request như sau: ( - VÍ DỤ - )
		+ https://localhost:7027/api/Walks?api-version=1.0
		+ https://localhost:7027/api/Walks?api-version=2.0

	- Nếu sửa lại Route ở trong controller thành [Route("api/v{version:apiVersion}/[controller]")] thì có thể request như sau:
		+ https://localhost:7027/api/v1/Walks
		+ https://localhost:7027/api/v2/Walks



	- Cài đặt Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
	- Tự động đổi thành 
		builder.Services.AddApiVersioning(options => 
		{
			options.AssumeDefaultVersionWhenUnspecified = true;
			options.DefaultApiVersion = new ApiVersion(1, 0);
			options.ApiVersion = true;
		})  

	- Thêm vào program.cs
		builder.Services.AddVersionedApiExplorer(options => 
		{
			options.GroupNameFormat = "'v'VVV";
			options.SubstituteApiVersionInUrl = true;
		}) 

		var versionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

	- Sửa
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI(options => 
			{
				foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
				{
					options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", 
						description.GroupName.ToUpperInvariant());
				}
			});
		}

	- Lúc này swagger đã hiện action V1 nhưng chưa hiện action V2. Tạo class ConfigurationSwaggerOption.cs
		+ Class đã được tạo ở bên Solution Explorer	:>>>>>>>>

	- Thêm builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
		
	--> Bây giờ chạy lại thì thấy có Select a definition ở trên swagger UI
		+ Chọn V2 thì sẽ hiện ra các action của V2
		+ Chọn V1 thì sẽ hiện ra các action của V1







