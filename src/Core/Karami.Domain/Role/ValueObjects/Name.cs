using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Exceptions;

namespace Karami.Domain.Role.ValueObjects;

public class Name : ValueObject
{
    public readonly string Value;
    
    public Name() {}

    public Name(string value)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد نام الزامی می باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}