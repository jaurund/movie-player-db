using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : ControllerBase
    {
        // In-memory video list for testing
        private static List<Videos> _videos = new List<Videos>
        {
            new Videos
            {
                Id = 1,
                Title = "Test MKV Video",
                Description = "A local MKV test video.",
                VideoURL = "/videos/2025-08-07 21-22-27.mkv",
                NumberOfLikes = 0
            }
        };

        [HttpGet]
        public IActionResult GetVideos()
        {
            // Return list of videos from in-memory list
            return Ok(_videos);
        }

        [HttpPost("{id}/like")]
        public IActionResult LikeVideo(int id, [FromBody] VideoLikesDTO dto)
        {
            var video = _videos.FirstOrDefault(v => v.Id == id);
            if (video == null)
                return NotFound();

            video.NumberOfLikes++;
            return Ok(video);
        }

        // Add more endpoints as needed (upload, stream, etc.)
    }
}