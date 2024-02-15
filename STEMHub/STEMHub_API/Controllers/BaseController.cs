using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Service;

namespace STEMHub.STEMHub_API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly UnitOfWork _unitOfWork;

        public BaseController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
