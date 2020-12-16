using Microsoft.AspNetCore.Mvc;
using Station.Helper.Extensions;

namespace Station.Model.Sap_MaraDto
{
    public class Sap_MaraDtoParameter : DtoParameter
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [ModelBinder(BinderType = typeof(DtoModelBinder<Sap_MaraSearchDto>))]
        public Sap_MaraSearchDto Search { get; set; }
    }
}