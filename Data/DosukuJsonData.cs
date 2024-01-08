using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuBlaze.Data
{
	internal class DosukuJsonData
	{
		// DosukuJsonData myDeserializedClass = JsonConvert.DeserializeObject<DosukuJsonData>(myJsonResponse);

		public Newboard newboard { get; set; }

		public class Grid
		{
			public List<List<int>> value { get; set; }
			public List<List<int>> solution { get; set; }
			public string difficulty { get; set; }
		}

		public class Newboard
		{
			public List<Grid> grids { get; set; }
			public int results { get; set; }
			public string message { get; set; }
		}
	}
}
