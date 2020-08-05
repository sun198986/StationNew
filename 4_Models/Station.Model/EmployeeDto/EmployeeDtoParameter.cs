using Microsoft.AspNetCore.Mvc;
using Station.Helper.Extensions;

namespace Station.Model.EmployeeDto
{
    public class EmployeeDtoParameter:DtoParameter
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [ModelBinder(BinderType = typeof(DtoModelBinder<EmployeeSearchDto>))]
        public EmployeeSearchDto Search { get; set; }
    }
}