using System.Collections.Immutable;

using MoreLinq;

var input = ReadInput().ToImmutableArray();
var numbers = input.First().Split(',').Select(int.Parse).ToImmutableArray();

var boards =
    MoreEnumerable
    .Generate(2, boardStart => boardStart += 6)
    .TakeWhile(boardStart => boardStart < input.Length)
    .Select(boardStart =>
        input
        .Slice(boardStart, 5)
        .Select(x =>
            x
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Select(x => new BoardCell(x))
            .ToArray())
        .ToArray())
    .Select(x => new Board(x))
    .ToImmutableArray();

void PlayNumber(int number)
    => boards
        .SelectMany(x => x.EnumerateAllCells())
        .Where(x => x.Number == number)
        .ForEach(x => x.Marked = true);

Win? win = null;
foreach (var number in numbers)
{
    PlayNumber(number);

    var winningBoard = boards.SingleOrDefault(board => board.IsWinningBoard());
    if (winningBoard is not null)
    {
        win = new Win(winningBoard, number);
        break;
    }
}

if (win is null)
{
    throw new Exception("Nobody won");
}

WriteOutput(1, win.GetScore());

boards.ForEach(x => x.Reset());

ImmutableArray<(int Index, bool Won)> GetBoardStatus()
    => boards.Select((x, i) => (i, x.IsWinningBoard())).ToImmutableArray();
var previousStepBoardStatus = GetBoardStatus();
Win? lastWin = null;
foreach (var number in numbers)
{
    PlayNumber(number);

    var boardStatus = GetBoardStatus();
    if (boardStatus.All(x=>x.Won))
    {
        lastWin = new Win(boards[previousStepBoardStatus.Single(x => !x.Won).Index], number);
        break;
    }
    previousStepBoardStatus = boardStatus;
}

if (lastWin is null)
{
    throw new Exception("something went wrong");
}

WriteOutput(2, lastWin.GetScore());

record Win(Board Board, int Number)
{
    public int GetScore()
        => Number * Board.EnumerateAllCells().Where(x => !x.Marked).Sum(x => x.Number);
}

class BoardCell
{
    public BoardCell(int number, bool marked = false)
    {
        Number = number;
        Marked = marked;
    }

    public int Number { get; set; }
    public bool Marked { get; set; }
}

record Board(BoardCell[][] Cells)
{
    private const int _boardSize = 5;
    private static readonly IEnumerable<int> _indices = Enumerable.Range(0, _boardSize);

    private IEnumerable<BoardCell> GetRow(int row) => Cells[row];
    private IEnumerable<IEnumerable<BoardCell>> EnumerateRows() => _indices.Select(x=> GetRow(x));
    private IEnumerable<BoardCell> GetColumn(int column) => Cells.Select(x => x[column]);
    private IEnumerable<IEnumerable<BoardCell>> EnumerateColumns() => _indices.Select(x => GetColumn(x));
    private IEnumerable<IEnumerable<BoardCell>> EnumerateRowsAndColumns() => EnumerateRows().Concat(EnumerateColumns());

    public bool IsWinningBoard() => EnumerateRowsAndColumns().Any(x => x.All(c => c.Marked));
    public IEnumerable<BoardCell> EnumerateAllCells() => Cells.SelectMany(x => x);
    public void Reset() => EnumerateAllCells().ForEach(x=>x.Marked = false);
}
