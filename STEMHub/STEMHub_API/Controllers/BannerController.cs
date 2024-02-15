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
        public IActionResult GetAllBanner()
        {
            var banner = _unitOfWork.BannerRepository.GetAll<BannerDto>();
            return Ok(banner);
        }

        [HttpGet("{id}")]
        public IActionResult GetBanner(Guid id)
        {
            var banner = _unitOfWork.BannerRepository.GetById<BannerDto>(id);

            if (banner == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(banner);
        }

        [HttpPost]
        public IActionResult CreateBanner(BannerDto? bannerModel)
        {
            try
            {
                if (bannerModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var bannerEntity = _unitOfWork.Mapper.Map<Banner>(bannerModel);
                if (bannerEntity != null)
                {
                    _unitOfWork.BannerRepository.Add(bannerEntity);
                    _unitOfWork.Commits();

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
        public IActionResult UpdateBanner(Guid id, BannerDto updatedBannerModel)
        {
            try
            {
                var existingBannerEntity = _unitOfWork.BannerRepository.GetById<Banner>(id);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (existingBannerEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingBannerEntity.Title = updatedBannerModel.Title;
                existingBannerEntity.Content = updatedBannerModel.Content;
                existingBannerEntity.Image = updatedBannerModel.Image;

                _unitOfWork.BannerRepository.Update(existingBannerEntity);

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
        public IActionResult DeleteBanner(Guid id)
        {
            var bannerEntity = _unitOfWork.BannerRepository.GetById<BannerDto>(id);

            if (bannerEntity == null)
                return NotFound();

            _unitOfWork.BannerRepository.Delete(id);
            _unitOfWork.Commits();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public IActionResult SearchBanners([FromQuery] string bannerKey)
        {
            
            var banners = _unitOfWork.BannerRepository.Search<BannerDto>(banner =>
                banner.Title != null &&
                banner.Title.Contains(bannerKey));
            if (banners == null || !banners.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có banner chứa từ khoá {bannerKey}" });
            }

            return Ok(banners);
        }
    }
}
