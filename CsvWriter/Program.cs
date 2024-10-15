using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace csvWirter
{
	internal class Program
	{
		public class Props
		{
			public string Name { get; set; }

			public string Value { get; set; }

			public override bool Equals(object obj)
			{
				var item = obj as Props;

				return item != null &&
				   Name == item.Name &&
				   Value == item.Value;
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(Name, Value);
			}
		}

		public class PropValue
		{
			public string Value { get; set; }
			public int RealEstateProperiesDictionaryId { get; set; }
		}

		public class PropNameDb
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
		public class PropName
		{
			public string Name { get; set; }

			public override bool Equals(object obj)
			{
				var item = obj as Props;

				return item != null &&
				   Name == item.Name;
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(Name);
			}
		}
		static void Main(string[] args)
		{
			ConvertPropsValue();
		}

		static void ConvertPropsValue()
		{
			var porpValues = new List<PropValue>();
			var propsList = new List<Props>();
			var propNames = new List<PropNameDb>();
			var propsNamePath = "..\\..\\attrs.csv";
			var path = "..\\..\\propertiesCsv.csv";
			var resultPath = "..\\..\\propsValues.csv";

			using (var reader = new StreamReader(propsNamePath))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				propNames = csv.GetRecords<PropNameDb>().ToList();
			}

			var dataList = File.ReadAllLines(path: path);
			foreach (var line in dataList)
			{
				var array = line.Split(',');
				if (array.Count() == 2)
				{
					propsList.Add(new Props
					{
						Name = array[0],
						Value = array[1]
					});
				}
				else
				{
					var name  = array[0];
					var span = new Span<string>(array);
					foreach (var val in span.Slice(1))
					{
						propsList.Add(new Props
						{
							Name = name,
							Value = val
						});
					}
				}
			}

			foreach (var name in propNames)
			{
				foreach (var prop in propsList)
				{
					if (name.Name == prop.Name)
					{
						porpValues.Add(new PropValue
						{
							RealEstateProperiesDictionaryId = name.Id,
							Value = prop.Value.Trim('\"').Trim(' ')
						});
					}
				}
			}

			using (var writer = new StreamWriter(resultPath))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.WriteRecords(porpValues);
			}
		}

		static void ReductAvitoData()
		{
			List<Props> propsList = new List<Props>();

			var path = "..\\..\\props.csv";
			var resultPath = "..\\..\\result.csv";
			var attrNames = "..\\..\\attrNames.csv";

			using (var reader = new StreamReader(path))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				propsList = csv.GetRecords<Props>().ToList();
			}


			var result = new HashSet<Props>(propsList);

			var propNamesList = propsList.Select(p => p.Name);
			var propNames = new HashSet<string>(propNamesList);
			var propNamesSet = propNames.Select(p => new PropName { Name = p });

			using (var writer = new StreamWriter(resultPath))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.WriteRecords(result);
			}

			using (var writer = new StreamWriter(attrNames))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.WriteRecords(propNamesSet);
			}
		}
	}
}
