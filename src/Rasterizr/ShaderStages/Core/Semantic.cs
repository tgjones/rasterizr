namespace Rasterizr.ShaderStages.Core
{
	public class Semantic
	{
		public string Name { get; private set; }
		public int Index { get; private set; }

		public Semantic(string name, int index)
		{
			Name = name;
			Index = index;
		}

		public static bool operator==(Semantic left, Semantic right)
		{
			return left.Name == right.Name && left.Index == right.Index;
		}

		public static bool operator !=(Semantic left, Semantic right)
		{
			return !(left == right);
		}

		public bool Equals(Semantic other)
		{
			return other == this;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof(Semantic)) return false;
			return Equals((Semantic) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Index;
			}
		}
	}
}