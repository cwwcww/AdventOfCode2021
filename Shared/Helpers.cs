namespace Shared;

public static class Helpers
{
    public static IEnumerable<string> ReadInput(string filename = "input.txt")
    {
        using var stream = File.OpenRead(filename);
        using var reader = new StreamReader(stream);
        while (true)
        {
            var line = reader.ReadLine();

            if (line is null)
            {
                yield break;
            }

            yield return line;
        }
    }

    public static void WriteOutput(int part, object output)
    {
        var str = output.ToString();
        Console.WriteLine($"Part {part} output: {str}");
        File.WriteAllText($"part{part}-output.txt", str);
    }
}
