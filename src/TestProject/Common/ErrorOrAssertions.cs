using ErrorOr;


namespace TestProject.Common;

/// <summary>
///  A set of assertions for ErrorOr results to be used in tests.
/// </summary>

public interface IErrorOrAssertions
{
    void AssertValidationError<T>(ErrorOr<T> result, string? expectedCode = null);
    void AssertNotFoundError<T>(ErrorOr<T> result, string? expectedCode = null);
    void AssertConflictError<T>(ErrorOr<T> result, string? expectedCode = null);
    void AssertUnauthorizedError<T>(ErrorOr<T> result, string? expectedCode = null);
    void AssertErrorOfType<T>(ErrorOr<T> result, ErrorType expectedType, string? expectedCode = null);
    void AssertSuccess<T>(ErrorOr<T> result);
    void AssertError<T>(ErrorOr<T> result);
    void AssertHasValue<T>(ErrorOr<T> result);
    void AssertNullValue<T>(ErrorOr<T> result);
    void AssertCollection<T>(ErrorOr<IEnumerable<T>> result, int expectedCount);
    void AssertNonEmptyCollection<T>(ErrorOr<IEnumerable<T>> result);
    void AssertEmptyCollection<T>(ErrorOr<IEnumerable<T>> result);
    void AssertValidGuid(Guid id);
    string FormatErrors(List<Error> errors);
}


public  class ErrorOrAssertions: IErrorOrAssertions
{

    public void AssertValidationError<T>(ErrorOr<T> result, string? expectedCode = null)
        => AssertErrorOfType(result, ErrorType.Validation, expectedCode);

    public void AssertNotFoundError<T>(ErrorOr<T> result, string? expectedCode = null)
        => AssertErrorOfType(result, ErrorType.NotFound, expectedCode);

    public void AssertConflictError<T>(ErrorOr<T> result, string? expectedCode = null)
        => AssertErrorOfType(result, ErrorType.Conflict, expectedCode);

    public void AssertUnauthorizedError<T>(ErrorOr<T> result, string? expectedCode = null)
        => AssertErrorOfType(result, ErrorType.Unauthorized, expectedCode);

    public void AssertErrorOfType<T>(ErrorOr<T> result, ErrorType expectedType, string? expectedCode = null)
    {
        AssertError(result);
        Assert.Contains(result.Errors, e => e.Type == expectedType);
        if (!string.IsNullOrEmpty(expectedCode))
            Assert.Contains(result.Errors, e => e.Code == expectedCode);
    }

    public void AssertSuccess<T>(ErrorOr<T> result)
    {
        Assert.False(result.IsError, $"Esperava sucesso, mas recebeu erros: {FormatErrors(result.Errors)}");
    }

    public void AssertError<T>(ErrorOr<T> result)
    {
        Assert.True(result.IsError, "Esperava erro, mas a operação foi bem-sucedida");
    }

    public void AssertHasValue<T>(ErrorOr<T> result)
    {
        AssertSuccess(result);
        Assert.NotNull(result.Value);
    }

    public void AssertNullValue<T>(ErrorOr<T> result)
    {
        AssertSuccess(result);
        Assert.Null(result.Value);
    }

    public void AssertCollection<T>(ErrorOr<IEnumerable<T>> result, int expectedCount)
    {
        AssertSuccess(result);
        Assert.Equal(expectedCount, result.Value.Count());
    }

    public void AssertNonEmptyCollection<T>(ErrorOr<IEnumerable<T>> result)
    {
        AssertSuccess(result);
        Assert.NotEmpty(result.Value);
    }

    public void AssertEmptyCollection<T>(ErrorOr<IEnumerable<T>> result)
    {
        AssertSuccess(result);
        Assert.Empty(result.Value);
    }

    public void AssertValidGuid(Guid id)
    {
        Assert.NotEqual(Guid.Empty, id);
    }

    public string FormatErrors(List<Error> errors)
    {
        return string.Join("; ", errors.Select(e => $"{e.Type}: {e.Code} - {e.Description}"));
    }
}