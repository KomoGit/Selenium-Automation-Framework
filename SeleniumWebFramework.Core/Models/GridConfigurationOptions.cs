namespace SeleniumWebFramework.Core.Models;

public class GridConfigurationOptions
{
    public string ExecutionMode { get; set; } = "local";
    public string GridUrl { get; set; }
    public string Browser { get; set; } = "chrome";
}