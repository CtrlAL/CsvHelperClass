using CsvHelper;
using System.Collections.Generic;
using System.Linq;
using static csvWirter.MapperClasses;
using PropNameMaper = csvWirter.MapperClasses.PropNameMaper;
using PropValueMaper = csvWirter.MapperClasses.PropValueMaper;

namespace csvWirter
{
	public class Program
	{
		private static readonly string pathProps = "..\\..\\props.csv";
		private static readonly string pathNames = "..\\..\\names.csv";
		private static readonly string pathValues = "..\\..\\values.csv";
		static void Main(string[] args)
		{
			//var values = CsvWorker.ReadFile<PropValueMaper>(pathValues);
			//var names = CsvWorker.ReadFile<PropNameMaper>(pathNames);

			//var nameSet = new HashSet<PropNameMaper>(names).ToList();
			//var valueSet = new HashSet<PropValueMaper>(values).ToList();

			var properties = CsvWorker.ReadFile<Property>(pathProps);
			WriteProperties(properties);
		}

		public static void WriteProperties(IList<Property> propertiesList)
		{
			var properties = new HashSet<Property>();
			var names = new List<PropNameMaper>();
			var values = new List<PropValueMaper>();

			PorpsNamesValuesExtract(propertiesList, out properties, out names, out values);

			CsvWorker.WriteFile(pathProps, properties.ToList());
			CsvWorker.WriteFile(pathNames, names);
			CsvWorker.WriteFile(pathValues, values);
		}

		public static void PorpsNamesValuesExtract(IList<Property> propertiesList,
			out HashSet<Property> properties,
			out List<PropNameMaper> propertiesNames,
			out List<PropValueMaper> propertiesValues)
		{
			properties = new HashSet<Property>(propertiesList);

			propertiesNames = new HashSet<PropNameMaper>(properties.Select(p => new PropNameMaper
			{
				Id = 0,
				Name = p.Name,
				SubCategoryName = p.SubCategoryName,
			})).OrderBy(p => p.SubCategoryName).ToList().Select((item, Index) =>
			{
				item.Id = Index + 1;
				return item;
			}).OrderBy(pn => pn.Id).ToList();

			var names = propertiesNames;

			propertiesValues = new HashSet<PropValueMaper>(properties.Select((item) => new PropValueMaper
			{
				Id = 0,
				PropertyId = names.Find(pn => item.Name == pn.Name && pn.SubCategoryName == item.SubCategoryName).Id,
				Value = item.Value,
				SubCategoryName = item.SubCategoryName
			})).OrderBy(pv => pv.PropertyId).Select((item, Index) =>
			{
				item.Id = Index + 1;
				return item;
			}).ToList();
		}
	}
}
