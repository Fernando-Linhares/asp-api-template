using System.Text.RegularExpressions;

namespace Api.App.Features.Dotenv;

public static class GetterEnv
{
    public static string Get (string key) => Environment.GetEnvironmentVariable(key) ?? string.Empty;
    public static void Set (string key, string value) => Environment.SetEnvironmentVariable(key, value);
    
    public static void Build()
    {
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        var fileContent = File.ReadAllText(envPath);

        foreach (var line in SplitFileContent(fileContent))
        {
            var values = line.Split("=");
            var key = values[0];
            
            if (key.StartsWith("\n") || key.StartsWith("#"))
            {
                continue;
            }
            
            var value = RemoveComments(values[1]);
            value = ResolveInterpolations(value) ?? value;
            
            if (Environment.GetEnvironmentVariable(key) == null)
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }

    private static string RemoveComments(string value)
    {
        if (value.Contains('#'))
        {
            int commentPosition = value.IndexOf('#');
            value = value.Substring(0, commentPosition);
        }

        return value.Trim();
    }

    private static string? ResolveInterpolations(string value)
    {
        string? valueToReplace = value;
        
        if (Regex.IsMatch(valueToReplace, @"\$\{(\w+)\}"))
        {
            var matchHolder = Regex.Match(valueToReplace, @"\$\{(\w+)\}");
            string placeholder = matchHolder.Value
                .Replace("${", "")
                .Replace("}", "");
            if (Environment.GetEnvironmentVariable(placeholder) != null)
            {
                valueToReplace = Environment.GetEnvironmentVariable(placeholder);
            }
        }

        return valueToReplace;
    }
    
    private static string[] SplitFileContent(string fileContent)
    {
        return fileContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }
}