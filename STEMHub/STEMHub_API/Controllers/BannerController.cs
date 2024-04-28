using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Constants;
using STEMHub.STEMHub_Services;
using Microsoft.IdentityModel.Tokens;
using STEMHub.STEMHub_Data.DTO;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : BaseController
    {
        public BannerController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBanner()
        {
            var banner = await _unitOfWork.BannerRepository.GetAllAsync<BannerDto>();
            if (!banner.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "Danh sách Banner hiện đang đang trống", IsSuccess = false});
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (banner == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response
                        { Status = "Thất bại", Message = "Danh sách Banner không tồn tại", IsSuccess = false });
            }

            return Ok(banner);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBanner(Guid id)
        {
            var banner = await _unitOfWork.BannerRepository.GetByIdAsync<BannerDto>(id);
            if (banner == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(banner);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBanner(BannerDto? bannerModel)
        {
            try
            {
                if (bannerModel != null)
                {
                    // IsNullOrEmpty kiểm tra null or rỗng
                    if (string.IsNullOrEmpty(bannerModel.Image))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Hình ảnh không thể bỏ trống!"});
                    }

                    if (string.IsNullOrEmpty(bannerModel.Title))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Tiêu đề không được bỏ trống!" });
                    }

                    if(string.IsNullOrEmpty(bannerModel.Content)) 
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Nội dung không được bỏ trống!" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });
                }

                var bannerEntity = _unitOfWork.Mapper.Map<Banner>(bannerModel);
                if (bannerEntity != null)
                {
                    await _unitOfWork.BannerRepository.AddAsync(bannerEntity);
                    await _unitOfWork.CommitAsync();

                    var bannerDto = _unitOfWork.Mapper.Map<BannerDto>(bannerEntity);

                    return CreatedAtAction(nameof(GetBanner), new { id = bannerDto!.BannerId }, bannerDto);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo banner!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBanner(Guid id, BannerDto? updatedBannerModel)
        {
            try
            {
                var existingBannerEntity = await _unitOfWork.BannerRepository.GetByIdAsync<Banner>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingBannerEntity != null && updatedBannerModel != null)
                {
                    if (!string.IsNullOrEmpty(updatedBannerModel.Title))
                    {
                        existingBannerEntity.Title = updatedBannerModel.Title;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Tiêu đề không thể bỏ trống!" });
                    }

                    if (!string.IsNullOrEmpty(updatedBannerModel.Content))
                    {
                        existingBannerEntity.Content = updatedBannerModel.Content;
                    }
                    else 
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Nội dung không được bỏ trống!" });
                    }

                    if (!string.IsNullOrEmpty(updatedBannerModel.Image))
                    {
                        existingBannerEntity.Image = updatedBannerModel.Image;

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Response { Status = "Thất bại", Message = "Hình ảnh không được bỏ trống!" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });
                }

                await _unitOfWork.BannerRepository.UpdateAsync(existingBannerEntity);

                await _unitOfWork.CommitAsync();

                return Ok(existingBannerEntity);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error:");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(Guid id)
        {
            var bannerEntity = await _unitOfWork.BannerRepository.GetByIdAsync<BannerDto>(id);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (bannerEntity == null)
                return NotFound();

            await _unitOfWork.BannerRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBanners([FromQuery] string bannerKey)
        {

            var banners = await _unitOfWork.BannerRepository.SearchAsync<BannerDto>(banner =>
                banner.Title != null &&
                banner.Title.Contains(bannerKey));
            if (!banners.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có banner chứa từ khoá {bannerKey}" });
            }

            return Ok(banners);
        }
    }
}
