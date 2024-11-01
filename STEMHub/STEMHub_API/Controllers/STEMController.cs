using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Constants;
using STEMHub.STEMHub_Services;
using STEMHub.STEMHub_Data.DTO;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class STEMController : BaseController
    {
        public STEMController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAllSTEM()
        {
            var stem = await _unitOfWork.STEMRepository.GetAllAsync<STEMDto>();
            return Ok(stem);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSTEM(Guid id)
        {
            var stem = await _unitOfWork.STEMRepository.GetByIdAsync<STEMDto>(id);

            if (stem == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(stem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSTEM(STEMDto? stemModel)
        {
            try
            {
                if (stemModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var stemEntity = _unitOfWork.Mapper.Map<STEM>(stemModel);
                if (stemEntity != null)
                {
                    await _unitOfWork.STEMRepository.AddAsync(stemEntity);
                    await _unitOfWork.CommitAsync();

                    var stemDto = _unitOfWork.Mapper.Map<STEMDto>(stemEntity);

                    return CreatedAtAction(nameof(GetSTEM), new { id = stemDto!.STEMId }, stemDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo stem!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSTEM(Guid id, STEMDto updatedSTEMModel)
        {
            try
            {
                var existingSTEMEntity = await _unitOfWork.STEMRepository.GetByIdAsync<STEM>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingSTEMEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingSTEMEntity.STEMName = updatedSTEMModel.STEMName;

                await _unitOfWork.STEMRepository.UpdateAsync(existingSTEMEntity);
                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception e)
            {
                //if (_uniqueConstraintHandler.IsUniqueConstraintViolation(e))
                //{
                //    Log.Error(e, "Vi phạm trùng lặp!");
                //    return BadRequest(new { ErrorMessage = "Vi phạm trùng lặp!", ErrorCode = "DUPLICATE_KEY" });
                //}
                //else
                //{
                return StatusCode(500, "Internal Server Error");
                //}
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSTEM(Guid id)
        {
            var stemEntity = await _unitOfWork.STEMRepository.GetByIdAsync<STEMDto>(id);

            if (stemEntity == null)
                return NotFound();

            await _unitOfWork.STEMRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSTEMs([FromQuery] string stemKey)
        {
            var stems = await _unitOfWork.STEMRepository.SearchAsync<STEMDto>(stem =>
                stem.STEMName != null &&
                stem.STEMName.Contains(stemKey));
            if (stems == null || !stems.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có STEM chứa từ khoá {stemKey}" });
            }
            return Ok(stems);
        }
    }
}

