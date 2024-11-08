using System.Text.RegularExpressions;
using Api.App.Features.Support.Types.Excpetions;

namespace Api.Features.Support.Types;

public class Email
{
    private readonly string _value;

    public Email(string value)
    {
        var regex = new Regex(@".*[@].*[.].*");

        if (!regex.IsMatch(value))
        {
            throw new TypeError("Email is not a valid email address. Accept format: <address_name>@<domain>.<com>");
        }
        
        _value = value;
    }
    
    public override string ToString() => _value;
}