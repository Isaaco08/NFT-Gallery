using System.Text.Json;

namespace ProyectoNFTs.Util;
public class GenericJsonFileHander<T> where T : class
{
    public static void SaveObjetToFile(Object obj, string file)
    {
        var jsonString = JsonSerializer.Serialize<T>((T)obj);
        File.WriteAllText($"Data//{file}", jsonString);
    }

    public static T GetObjetFromFile(string file)
    {
        string jsonString = File.ReadAllText($"Data//{file}");
        var @object = JsonSerializer.Deserialize<T>(jsonString);
        return (T)@object!;
    }
}
