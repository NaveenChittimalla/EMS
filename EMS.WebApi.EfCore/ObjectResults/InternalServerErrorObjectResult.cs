using EMS.WebApi.EfCore.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EMS.WebApi.EfCore.ObjectResults;

[DefaultStatusCode(DefaultStatusCode)]
public class InternalServerErrorObjectResult : ObjectResult
{
    private const int DefaultStatusCode = StatusCodes.Status500InternalServerError;

    /// <summary>
    /// Creates a new <see cref="InternalServerErrorObjectResult"/> instance.
    /// </summary>
    /// <param name="error">Contains the errors to be returned to the client.</param>
    public InternalServerErrorObjectResult([ActionResultObjectValue] IDictionary<string, string[]>? error)
        : base(error)
    {
        ArgumentNullException.ThrowIfNull(error);

        StatusCode = DefaultStatusCode;
        var validationProblemDetails = new ValidationProblemDetails(error);
        validationProblemDetails.Type = IETFRFC9110TypeStrings.InternalServerError;
        validationProblemDetails.Extensions.Add("traceId", Guid.NewGuid());
        Value = validationProblemDetails;
    }

    /// <summary>
    /// Creates a new <see cref="InternalServerErrorObjectResult"/> instance.
    /// </summary>
    /// <param name="modelState"><see cref="ModelStateDictionary"/> containing the validation errors.</param>
    public InternalServerErrorObjectResult([ActionResultObjectValue] ModelStateDictionary modelState)
        : base(new SerializableError(modelState))
    {
        ArgumentNullException.ThrowIfNull(modelState);

        StatusCode = DefaultStatusCode;
        var validationProblemDetails = new ValidationProblemDetails(modelState);
        validationProblemDetails.Type = IETFRFC9110TypeStrings.InternalServerError;
        validationProblemDetails.Extensions.Add("traceId", Guid.NewGuid());
        Value = validationProblemDetails;
    }
}
