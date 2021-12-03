using System.Collections.Immutable;

var input = ReadInput().ToImmutableArray();

var agg =
    input
    .Aggregate(
        new (int OneCount, int ZeroCount)[12],
        (statistics, binaryDigitsStr) =>
            statistics
            .Zip(
                binaryDigitsStr,
                (stat, c) =>
                    c switch
                    {
                        '0' => stat with { ZeroCount = stat.ZeroCount + 1 },
                        '1' => stat with { OneCount = stat.OneCount + 1 },
                        _ => throw new Exception()
                    })
            .ToArray());

string ExtractNumber(bool mostCommon)
    => new(agg!
        .Select(x => !mostCommon ^ x.OneCount >= x.ZeroCount ? '1' : '0')
        .ToArray());

static int ParseNumber(string x) => Convert.ToInt32(x, 2);

var gamma = ExtractNumber(true);
var epsilon = ExtractNumber(false);
WriteOutput(1, ParseNumber(gamma) * ParseNumber(epsilon));

string ExtractNumber2(bool mostCommon)
{
    var numbers = input!;

    foreach (var index in Enumerable.Range(0, 12))
    {
        var extrema =
              MoreLinq.Extensions.MaxByExtension.MaxBy(
                   numbers
                   .Select(x => x[index])
                   .GroupBy(x => x)
                   .Select(x => (x.Key, Count: x.Count())),
                   x => x.Count)
              .ToImmutableArray();

        var val = !mostCommon ^ (extrema.Length > 1 || extrema.Single().Key == '1') ? '1' : '0';

        numbers = numbers.Where(x => x[index] == val).ToImmutableArray();

        if (numbers.Length == 1)
        {
            return numbers[0];
        }
    }

    throw new Exception();
}

var oxygen = ExtractNumber2(true);
var co2 = ExtractNumber2(false);
WriteOutput(2, ParseNumber(oxygen) * ParseNumber(co2));
