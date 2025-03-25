using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace NZWalks.API.CustomActionFilters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        // Có tác dụng validate các model trước khi vào controller 
        // Thêm [ValidateModel] vào trước action method trong controller
        // Thay thế cho điều kiện if (ModelState.IsValid) {} trong controller
        // Method OnActionExecuting sẽ được gọi trước khi action method của controller được thực thi.

        // Khi áp dụng [ValidateModel] lên một action hoặc controller, nó sẽ tự động kiểm tra tính
        // hợp lệ của model trước khi action được gọi
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestResult();
            }
        }

        // ActionExecutingContext chứa thông tin về request và action đang được thực thi trong
        // pipeline của ASP.NET Core.
        // Lớp này thường được sử dụng trong các Action Filters để can thiệp vào quá trình xử lý
        // action trước khi nó thực sự được thực thi.
    }
}
