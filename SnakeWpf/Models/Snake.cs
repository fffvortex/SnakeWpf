using SnakeWpf.ViewModels;

namespace SnakeWpf.Models
{
    internal class Snake
    {
        public Queue<CellVM> SnakeCells { get; } = new Queue<CellVM>();

        private List<List<CellVM>> _allCells;

        private CellVM _start;

        private Action _generateFood;

        private static int _score = 0;

        public Snake( List<List<CellVM>> allCells, CellVM start, Action generateFood)
        {
            _start = start;
            _allCells = allCells;
            _start.CellType = CellType.Snake;
            SnakeCells.Enqueue(start);
            _generateFood = generateFood;
        }

        public void Restart()
        {
            foreach(var cell in SnakeCells)
            {
                cell.CellType = CellType.None;
            }
            SnakeCells.Clear();
            _score = 0;
            SnakeCells.Enqueue(_start);
        }

        public void Move(MoveDirection direction)
        {
            var leaderCell = SnakeCells.Last();

            int nextRow = leaderCell.Row;
            int nextColumn = leaderCell.Column;
            switch (direction)
            {
                case MoveDirection.Left:
                    nextColumn--;
                    break;
                case MoveDirection.Right:
                    nextColumn++;
                    break;
                case MoveDirection.Up:
                    nextRow--;
                    break;
                case MoveDirection.Down:
                    nextRow++;
                    break;
                default:
                    break;
            }

            try
            {
                var nextCell = _allCells[nextRow][nextColumn];
                switch (nextCell?.CellType)
                {
                    case CellType.None:
                        nextCell.CellType = CellType.Snake;
                        SnakeCells.Dequeue().CellType = CellType.None;
                        SnakeCells.Enqueue(nextCell);
                        break;
                    case CellType.Food:
                        nextCell.CellType = CellType.Snake;
                        SnakeCells.Enqueue(nextCell);
                        _score++;
                        _generateFood?.Invoke();
                        break;
                    default: throw _gameOverEx;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw _gameOverEx;
            }
        }
        private Exception _gameOverEx => new Exception("Game over");
        public static void RenderLabelGameScore()
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).TextBlockScoreSpeed.Text = $"Score: {_score} | Speed: {(1000 - MainWindVM._speed)}";
        }
    }
}
