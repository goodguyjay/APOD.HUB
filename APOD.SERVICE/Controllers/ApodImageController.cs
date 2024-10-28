using APOD.SERVICE.Core;
using APOD.SERVICE.Services;
using Microsoft.AspNetCore.Mvc;

namespace APOD.SERVICE.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ApodImageController : ControllerBase
{
    private readonly ImageService _imageService;
    private readonly ILogger<ApodImageController> _logger;

    public ApodImageController(ImageService imageService, ILogger<ApodImageController> logger)
    {
        _imageService = imageService;
        _logger = logger;
    }

    [HttpGet("download-image")]
    public async Task<IActionResult> DownloadApodImage()
    {
        try
        {
            var downloadFolder = ConfigurationReader.ReadSetting("DownloadFolder");

            var imagePath = await _imageService.DownloadNasaImageAsync(downloadFolder);

            if (string.IsNullOrEmpty(imagePath))
            {
                return BadRequest("The path is invalid. Please check the file config.ini");
            }

            return Ok($"Image downloaded to: {imagePath}");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error downloading APOD image.");
            return StatusCode(500, "Internal server error.");
        }
    }
}
