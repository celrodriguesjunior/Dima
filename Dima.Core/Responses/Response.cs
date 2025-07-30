using System.Text.Json.Serialization;

namespace Dima.Core.Responses;

public class Response<TData>
{
    private readonly int _code;

    [JsonConstructor]
    public Response() => _code = Configuration.DefaultStatusCode;

    public Response(TData? data, int code = 200, string? message = null)
    {
        _code = code;
        Data = data;
        Message = message;
    }

    public TData? Data { get; set; }
    public string Message { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsSuccess => _code >= 200 && _code < 300;

}
