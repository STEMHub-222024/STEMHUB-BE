using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Constants;
using STEMHub.STEMHub_Services;
using STEMHub.STEMHub_Data.DTO;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : BaseController
    {
        public IngredientsController(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _unitOfWork.IngredientsRepository.GetAllAsync<IngredientsDto>();
            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngredients(Guid id)
        {
            var ingredients = await _unitOfWork.IngredientsRepository.GetByIdAsync<IngredientsDto>(id);

            if (ingredients == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(ingredients);
        }

        [HttpPost]
        public async Task<IActionResult> CreateIngredients(IngredientsDto? ingredientsModel)
        {
            try
            {
                if (ingredientsModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Thất bại", Message = "Gửi request thất bại!" });

                var ingredientsEntity = _unitOfWork.Mapper.Map<Ingredients>(ingredientsModel);
                if (ingredientsEntity != null)
                {
                    await _unitOfWork.IngredientsRepository.AddAsync(ingredientsEntity);
                    await _unitOfWork.CommitAsync();

                    var ingredientsDto = _unitOfWork.Mapper.Map<IngredientsDto>(ingredientsEntity);

                    return CreatedAtAction(nameof(GetIngredients), new { id = ingredientsDto!.IngredientsId }, ingredientsDto);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Thất bại", Message = "Không thể tạo ingredients!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredients(Guid id, IngredientsDto updatedIngredientsModel)
        {
            try
            {
                var existingIngredientsEntity = await _unitOfWork.IngredientsRepository.GetByIdAsync<Ingredients>(id);

                if (existingIngredientsEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingIngredientsEntity.IngredientsName = updatedIngredientsModel.IngredientsName;
                existingIngredientsEntity.TopicId = updatedIngredientsModel.TopicId;

                await _unitOfWork.IngredientsRepository.UpdateAsync(existingIngredientsEntity);

                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error:");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredients(Guid id)
        {
            var ingredientsEntity = await _unitOfWork.IngredientsRepository.GetByIdAsync<IngredientsDto>(id);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (ingredientsEntity == null)
                return NotFound();

            await _unitOfWork.IngredientsRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchIngredientss([FromQuery] string ingredientsKey)
        {

            var ingredientss = await _unitOfWork.IngredientsRepository.SearchAsync<IngredientsDto>(ingredients =>
                ingredients.IngredientsName != null &&
                ingredients.IngredientsName.Contains(ingredientsKey));
            if (!ingredientss.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = $"Không có nguyên liệu nào chứa từ khoá {ingredientsKey}" });
            }

            return Ok(ingredientss);
        }
    }
}

