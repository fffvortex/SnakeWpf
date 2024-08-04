using Prism.Mvvm;
using SnakeWpf.Models;

namespace SnakeWpf.ViewModels
{
    internal class CellVM : BindableBase
    {
        public CellVM(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public int Row { get; }

        public int Column { get; }

        private CellType _cellType = CellType.None;

        public CellType CellType
        {
            get => _cellType;
            set
            {
                _cellType = value;
                RaisePropertyChanged(nameof(CellType));
            }
        }

    }
}
