using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkloadPlanner.Constants;

namespace WorkloadPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoardColorsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetColors()
        {
            return Ok(BoardColors.Colors);
        }
    }
}
