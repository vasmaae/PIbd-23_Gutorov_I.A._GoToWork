using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoToWorkContracts.Infrastructure;

public class OperationResponse
{
    protected HttpStatusCode StatusCode { get; set; }
    protected object? Result { get; set; }
    protected string? FileName { get; set; }

    public IActionResult GetResponse(HttpRequest request, HttpResponse response)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(response);

        response.StatusCode = (int)StatusCode;

        return Result switch
        {
            null => new StatusCodeResult((int)StatusCode),
            Stream stream => new FileStreamResult(stream, "application/octet-stream") { FileDownloadName = FileName },
            _ => new ObjectResult(Result)
        };
    }

    protected static TResult OK<TResult, TData>(TData data) where TResult : OperationResponse, new() =>
        new() { StatusCode = HttpStatusCode.OK, Result = data };

    protected static TResult OK<TResult, TData>(TData data, string filename) where TResult : OperationResponse, new() =>
        new() { StatusCode = HttpStatusCode.OK, Result = data, FileName = filename };

    protected static TResult NoContent<TResult>() where TResult : OperationResponse, new() =>
        new() { StatusCode = HttpStatusCode.NoContent };

    protected static TResult BadRequest<TResult>(string? errorMessage = null)
        where TResult : OperationResponse, new() =>
        new() { StatusCode = HttpStatusCode.BadRequest, Result = errorMessage };

    protected static TResult NotFound<TResult>(string? errorMessage = null)
        where TResult : OperationResponse, new() =>
        new() { StatusCode = HttpStatusCode.NotFound, Result = errorMessage };

    protected static TResult InternalServerError<TResult>(string? errorMessage = null)
        where TResult : OperationResponse, new() =>
        new() { StatusCode = HttpStatusCode.InternalServerError, Result = errorMessage };

    protected static TResult Unauthorized<TResult>(string? errorMessage = null)
        where TResult : OperationResponse, new() =>
        new() { StatusCode = HttpStatusCode.Unauthorized, Result = errorMessage };
}