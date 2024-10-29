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
    public class OwnerController : BaseController
    {
        public OwnerController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAllOwner()
        {
            var owner = await _unitOfWork.OwnerRepository.GetAllAsync<OwnerDto>();
            if (!owner.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "Owner hiện đang đang trống", IsSuccess = false });
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (owner == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response
                    { Status = "Thất bại", Message = "Owner không tồn tại", IsSuccess = false });
            }

            return Ok(owner);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOwner(Guid id)
        {
            var owner = await _unitOfWork.OwnerRepository.GetByIdAsync<OwnerDto>(id);
            if (owner == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(owner);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner(OwnerDto? ownerModel)
        {
            try
            {
                if (ownerModel != null)
                {
                    // IsNullOrEmpty kiểm tra null or rỗng
                    if (string.IsNullOrEmpty(ownerModel.Phone))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Số điện thoại không thể bỏ trống!" });
                    }

                    if (string.IsNullOrEmpty(ownerModel.Introduction))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Giới thiệu không được bỏ trống!" });
                    }

                    if (string.IsNullOrEmpty(ownerModel.Gmail))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Gmail không được bỏ trống!" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });
                }

                var ownerEntity = _unitOfWork.Mapper.Map<Owner>(ownerModel);
                if (ownerEntity != null)
                {
                    await _unitOfWork.OwnerRepository.AddAsync(ownerEntity);
                    await _unitOfWork.CommitAsync();

                    var ownerDto = _unitOfWork.Mapper.Map<OwnerDto>(ownerEntity);

                    return CreatedAtAction(nameof(GetOwner), new { id = ownerDto!.Id }, ownerDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo owner!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(Guid id, OwnerDto? updatedOwnerModel)
        {
            try
            {
                var existingOwnerEntity = await _unitOfWork.OwnerRepository.GetByIdAsync<Owner>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingOwnerEntity != null && updatedOwnerModel != null)
                {
                    if (!string.IsNullOrEmpty(updatedOwnerModel.Phone))
                    {
                        existingOwnerEntity.Phone = updatedOwnerModel.Phone;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Số điện thoại không thể bỏ trống!" });
                    }

                    if (!string.IsNullOrEmpty(updatedOwnerModel.Introduction))
                    {
                        existingOwnerEntity.Introduction = updatedOwnerModel.Introduction;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Giới thiệu không được bỏ trống!" });
                    }

                    if (!string.IsNullOrEmpty(updatedOwnerModel.Gmail))
                    {
                        existingOwnerEntity.Gmail = updatedOwnerModel.Gmail;

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Gmail không được bỏ trống!" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });
                }

                await _unitOfWork.OwnerRepository.UpdateAsync(existingOwnerEntity);

                await _unitOfWork.CommitAsync();

                return Ok(existingOwnerEntity);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error:");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            var ownerEntity = await _unitOfWork.OwnerRepository.GetByIdAsync<OwnerDto>(id);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (ownerEntity == null)
                return NotFound();

            await _unitOfWork.OwnerRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
