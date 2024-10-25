using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace csvWirter
{
	public static class CsvWorker
	{
		public static void WriteFile<T>(string path, List<T> content)
		{
			using (var writer = new StreamWriter(path))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.WriteRecords(content);
			}
		}

		public static List<T> ReadFile<T>(string path)
		{
			var content = new List<T>();
			using (var reader = new StreamReader(path))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				return csv.GetRecords<T>().ToList();
			}
		}
	}
}
