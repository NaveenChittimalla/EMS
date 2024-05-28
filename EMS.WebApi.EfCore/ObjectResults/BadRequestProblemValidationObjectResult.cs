using EMS.WebApi.EfCore.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EMS.WebApi.EfCore.ObjectResults;

public class BadRequestProblemValidationObjectResult : BadRequestObjectResult
{

    /// <summary>
    /// Creates a new <see cref="BadRequestProblemValidationObjectResult"/> instance.
    /// </summary>
    /// <param name="error">Contains the errors to be returned to the client.</param>
    public BadRequestProblemValidationObjectResult([ActionResultObjectValue] IDictionary<string, string[]>? error)
        : base(error)
    {
        ArgumentNullException.ThrowIfNull(error);

        var validationProblemDetails = new ValidationProblemDetails(error);
        validationProblemDetails.Type = IETFRFC9110TypeStrings.BadRequest;
        validationProblemDetails.Extensions.Add("traceId", Guid.NewGuid());
        Value = validationProblemDetails;
    }

    /// <summary>
    /// Creates a new <see cref="BadRequestProblemValidationObjectResult"/> instance.
    /// </summary>
    /// <param name="modelState"><see cref="ModelStateDictionary"/> containing the validation errors.</param>
    public BadRequestProblemValidationObjectResult([ActionResultObjectValue] ModelStateDictionary modelState)
        : base(new SerializableError(modelState))
    {
        ArgumentNullException.ThrowIfNull(modelState);

        var validationProblemDetails = new ValidationProblemDetails(modelState);
        validationProblemDetails.Extensions.Add("traceId", Guid.NewGuid());
        validationProblemDetails.Type = IETFRFC9110TypeStrings.BadRequest;
        Value = validationProblemDetails;
    }
}
