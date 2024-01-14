namespace SudokuBlaze.Data
{
	internal class InnerGrid
	{
		private readonly Cell[,] _cells = new Cell[3,3];

		public int Row { get; }
		public int Column { get; }
		public InnerGrid[,] OuterGrid { get; }

		public InnerGrid(int row, int column, InnerGrid[,] outerGrid)
		{
			for (int iRow = 0; iRow < 3; iRow++)
				for (int iColumn = 0; iColumn < 3; iColumn++)
					_cells[iRow, iColumn] = new Cell(this, iRow, iColumn);

			Row = row;
			Column = column;
			OuterGrid = outerGrid;
		}

		public void FillFromArray(int[] cells, int[] solutions)
		{
			int i = 0;
			foreach (Cell cell in _cells)
				cell.SetValue(cells[i], solutions[i++]);
		}

		public void FillFromArray(int[] cells)
		{
			int i = 0;
			foreach (Cell cell in _cells)
				cell.SetValue(cells[i++]);
		}

		public Cell GetCell(int row, int column) => _cells[row, column];

		public bool RowHasValue(int row, int value) => Enumerable.Range(0, 3).Any(i => _cells[row, i].Value == value);

		public bool ColumnHasValue(int column, int value) => Enumerable.Range(0, 3).Any(i => _cells[i, column].Value == value);

		public bool IsSolved() => _cells.Cast<Cell>().All(cell => cell.IsCorrect);
		
	}
}