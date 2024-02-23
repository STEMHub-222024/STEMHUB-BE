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
    public class BannerController : BaseController
    {
        public BannerController(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBanner()
        {
            var banner = await _unitOfWork.BannerRepository.GetAllAsync<BannerDto>();
            return Ok(banner);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBanner(Guid id)
        {
            var banner =await _unitOfWork.BannerRepository.GetByIdAsync<BannerDto>(id);

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
                if (bannerModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

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
        public async Task<IActionResult> UpdateBanner(Guid id, BannerDto updatedBannerModel)
        {
            try
            {
                var existingBannerEntity = await _unitOfWork.BannerRepository.GetByIdAsync<Banner>(id);

                if (existingBannerEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingBannerEntity.Title = updatedBannerModel.Title;
                existingBannerEntity.Content = updatedBannerModel.Content;
                existingBannerEntity.Image = updatedBannerModel.Image;

                await _unitOfWork.BannerRepository.UpdateAsync(existingBannerEntity);

                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Cập nhật thành công" });
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
