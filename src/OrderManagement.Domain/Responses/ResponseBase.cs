namespace OrderManagement.Domain.Responses;

public abstract class ResponseBase
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}