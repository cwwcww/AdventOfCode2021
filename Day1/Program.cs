using MoreLinq;

WriteOutput(
    1,
    ReadInput()
    .Select(int.Parse)
    .Pairwise((prev, next) => next > prev)
    .Count(x => x));

WriteOutput(
    2,
    ReadInput()
    .Select(int.Parse)
    .WindowLeft(3)
    .Select(x => x.Sum())
    .Pairwise((prev, next) => next > prev)
    .Count(x => x));
