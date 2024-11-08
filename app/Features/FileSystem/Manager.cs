namespace Api.App.Features.FileSystem;

public class Manager(string folder)
{
    private readonly string _storagePath = Path.Combine(
        Directory.GetCurrentDirectory(),
        "storage"
    );

    public bool Exists(string name)
    {
        return File.Exists(AbsolutePath(name));
    }

    public string Url(string name)
    {
        return ConfigApp.Get("app.url") + $"/public/{folder}/{name}";
    }

    public string AbsolutePath(string name)
    {
        return Path.Combine(_storagePath, folder, name);
    }

    public string GetContent(string name)
    {
        return File.ReadAllText(AbsolutePath(name));
    }

    public FileStream Get(string name)
    {
        return File.Open(AbsolutePath(name), FileMode.Open);
    }

    public void Save(FileStream file)
    {
        string nfilename = AbsolutePath(Path.GetFileName(file.Name));
        using var nfile = File.Create(nfilename);
        file.CopyTo(nfile);
        nfile.Close();
        file.Close();
    }

    public void Save(FileStream file, string name)
    {
        string nfilename = AbsolutePath(name);
        using var nfile = File.Create(nfilename);
        file.CopyTo(nfile);
        nfile.Close();
        file.Close();
    }

    public void Save(IFormFile file)
    {
        string nfilename = AbsolutePath(file.FileName);
        using var nfile = File.Create(nfilename);
        file.CopyTo(nfile);
        nfile.Close();
    }

    public void Save(IFormFile file, string name)
    {
        string nfilename = AbsolutePath(name);
        using var nfile = File.Create(nfilename);
        nfile.Close();
     
        file.CopyTo(nfile);
    }

    public void Remove(string name)
    {
        File.Delete(AbsolutePath(name));
    }

    public static void SaveFile(string folder, FileStream file)
    {
        var instance = new Manager(folder);
        instance.Save(file);
    }

    public static void SaveFile(string folder, IFormFile file)
    {
        var instance = new Manager(folder);
        instance.Save(file);
    }

    public static void SaveFile(string folder, string name, FileStream file)
    {
        var instance = new Manager(folder);
        instance.Save(file);
    }

    public static void SaveFile(string folder, string name, IFormFile file)
    {
        var instance = new Manager(folder);
        instance.Save(file);
    }

    public static string GetUrl(string folder, string name)
    {
        var instance = new Manager(folder);
        return instance.Url(name);
    }

    public static void Remove(string folder, string name)
    {
        var instance = new Manager(folder);
        instance.Remove(name);
    }

    public static string Get(string folder, string name)
    {
        var instance = new Manager(folder);
        return instance.GetContent(name);
    }

    public static bool Exists(string folder, string name)
    {
        var instance = new Manager(folder);
        return instance.Exists(name);
    }
}