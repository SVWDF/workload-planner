using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkloadPlanner.DTOs.Tickets;
using WorkloadPlanner.Exceptions.Auth;
using WorkloadPlanner.Exceptions.Ticket;
using WorkloadPlanner.Models;
using WorkloadPlanner.Services.Tickets;

namespace WorkloadPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketsController(ITicketService ticketService, UserManager<ApplicationUser> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
        }

        private async Task<ApplicationUser> GetUserAsync()
        {
            return await _userManager.GetUserAsync(User) ?? throw new UserNotFoundException();
        }

        [HttpGet("scrumboard/{scrumboardId}")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetScrumboardTickets(int scrumboardId)
        {
            try
            {
                ApplicationUser user = await GetUserAsync();
                IEnumerable<TicketDTO> tickets = await _ticketService.GetScrumboardTicketsAsync(scrumboardId, user.Id);
                return Ok(tickets);                
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TicketDTO>> CreateTicket([FromBody] CreateTicketDTO dto)
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
                TicketDTO ticket = await _ticketService.CreateTicketAsync(dto, user.Id);
                return Ok(ticket);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TicketDTO>> UpdateTicket(int id, [FromBody] UpdateTicketDTO dto)
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
                TicketDTO ticket = await _ticketService.UpdateTicketAsync(id, dto, user.Id);
                return Ok(ticket);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (TicketNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                ApplicationUser user = await GetUserAsync();
                await _ticketService.DeleteTicketAsync(id, user.Id);
                return NoContent();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (TicketNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpPatch("{id}/assign")]
        public async Task<ActionResult<TicketDTO>> AssignSelf(int id)
        {
            try
            {
                ApplicationUser user = await GetUserAsync();
                TicketDTO ticket = await _ticketService.AssignSelfToTicketAsync(id, user.Id);
                return Ok(ticket);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (TicketNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<TicketDTO>> UpdateStatus(int id, [FromBody] UpdateTicketStatusDTO dto)
        {
            try
            {
                ApplicationUser user = await GetUserAsync();
                TicketDTO ticket = await _ticketService.UpdateStatusAsync(id, dto, user.Id);
                return Ok(ticket);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (TicketNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new[] { ex.Message } });
            }
        }
    }
}
