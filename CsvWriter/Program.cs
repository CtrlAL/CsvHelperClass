using System.Linq;
using PropNameMaper = csvWirter.MapperClasses.PropNameMaper;
using PropValueMaper = csvWirter.MapperClasses.PropValueMaper;

namespace csvWirter
{
	public class Program
	{
		private static readonly string pathNames = "..\\..\\names.csv";
		private static readonly string pathValues = "..\\..\\values.csv";
		static void Main(string[] args)
		{
			var values = CsvWorker.ReadFile<PropValueMaper>(pathValues);
			var names = CsvWorker.ReadFile<PropNameMaper>(pathNames);
			
			if (names.Where(v => v.Id == 0).Count() > 0)
			{
				names.Select((name) =>
				{
					name.Id++;
					return name;
				}).ToList();
			}

			CsvWorker.WriteFile(pathNames, names);

			if (values.Where(v => v.Id == 0).Count() > 0)
			{
				values = values.OrderBy(v => v.PropertyId).Select((value, index) =>
				{
					value.Id = index + 1;
					value.PropertyId = value.PropertyId + 1;
					return value;
				}).ToList();
			}

			CsvWorker.WriteFile(pathValues, values);
		}
	}
}
