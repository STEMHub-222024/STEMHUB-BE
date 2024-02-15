using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Service.Constants;
using STEMHub.STEMHub_Service.DTO;
using STEMHub.STEMHub_Service;

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
        public IActionResult GetAllSTEM()
        {
            var stem = _unitOfWork.STEMRepository.GetAll<STEMDto>();
            return Ok(stem);
        }

        [HttpGet("{id}")]
        public IActionResult GetSTEM(Guid id)
        {
            var stem = _unitOfWork.STEMRepository.GetById<STEMDto>(id);

            if (stem == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(stem);
        }

        [HttpPost]
        public IActionResult CreateSTEM(STEMDto? stemModel)
        {
            try
            {
                if (stemModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var stemEntity = _unitOfWork.Mapper.Map<STEM>(stemModel);
                if (stemEntity != null)
                {
                    _unitOfWork.STEMRepository.Add(stemEntity);
                    _unitOfWork.Commits();

                    var stemDto = _unitOfWork.Mapper.Map<STEMDto>(stemEntity);

                    return CreatedAtAction(nameof(GetSTEM), new { id = stemDto!.STEMId }, stemDto);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo stem!" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSTEM(Guid id, STEMDto updatedSTEMModel)
        {
            try
            {
                var existingSTEMEntity = _unitOfWork.STEMRepository.GetById<STEM>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingSTEMEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingSTEMEntity.STEMName = updatedSTEMModel.STEMName;

                _unitOfWork.STEMRepository.Update(existingSTEMEntity);

                _unitOfWork.Commits();

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
        public IActionResult DeleteSTEM(Guid id)
        {
            var stemEntity = _unitOfWork.STEMRepository.GetById<STEMDto>(id);

            if (stemEntity == null)
                return NotFound();

            _unitOfWork.STEMRepository.Delete(id);
            _unitOfWork.Commits();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public IActionResult SearchSTEMs([FromQuery] string stemKey)
        {
            var stems = _unitOfWork.STEMRepository.Search<STEMDto>(stem =>
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

