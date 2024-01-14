namespace SudokuBlaze.Data
{
	internal class Cell
	{

		public int Row { get; }
		public int Column { get; }
		public InnerGrid Grid { get; }
		public string Classes => $"cell{(IsValid() ? "" : " invalid")}{(IsHint ? " hint" : "")}{(IsDisabled ? " disabled" : "")}";

		private int _solution;
		public int Value { get; private set; }
		public bool IsCorrect => Value == _solution;

		public bool IsStarter { get; set; }
		public bool IsHint { get; set; }
		public bool IsDisabled => Value != 0 && (IsStarter || Grid.IsSolved());

		public Cell(InnerGrid grid, int row, int column)
		{
			Grid = grid;
			Row = row;
			Column = column;
		}

		public bool SetValue(int value, bool starterValue = false)
		{
			IsHint = false;

			bool valid = true;

			for (int i = 0; i < 3; i++)
			{
				if (value == 0) break;
				if (Grid.RowHasValue(i, value)) valid = false;
			}

			if (value < 0) value = 0;

			Value = value;
			IsStarter = starterValue;

			return valid;
		}

		public bool SetValue(int value, int solution)
		{
			_solution = solution;
			return SetValue(value, true);
		}

		public void SetHintValue()
		{
			SetValue(_solution);
			IsHint = true;
		}

		public bool IsValid()
		{
			if (Value == 0) return true;

			// Checks if more than 1 cell in a square, row or column have the same value
			if (Enumerable.Range(0, 3).Count(i => Grid.OuterGrid[Grid.Row, Grid.Column].RowHasValue(i, Value)) > 1) return false;
			if (Enumerable.Range(0, 3).Count(i => Grid.OuterGrid[Grid.Row, Grid.Column].ColumnHasValue(i, Value)) > 1) return false;
			if (Enumerable.Range(0, 3).Count(i => Grid.OuterGrid[Grid.Row, i].RowHasValue(Row, Value)) > 1) return false;
			return !(Enumerable.Range(0, 3).Count(i => Grid.OuterGrid[i, Grid.Column].ColumnHasValue(Column, Value)) > 1);
		}
	}
}
