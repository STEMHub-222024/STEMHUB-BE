using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services;
using STEMHub.STEMHub_Services.Constants;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartsController : BaseController
    {
        public PartsController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<IActionResult> GetAllParts()
        {
            var parts = await _unitOfWork.PartsRepository.GetAllAsync<PartsDto>();
            if (!parts.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "Danh sách thành phần hiện đang đang trống", IsSuccess = false });
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (parts == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response
                        { Status = "Thất bại", Message = "Danh sách thành phần không tồn tại", IsSuccess = false });
            }

            return Ok(parts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParts(Guid id)
        {
            var parts = await _unitOfWork.PartsRepository.GetByIdAsync<PartsDto>(id);
            if (parts == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(parts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePart(PartsDto partsModel)
        {
            try
            {
                if (partsModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var existingPart = await _unitOfWork.PartsRepository.GetByIdAsync<Parts>(partsModel.LessonId);
                if (existingPart == null)
                {
                    return StatusCode(StatusCodes.Status409Conflict,
                        new Response { Status = "Thất bại", Message = "Bài học này đã có thành phần!" });
                }

                var partsEntity = _unitOfWork.Mapper.Map<Parts>(partsModel);
                if (partsEntity == null)
                {
                    await _unitOfWork.PartsRepository.AddAsync(partsEntity);
                    await _unitOfWork.CommitAsync();

                    var partsDto = _unitOfWork.Mapper.Map<PartsDto>(partsEntity);

                    return CreatedAtAction(nameof(GetAllParts), new { id = partsDto!.PartId }, partsDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể đăng thành phần!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(Guid id, PartsDto updatedPartsDto)
        {
            try
            {
                var existingPartsEntity = await _unitOfWork.PartsRepository.GetByIdAsync<Parts>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingPartsEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingPartsEntity.MaterialsHtmlContent = updatedPartsDto.MaterialsHtmlContent;
                existingPartsEntity.MaterialsMarkdown = updatedPartsDto.MaterialsMarkdown;
                existingPartsEntity.StepsMarkdown = updatedPartsDto.StepsMarkdown;
                existingPartsEntity.StepsHtmlContent = updatedPartsDto.StepsHtmlContent;
                existingPartsEntity.ResultsMarkdown = updatedPartsDto.ResultsMarkdown;
                existingPartsEntity.ResultsHtmlContent = updatedPartsDto.ResultsHtmlContent;

                await _unitOfWork.PartsRepository.UpdateAsync(existingPartsEntity);

                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParts(Guid id)
        {
            var partsEntity = await _unitOfWork.PartsRepository.GetByIdAsync<PartsDto>(id);

            if (partsEntity == null)
                return NotFound();

            await _unitOfWork.PartsRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
