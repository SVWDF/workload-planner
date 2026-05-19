using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkloadPlanner.DTOs.ScrumBoards;
using WorkloadPlanner.Exceptions.Auth;
using WorkloadPlanner.Exceptions.Scrumboard;
using WorkloadPlanner.Models;
using WorkloadPlanner.Services.ScrumBoards;

namespace WorkloadPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScrumBoardsController : ControllerBase
    {
        private readonly IScrumBoardService _scrumboardService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScrumBoardsController(IScrumBoardService scrumboardService, UserManager<ApplicationUser> userManager)
        {
            _scrumboardService = scrumboardService;
            _userManager = userManager;
        }

        private async Task<ApplicationUser> GetUserAsync()
        {
            return await _userManager.GetUserAsync(User) ?? throw new UserNotFoundException();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScrumBoardListDTO>>> GetBoards()
        {
            try
            {
                var user = await GetUserAsync();
                var boards = await _scrumboardService.GetBoardsForUserAsync(user!.Id);
                return Ok(boards);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScrumBoardDetailDTO>> GetBoard(int id)
        {
            try
            {
                var user = await GetUserAsync();
                var board = await _scrumboardService.GetBoardByIdAsync(id, user!.Id);
                return Ok(board);    
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (ScrumboardNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ScrumBoardDetailDTO>> CreateBoard([FromBody] CreateScrumBoardDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { errors = validationErrors });
            }

            try
            {
                var user = await GetUserAsync();
                var board = await _scrumboardService.CreateBoardAsync(dto, user!.Id);
                return Ok(board);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }
    }
}
