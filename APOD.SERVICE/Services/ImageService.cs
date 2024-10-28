using APOD.SERVICE.Core;
using APOD.SERVICE.Core.Models;

namespace APOD.SERVICE.Services;

public sealed class ImageService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ImageService> _logger;

    private readonly string _nasaApodUrl;

    public ImageService(HttpClient httpClient, ILogger<ImageService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        var apiKey = ConfigurationReader.ReadSetting("ApiKey");
        _nasaApodUrl = $"https://api.nasa.gov/planetary/apod?api_key={apiKey}";
    }

    public async Task<string?>? DownloadNasaImageAsync(string downloadFolder)
    {
        var response = await _httpClient.GetAsync(_nasaApodUrl);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Failed to fetch APOD data: {response.StatusCode}");
            return null;
        }

        var apodData = await response.Content.ReadFromJsonAsync<NasaApodData>();

        if (apodData?.MediaType != "image")
        {
            _logger.LogWarning("APOD content is not an image.");
            return null;
        }
        var imageUrl = !string.IsNullOrEmpty(apodData.HdUrl) ? apodData.HdUrl : apodData.Url;

        var imageResponse = await _httpClient.GetAsync(apodData.Url);
        var imagePath = $"{downloadFolder}\\{apodData.Date}.jpg";

        await using (var fs = new FileStream(imagePath, FileMode.Create))
        {
            await imageResponse.Content.CopyToAsync(fs);
        }

        _logger.LogInformation($"Downloaded image to {imagePath}");

        return imagePath;
    }
}
