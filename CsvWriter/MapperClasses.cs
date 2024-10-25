using System;

namespace csvWirter
{
	public static class MapperClasses
	{
		public class PropNameMaper
		{
			public int Id { get; set; }
			public string SubCategoryName { get; set; }
			public string Name { get; set; }

			public override bool Equals(object obj)
			{
				var item = obj as PropNameMaper;

				return item != null &&
				   Name == item.Name &&
				   SubCategoryName == item.SubCategoryName;
			}
			public override int GetHashCode()
			{
				return HashCode.Combine(Name, SubCategoryName);
			}
		}

		public class PropValueMaper
		{
			public int Id { get; set; }
			public int PropertyId { get; set; }
			public string SubCategoryName { get; set; }
			public string Value { get; set; }

			public override bool Equals(object obj)
			{
				var item = obj as PropValueMaper;
				return item != null &&
				   Value == item.Value &&
				   SubCategoryName == item.SubCategoryName &&
				   PropertyId == item.PropertyId;
			}
			public override int GetHashCode()
			{
				return HashCode.Combine(Value, SubCategoryName, PropertyId);
			}
		}
	}
}
