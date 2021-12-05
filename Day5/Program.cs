using System.Collections.Immutable;

using MoreLinq;

var lines =
    ReadInput()
    .Select(line =>
    {
        var s = line
            .Split(" -> ")
            .SelectMany(x => x.Split(','))
            .Select(int.Parse)
            .ToImmutableArray();
        return new Line(new Point(s[0], s[1]), new Point(s[2], s[3]));
    });

var grid = new int[1000, 1000];

void ApplyLines(bool straight)
    => lines!
        .Where(x => straight ^ x.IsStraightLine)
        .SelectMany(l => l.EnumerateGridPoints())
        .ForEach(p => grid![p.X, p.Y]++);

IEnumerable<int> EnumerateGrid()
{
    for (var i = 0; i < 1000; i++)
    {
        for (var j = 0; j < 1000; j++)
        {
            yield return grid[i, j];
        }
    }
}

ApplyLines(straight: true);
WriteOutput(1, EnumerateGrid().Count(x => x > 1));

ApplyLines(straight: false);
WriteOutput(2, EnumerateGrid().Count(x => x > 1));

record Point(int X, int Y);
record Line(Point StartPoint, Point EndPoint)
{
    public bool IsStraightLine => StartPoint.X == EndPoint.X || StartPoint.Y == EndPoint.Y;

    public IEnumerable<Point> EnumerateGridPoints()
    {
        // I really cannot think of a name now
        IEnumerable<int> GenerateSomething(int start, int end)
        {
            if (start == end)
            {
                while (true)
                {
                    yield return start;
                }
            }
            else if (start > end)
            {
                for (var i = start; i >= end; i--)
                {
                    yield return i;
                }
            }
            else if (start < end)
            {
                for (var i = start; i <= end; i++)
                {
                    yield return i;
                }
            }
            else
            {
                throw new Exception();
            }
        }

        return 
            GenerateSomething(StartPoint.X, EndPoint.X)
            .Zip(
                GenerateSomething(StartPoint.Y, EndPoint.Y),
                (x, y) => new Point(x, y));
    }
}
