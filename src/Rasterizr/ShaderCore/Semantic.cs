namespace Rasterizr.ShaderCore
{
	public struct Semantic
	{
		public string Name { get; private set; }
		public int Index { get; private set; }
		public SystemValueType SystemValue { get; private set; }

		public Semantic(string name, int index)
			: this()
		{
			Name = name;
			Index = index;
		}

		public Semantic(SystemValueType systemValue)
			: this()
		{
			SystemValue = systemValue;
		}

		public static bool operator==(Semantic left, Semantic right)
		{
			return left.Name == right.Name && left.Index == right.Index && left.SystemValue == right.SystemValue;
		}

		public static bool operator !=(Semantic left, Semantic right)
		{
			return !(left == right);
		}

		public bool Equals(Semantic other)
		{
			return Equals(other.Name, Name) && other.Index == Index && Equals(other.SystemValue, SystemValue);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof (Semantic)) return false;
			return Equals((Semantic) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = (Name != null ? Name.GetHashCode() : 0);
				result = (result * 397) ^ Index;
				result = (result * 397) ^ SystemValue.GetHashCode();
				return result;
			}
		}

		public override string ToString()
		{
			return Name + Index;
		}
	}
}