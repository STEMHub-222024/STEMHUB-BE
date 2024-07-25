using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Constants;
using STEMHub.STEMHub_Services;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScientistController : BaseController
    {
        public ScientistController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAllScientist()
        {
            var scientist = await _unitOfWork.ScientistRepository.GetAllAsync<ScientistDto>();
            if (!scientist.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "Danh sách Scientist hiện đang đang trống", IsSuccess = false });
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (scientist == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response
                    { Status = "Thất bại", Message = "Danh sách Scientist không tồn tại", IsSuccess = false });
            }

            return Ok(scientist);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScientist(Guid id)
        {
            var scientist = await _unitOfWork.ScientistRepository.GetByIdAsync<ScientistDto>(id);
            if (scientist == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(scientist);
        }

        [HttpPost]
        public async Task<IActionResult> CreateScientist(ScientistDto? scientistModel)
        {
            try
            {
                if (scientistModel != null)
                {
                    // IsNullOrEmpty kiểm tra null or rỗng
                    if (string.IsNullOrEmpty(scientistModel.Adage))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Châm ngôn không thể bỏ trống!" });
                    }

                    if (string.IsNullOrEmpty(scientistModel.FullName))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Tên không được bỏ trống!" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });
                }

                var scientistEntity = _unitOfWork.Mapper.Map<Scientist>(scientistModel);
                if (scientistEntity != null)
                {
                    await _unitOfWork.ScientistRepository.AddAsync(scientistEntity);
                    await _unitOfWork.CommitAsync();

                    var scientistDto = _unitOfWork.Mapper.Map<ScientistDto>(scientistEntity);

                    return CreatedAtAction(nameof(GetScientist), new { id = scientistDto!.ScientistId }, scientistDto);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo scientist!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScientist(Guid id, ScientistDto? updatedScientistModel)
        {
            try
            {
                var existingScientistEntity = await _unitOfWork.ScientistRepository.GetByIdAsync<Scientist>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingScientistEntity != null && updatedScientistModel != null)
                {
                    if (!string.IsNullOrEmpty(updatedScientistModel.FullName))
                    {
                        existingScientistEntity.FullName = updatedScientistModel.FullName;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Tên không thể bỏ trống!" });
                    }

                    if (!string.IsNullOrEmpty(updatedScientistModel.Adage))
                    {
                        existingScientistEntity.Adage = updatedScientistModel.Adage;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Châm ngôn không được bỏ trống!" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });
                }

                await _unitOfWork.ScientistRepository.UpdateAsync(existingScientistEntity);

                await _unitOfWork.CommitAsync();

                return Ok(existingScientistEntity);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error:");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScientist(Guid id)
        {
            var scientistEntity = await _unitOfWork.ScientistRepository.GetByIdAsync<ScientistDto>(id);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (scientistEntity == null)
                return NotFound();

            await _unitOfWork.ScientistRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchScientists([FromQuery] string scientistKey)
        {

            var scientists = await _unitOfWork.ScientistRepository.SearchAsync<ScientistDto>(scientist =>
                scientist.FullName != null &&
                scientist.FullName.Contains(scientistKey));
            if (!scientists.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có nhà Khoa học chứa từ khoá {scientistKey}" });
            }

            return Ok(scientists);
        }
    }
}
