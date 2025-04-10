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


























