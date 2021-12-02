var position =
    ReadInput()
    .Select(x =>
    {
        var s = x.Split(' ');
        return (Direction: s[0], Distance: int.Parse(s[1]));
    })
    .Aggregate(
        (Depth: 0, HorizontalPosition: 0),
        (acc, x) => x.Direction switch
        {
            "forward" => acc with { HorizontalPosition = acc.HorizontalPosition + x.Distance },
            "up" => acc with { Depth = acc.Depth - x.Distance },
            "down" => acc with { Depth = acc.Depth + x.Distance },
            _ => throw new Exception()
        });

WriteOutput(1, position.HorizontalPosition * position.Depth);

var position2 =
    ReadInput()
    .Select(x =>
    {
        var s = x.Split(' ');
        return (MoveType: s[0], Value: int.Parse(s[1]));
    })
    .Aggregate(
        (Depth: 0, HorizontalPosition: 0, Aim: 0),
        (acc, x) => x.MoveType switch
        {
            "forward" => acc with 
            { 
                HorizontalPosition = acc.HorizontalPosition + x.Value,
                Depth = acc.Depth + (acc.Aim * x.Value)
            },
            "up" => acc with { Aim = acc.Aim - x.Value },
            "down" => acc with { Aim = acc.Aim + x.Value },
            _ => throw new Exception()
        });

WriteOutput(2, position2.HorizontalPosition * position2.Depth);
