using Prism.Commands;
using Prism.Mvvm;
using SnakeWpf.Models;
using System.Windows;
using System.Windows.Input;

namespace SnakeWpf.ViewModels
{
    internal class MainWindVM : BindableBase
    {
        private bool _continueGame;

        public bool ContinueGame
        {
            get => _continueGame;
            private set
            {
                _continueGame = value;
                RaisePropertyChanged(nameof(ContinueGame));

                if (ContinueGame) { SnakeGo(); }
            }
        }
        public DelegateCommand StartStopCommand { get; }
        public List<List<CellVM>> AllCells { get; } = new List<List<CellVM>>();

        private MoveDirection _currentDirection = MoveDirection.Right;

        private int _rowCount = 20;
        private int _columnCount = 20;

        public static int _speed = SPEED;
        private const int SPEED = 400;

        private Snake _snake;
        private MainWindow _mainWind;
        private CellVM _lastFood;

        public MainWindVM(MainWindow mainWind)
        {
            _mainWind = mainWind;
            StartStopCommand = new DelegateCommand(() =>
            {
                ContinueGame = !ContinueGame;
            });

            for (int row = 0; row < _rowCount; row++)
            {
                var rowList = new List<CellVM>();
                for (int column = 0; column < _columnCount; column++)
                {
                    var cell = new CellVM(row, column);
                    rowList.Add(cell);
                }
                AllCells.Add(rowList);
            }

            _snake = new Snake(AllCells, AllCells[_rowCount / 2][_columnCount / 2], CreateFood);
            CreateFood();

            _mainWind.KeyDown += UserKeyDown;
        }

        private async Task SnakeGo()
        {
            while (ContinueGame)
            {
                await Task.Delay(_speed);
                Snake.RenderLabelGameScore();
                try
                {
                    _snake.Move(_currentDirection);
                }
                catch (Exception ex)
                {
                    ContinueGame = false;
                    MessageBox.Show(ex.Message);
                    _speed = SPEED;
                    _snake.Restart();
                    _lastFood.CellType = CellType.None;
                    CreateFood();
                }
            }
        }
        private void UserKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    if (_currentDirection != MoveDirection.Right)
                        _currentDirection = MoveDirection.Left; break;
                case Key.D:
                    if (_currentDirection != MoveDirection.Left)
                        _currentDirection = MoveDirection.Right; break;
                case Key.W:
                    if (_currentDirection != MoveDirection.Down)
                        _currentDirection = MoveDirection.Up; break;
                case Key.S:
                    if (_currentDirection != MoveDirection.Up)
                        _currentDirection = MoveDirection.Down; break;
                default: break;

            }
        }
        private void CreateFood()
        {
            var rnd = new Random();

            int row = rnd.Next(0, _columnCount);
            int col = rnd.Next(0, _rowCount);

            _lastFood = AllCells[row][col];
            if (_snake.SnakeCells.Contains(_lastFood))
            {
                CreateFood();
            }
            _lastFood.CellType = CellType.Food;
            _speed = (int)(_speed * 0.95);
        }
    }
}
