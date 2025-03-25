1. Model validations
	- Xác thực các model mà user gửi đến xem nội dung model đó có hợp lệ không (VD: Email, Password, ...)
	- Adding model validations to Endpoints 

	- Custom validate model attribute
		+ Tạo folder CustomActionFilters 
		+ Tạo class ValidateModelAttribute.cs 
		+ Thêm [ValidateModel] vào action trong controller
		--> Lúc này ở controller sẽ không phải kiểm tra điều kiện if(ModelState.IsValid) {} nữa