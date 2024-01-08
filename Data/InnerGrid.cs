namespace SudokuBlaze.Data
{
	internal class InnerGrid
	{
		private readonly int[,] _cells = new int[3,3];
		private readonly bool[,] _starterValue = new bool[3,3];

		public int Row { get; set; }
		public int Column { get; set; }
		public bool IsFilled => !Enumerable.Range(0, 3).Any(i => RowHasValue(i, 0));

		public InnerGrid(int row, int column)
		{
			for (int iRow = 0; iRow < 3; iRow++)
			{
				for (int iColumn = 0; iColumn < 3; iColumn++)
				{
					_cells[iRow, iColumn] = 0;
				}
			}

			Row = row;
			Column = column;
		}

		public void FillFromArray(int[] cells)
		{
			int i = 0;
			for (int iRow = 0; iRow < 3; iRow++)
			{
				for (int iColumn = 0; iColumn < 3; iColumn++)
				{
					SetCellValue(iRow, iColumn, cells[i++], true);
				}
			}
		}

		public bool SetCellValue(int row, int column, int value, bool starterValue = false)
		{
			bool valid = true;

			for (int i = 0; i < 3; i++)
			{
				if (value == 0 ) break;
				if (RowHasValue(i, value)) valid = false;
			}

			if (value < 0) value = 0;

			_cells[row, column] = value;
			_starterValue[row, column] = starterValue;

			return valid;
		}

		public int GetCellValue(int row, int column) => _cells[row, column];

		public bool RowHasValue(int row, int value) => Enumerable.Range(0, 3).Any(i => _cells[row, i] == value);

		public bool ColumnHasValue(int column, int value) => Enumerable.Range(0, 3).Any(i => _cells[i, column] == value);

		public bool IsStarter(int row, int column) => _starterValue[row, column];

		public override string ToString()
		{

			string str = string.Empty;

			for (int row = 0; row < 3; row++)
			{
				str += "[";
				for (int column = 0; column < 3; column++)
				{
					string val = _cells[row, column] == 0 ? "" : _cells[row, column].ToString();
					str = $"{str} {val},";
				}
				str = str.Remove(str.Length - 1);
				str += "] ";
			}

			return str;
		}
	}
}