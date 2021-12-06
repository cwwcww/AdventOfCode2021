using MoreLinq;


long Solve(int days)
{
    var population = new FishPopulation(ReadInput().Single().Split(',').Select(long.Parse));
    population.Step(days);
    return population.FishCount;
}

WriteOutput(1, Solve(80));
WriteOutput(2, Solve(256));

class FishPopulation
{
    private const int _phaseCount = 7;
    private readonly long[] _fishGroups = new long[_phaseCount];
    private readonly long[] _newFish = new long[_phaseCount];

    public FishPopulation(IEnumerable<long> fishTimers)
    {
        fishTimers.ForEach(x => _fishGroups[x]++);
    }

    public void Step(int days)
    {
        foreach (var birthingGroup in Enumerable.Range(0, _phaseCount).Repeat().Take(days))
        {
            var targetGroup = birthingGroup + 2;
            if (targetGroup >= _phaseCount)
            {
                targetGroup -= _phaseCount;
            }

            _fishGroups[targetGroup] += _newFish[targetGroup];
            _newFish[targetGroup] = _fishGroups[birthingGroup];
        }
    }

    public long FishCount => _fishGroups.Concat(_newFish).Sum();
}
