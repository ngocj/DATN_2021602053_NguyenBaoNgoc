using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.Infrastructure.UnitOfWork;
using System.Security.AccessControl;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Ir

    }
}
