using System.Collections.Immutable;

using MoreLinq;

var fishes = ReadInput().Single().Split(',').Select(int.Parse).Select(x=>new Fish(x)).ToImmutableList();

for (var day = 0; day < 100; day++)
{
    foreach (var fish in fishes)
    {
        if (fish.DaysLeftToMultiply == 0)
        {
            fishes = fishes.Add(new Fish(8));
            fish.DaysLeftToMultiply = 6;
        }
        else
        {
            fish.DaysLeftToMultiply--;
        }
    }

    Console.WriteLine($"{fishes.Count} fish after {day+1} days");
}

class Fish 
{
    public Fish(int daysLeftToMultiply)
    {
        DaysLeftToMultiply = daysLeftToMultiply;
    }

    public int DaysLeftToMultiply { get; set; }
}
