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
        public async Task<ActionResult<IEnumerable<ScrumBoardListDTO>>> GetScrumBoards()
        {
            try
            {
                ApplicationUser user = await GetUserAsync();
                IEnumerable<ScrumBoardListDTO> boards = await _scrumboardService.GetScrumBoardsForUserAsync(user.Id);
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
        public async Task<ActionResult<ScrumBoardDetailDTO>> GetScrumBoard(int id)
        {
            try
            {
                ApplicationUser user = await GetUserAsync();
                ScrumBoardDetailDTO board = await _scrumboardService.GetScrumBoardByIdAsync(id, user.Id);
                return Ok(board);    
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (ScrumBoardNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ScrumBoardCreatedDTO>> CreateScrumBoard([FromBody] CreateScrumBoardDTO dto)
        {
            if (!ModelState.IsValid)
            {
                List<string> validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { errors = validationErrors });
            }

            try
            {
                ApplicationUser user = await GetUserAsync();
                ScrumBoardCreatedDTO board = await _scrumboardService.CreateScrumBoardAsync(dto, user.Id);
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
