using System.Globalization;

namespace Api.App.Features.Support.Types;

public class Price(decimal value)
{
    private decimal _value = value;

    public string ToBrl()
    {
        CultureInfo brCultureInfo = new CultureInfo("pt-BR", false);
        return string.Format(brCultureInfo, "{0:c0}", _value);    }

    public string ToUsd()
    {
        CultureInfo enCultureInfo = new CultureInfo("en-SG", false);
        return string.Format(enCultureInfo, "{0:c0}", _value);    }

    public string ToEur()
    {
        CultureInfo euroCulture = new CultureInfo("de-DE", false);
        return string.Format(euroCulture, "{0:c0}", _value);
    }
    
    public override string ToString() => _value.ToString();
}