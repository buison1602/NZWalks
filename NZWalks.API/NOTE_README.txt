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








