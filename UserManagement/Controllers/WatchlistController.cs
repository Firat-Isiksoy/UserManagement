using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserManagement.DTOs;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            _watchlistService = watchlistService;
        }
        private Guid GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userIdString!);
        }
        [HttpPost]
        public IActionResult AddToWatchlist([FromBody] WatchlistCreateDto request)
        {
            var userId = GetCurrentUserId();
            var response = _watchlistService.AddToWatchlist(userId, request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpPatch("watched")]
        public IActionResult MarkAsWatched([FromBody] WatchlistCreateDto request)
        {
            var userId = GetCurrentUserId();
            var response = _watchlistService.MarkAsWatched(userId, request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet]
        public IActionResult GetWatchlist([FromQuery] bool? watched = null)
        {
            var userId = GetCurrentUserId();
            var response = _watchlistService.GetWatchlist(userId, watched);
            return Ok(response);
        }
        [HttpDelete("{movieId}")]
        public IActionResult RemoveFromWatchlist(Guid movieId)
        {
            var userId = GetCurrentUserId();
            var isRemoved = _watchlistService.RemoveFromWatchlist(userId, movieId);
            if (!isRemoved) return NotFound(new { Message = "Bu film izleme listenizde bulunamadı." });
            return Ok(new { Message = "Film izleme listenizden kaldırıldı." });
        }
    }
}