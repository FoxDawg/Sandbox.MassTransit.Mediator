using System.Collections.Generic;

namespace WebApi.Abstractions;

public record ValidationErrors(IReadOnlyCollection<string> Errors)
{
    public ValidationErrors() : this(new string[] { })
    {
    }
}