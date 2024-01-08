using Newtonsoft.Json;

namespace SudokuBlaze.Data
{
	internal class GameService
	{
		public InnerGrid[,] Grid { get; } = new InnerGrid[3,3];
		public InnerGrid[,] Solution { get; } = new InnerGrid[3, 3];

		private readonly HttpClient _httpClient;

		public GameService()
		{
			_httpClient = new HttpClient();

			for (int row = 0; row < 3; row++)
			{
				for (int column = 0; column < 3; column++)
				{
					Grid[row, column] = new InnerGrid(row, column);
					Solution[row, column] = new InnerGrid(row, column);
				}
			}
		}

		public void ClearGame()
		{

			for (int row = 0; row < 3; row++)
			{
				for (int column = 0; column < 3; column++)
				{
					Grid[row, column].FillFromArray(Enumerable.Repeat(0, 9).ToArray());
				}
			}
		}

		public async Task StartNewGame()
		{
			HttpRequestMessage request = new(HttpMethod.Get, "https://sudoku-api.vercel.app/api/dosuku");

			HttpResponseMessage response = await _httpClient.SendAsync(request);

			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();
				DosukuJsonData data = JsonConvert.DeserializeObject<DosukuJsonData>(content);
				DosukuJsonData.Grid newGrid = data.newboard.grids[0];
				for (int row = 0; row < 3; row++)
				{
					for (int column = 0; column < 3; column++)
					{
						Grid[row, column].FillFromArray(GetCellArray(newGrid.value, row, column));
						Solution[row, column].FillFromArray(GetCellArray(newGrid.solution, row, column));
					}
				}
			}
		}

		public void GetHint()
		{
			Random random = new();
			bool exit = false;

			while (!exit)
			{
				int x = Rng(), y = Rng(), z = Rng(), i = Rng();

				if (Grid[x,y].GetCellValue(z,i) != 0) continue;

				int value = Solution[x, y].GetCellValue(z,i);
				Grid[x, y].SetCellValue(z, i, value);
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
