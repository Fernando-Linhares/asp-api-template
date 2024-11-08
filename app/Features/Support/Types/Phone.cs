using System.Text.RegularExpressions;
using Api.App.Features.Support.Types.Excpetions;

namespace Api.App.Features.Support.Types;

public class Phone
{
    private readonly string _value;

    public Phone(string value)
    {
        var regex = new Regex(GetRegexPattern());

        if (!regex.IsMatch(value))
        {
            throw new TypeError("Phone is not a valid format. Accept format: Accept format: 99 9999-9999");
        }
        
        _value = value;
    }
    
    private string GetRegexPattern() 
        => @"[0-9]{2} [0-9]{5}\-[0-9]{4}|[0-9]{2} [0-9]{4}\-[0-9]{4}|[0-9]{2} [0-9] [0-9]{4}\-[0-9]{4}";
    
    public override string ToString() => _value;
}