namespace APOD.SERVICE.Core.Models;

internal sealed class NasaApodData
{
    public required string Url { get; set; }
    public string? HdUrl { get; set; }
    public required string MediaType { get; set; }
    public required string Date { get; set; }
}
