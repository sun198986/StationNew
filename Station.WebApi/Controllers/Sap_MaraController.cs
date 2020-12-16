using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Station.Entity.DB2Admin;
using Station.Helper.Extensions;
using Station.Model.RegistDto;
using Station.Model.Sap_MaraDto;
using Station.Repository.Sap_Mara;
using Station.Repository.StaionRegist;
using Station.SortApply.Helper;

namespace Station.WebApi.Controllers
{
    [ApiController]
    [Route("api/sap_Mara")]
    public class Sap_MaraController : ControllerBase
    {
        private readonly ISap_MaraRepository _sap_MaraRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;

        public Sap_MaraController(ISap_MaraRepository sap_MaraRepository, IMapper mapper, IPropertyMappingService propertyMappingService)
        {
            _sap_MaraRepository = sap_MaraRepository ?? throw new ArgumentNullException(nameof(sap_MaraRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService;
        }

        ///// <summary>
        ///// 查询单个注册信息
        ///// </summary>
        ///// <param name="id">主键Id</param>
        ///// <param name="fields">塑形字段</param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //public async Task<ActionResult<RegistDto>> GetRegist(string id,
        //    [FromQuery] string fields)
        //{
        //    if (id == null)
        //        throw new ArgumentNullException(nameof(id));
        //    var entity = await _sap_MaraRepository.GetSingleAsync(id);
        //    var returnDto = _mapper.Map<RegistDto>(entity);
        //    return Ok(returnDto.ShapeData(fields));
        //}

        /// <summary>
        /// 查询注册信息
        /// </summary>
        /// <param name="registDtoP">查询条件 例: Name:孙,Age:1</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSapMaraCollection(
            [FromQuery]Sap_MaraDtoParameter registDtoP
        )
        {
            Expression<Func<Sap_Mara, bool>> expression = null;

            if (registDtoP.Search != null)
            {
                var entity = _mapper.Map<Sap_Mara>(registDtoP.Search);
                expression = entity.AsExpression();
            }

            Dictionary<string, PropertyMappingValue> mappingDictionary = null;

            if (registDtoP.OrderBy != null)
            {
                if (!_propertyMappingService.ValidMappingExistsFor<Sap_MaraDto, Sap_Mara>(registDtoP.OrderBy))
                {
                    return BadRequest("无法找到对应的属性");
                }

                //PropertyMapping <RegistDto, Regist> registMapping= new PropertyMapping<RegistDto, Regist>(
                //    new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
                //    {
                //        {"RegistId", new PropertyMappingValue(new List<string> {"RegistId"})},
                //        {"RegistDate", new PropertyMappingValue(new List<string> {"RegistDate"})},
                //        {"MaintainNumber", new PropertyMappingValue(new List<string> {"MaintainNumber"})},
                //        {"CustomName", new PropertyMappingValue(new List<string> {"CustomName"})},
                //        {"Address", new PropertyMappingValue(new List<string> {"Address"})},
                //        {"Linkman", new PropertyMappingValue(new List<string> {"Linkman"})},
                //        {"TelPhone", new PropertyMappingValue(new List<string> {"Phone"})},
                //        {"Fax", new PropertyMappingValue(new List<string> {"Fax"})}
                //    });
                //mappingDictionary = registMapping.MappingDictionary;
                mappingDictionary = _propertyMappingService.GetPropertyMapping<Sap_MaraDto, Sap_Mara>();
            }

            var entities = await _sap_MaraRepository.GetAsync(registDtoP.Ids, expression,registDtoP.OrderBy,mappingDictionary);
            if (registDtoP.Ids!=null && registDtoP.Ids.Count() != entities.Count())
            {
                List<string> idNotFounds = registDtoP.Ids.Where(x => !entities.Select(p => p.MATNR).ToList().Contains(x)).ToList();
                return NotFound(JsonSerializer.Serialize(idNotFounds));
            }

            var listDto = _mapper.Map<IEnumerable<Sap_MaraDto>>(entities);
            return Ok(listDto.ShapeData(registDtoP.Fields));
        }

        ///// <summary>
        ///// 创建注册信息
        ///// </summary>
        ///// <param name="regist">注册信息</param>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult CreateReigst(RegistAddDto regist)
        //{
        //    if (regist == null)
        //        throw new ArgumentNullException(nameof(regist));

        //    var entity = _mapper.Map<Regist>(regist);
        //    _sap_MaraRepository.AddRegist(entity);
        //    _sap_MaraRepository.SaveChanges();
        //    var returnDto = _mapper.Map<RegistDto>(entity);
        //    return CreatedAtRoute(nameof(GetRegistCollection), new { ids = returnDto.RegistId }, returnDto);
        //}

        ///// <summary>
        ///// 批量创建注册信息
        ///// </summary>
        ///// <param name="regists">注册信息集合</param>
        ///// <returns></returns>
        //[HttpPost("batch")]
        //public IActionResult CreateRegist(IEnumerable<RegistAddDto> regists)
        //{
        //    if (!regists.Any())
        //    {
        //        throw new ArgumentNullException(nameof(regists));
        //    }
        //    var entities = _mapper.Map<IList<Regist>>(regists);
        //    _sap_MaraRepository.Add(entities);
        //    _sap_MaraRepository.SaveChanges();
        //    var returnDtos = _mapper.Map<IEnumerable<RegistDto>>(entities);
        //    var idsString = string.Join(",", returnDtos.Select(x => x.RegistId));

        //    return CreatedAtRoute(nameof(GetRegistCollection), new { ids = idsString }, returnDtos);
        //}

        ///// <summary>
        ///// 删除单个注册信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete("id")]
        //public async Task<IActionResult> DeleteRegist(string id)
        //{
        //    if (id == null)
        //    {
        //        throw new ArgumentNullException(nameof(id));
        //    }
        //    var entity = await _sap_MaraRepository.GetSingleAsync(id);
        //    if (entity == null)
        //    {
        //        return NotFound(nameof(id) + id);
        //    }

        //    _sap_MaraRepository.Delete(entity);
        //    _sap_MaraRepository.SaveChanges();
        //    return NoContent();
        //}


        ///// <summary>
        ///// 根据id的集合删除注册信息
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //[HttpDelete("{ids}")]
        //public async Task<IActionResult> DeleteRegists(
        //    [FromRoute]
        //    [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        //    IEnumerable<string> ids)
        //{
        //    if (ids == null)
        //        return BadRequest();

        //    var entities = await _sap_MaraRepository.GetRegistsAsync(ids);
        //    if (ids.Count() != entities.Count())
        //    {
        //        List<string> idNotFounds = ids.Where(x => !entities.Select(p => p.RegistId).ToList().Contains(x)).ToList();
        //        return NotFound(JsonSerializer.Serialize(idNotFounds));
        //    }

        //    _sap_MaraRepository.DeleteRegist(entities);
        //    _sap_MaraRepository.SaveChanges();

        //    return NoContent();
        //}

        ///// <summary>
        ///// 修改注册信息
        ///// </summary>
        ///// <param name="registId">主键id</param>
        ///// <param name="regist">注册信息</param>
        ///// <returns></returns>
        //[HttpPut("{registId}")]
        //public async Task<IActionResult> UpdateRegist([FromRoute] string registId, [FromBody] RegistUpdateDto regist)
        //{
        //    if (registId == null)
        //        throw new ArgumentNullException(nameof(registId));
        //    if (regist == null)
        //        throw new ArgumentNullException(nameof(regist));
        //    var entity = await _sap_MaraRepository.GetRegistsAsync(registId);
        //    if (entity == null)
        //    {
        //        return NotFound($"id:{registId}没有查到数据");
        //    }

        //    _mapper.Map(regist, entity);
        //    _sap_MaraRepository.UpdateRegist(entity);
        //    _sap_MaraRepository.SaveChanges();
        //    return NoContent();
        //}

        //[HttpGet("includeEmployee/{registId}")]
        //public async Task<IActionResult> GetRegistIncludeEmplyee(string registId)
        //{
        //    if (registId == null)
        //        throw new ArgumentNullException(nameof(registId));
        //    var entity = await _sap_MaraRepository.GetSingleRegistAndEmployeeAsync(registId);
        //    var returnDto = _mapper.Map<RegistDto>(entity);
        //    return Ok(returnDto);
        //}
    }
}