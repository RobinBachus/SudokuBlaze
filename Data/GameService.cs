#nullable enable
using Newtonsoft.Json;

namespace SudokuBlaze.Data
{
	internal class GameService
	{
		public InnerGrid[,] Grid { get; } = new InnerGrid[3,3];
		public bool GameWon => Grid.Cast<InnerGrid>().All(innerGrid => innerGrid.IsSolved());
		public string Difficulty { get; private set; } = "Not started";

		private readonly HttpClient _httpClient;

		public GameService()
		{
			_httpClient = new HttpClient();

			for (int row = 0; row < 3; row++)
			{
				for (int column = 0; column < 3; column++)
				{
					Grid[row, column] = new InnerGrid(row, column, Grid);
				}
			}
		}

		public void ClearGame()
		{
			foreach (InnerGrid innerGrid in Grid)
				innerGrid.FillFromArray(Enumerable.Repeat(0, 9).ToArray());
			Difficulty = "Not started";
		}

		public async Task StartNewGame(string difficulty)
		{
			// I wish I could do it better, but I don't want to change api so ¯\_(ツ)_/¯
			DosukuJsonData.Grid? newGrid;
			do
			{
				newGrid = await GetNewGrid();
				if (newGrid is null) return;
			} while (difficulty != "Random" && newGrid.difficulty != difficulty);

			foreach (InnerGrid innerGrid in Grid)
			{
				int row = innerGrid.Row, column = innerGrid.Column;
				innerGrid.FillFromArray(GetCellArray(newGrid.value, row, column), GetCellArray(newGrid.solution, row, column));
			}

			Difficulty = newGrid.difficulty;
		}

		private async Task<DosukuJsonData.Grid?> GetNewGrid()
		{
			HttpRequestMessage request = new(HttpMethod.Get, "https://sudoku-api.vercel.app/api/dosuku");
			HttpResponseMessage response = await _httpClient.SendAsync(request);

			if (!response.IsSuccessStatusCode) return null;

			string content = await response.Content.ReadAsStringAsync();
			DosukuJsonData? data = JsonConvert.DeserializeObject<DosukuJsonData>(content);

			return data?.newboard.grids[0];
		}

		public void GetHint()
		{
			if (GameWon) return;

			Random random = new();
			bool exit = false;

			while (!exit)
			{
				Cell cell = Grid[Rng(), Rng()].GetCell(Rng(), Rng());
				if (cell.IsCorrect) continue;

				cell.SetHintValue();
				exit = true;
			}

			return;

			int Rng() => random.Next(3);
		}

		private static int[] GetCellArray(IReadOnlyList<List<int>> grid, int row, int column)
		{
			// Offsets
			row *= 3;
			column *= 3;

			List<int> cells = new();
			cells.AddRange(grid[row].GetRange(column, 3));
			cells.AddRange(grid[row+1].GetRange(column, 3));
			cells.AddRange(grid[row+2].GetRange(column, 3));

			return cells.ToArray();
		}

	}
}
