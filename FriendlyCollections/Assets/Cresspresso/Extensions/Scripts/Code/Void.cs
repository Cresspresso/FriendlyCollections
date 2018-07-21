namespace Cresspresso
{
	/// <summary>
	/// Struct that can be used to represent <see langword="void"/> in place of a type parameter.
	/// </summary>
	public struct Void
	{
		/// <summary>
		/// Singleton instance. Has the same effect as <c>new Void()</c>.
		/// </summary>
		public static readonly Void value = new Void();

		/// <summary>
		/// Returns <see langword="true"/> if <paramref name="obj"/> inherits from <see cref="Void"/>. Otherwise <see langword="false"/>.
		/// </summary>
		public override bool Equals(object obj)
		{
			return obj is Void;
		}

		/// <summary>
		/// Returns <c>0</c>.
		/// </summary>
		public override int GetHashCode()
		{
			return 0;
		}

		/// <summary>
		/// Returns <c>"Void"</c>.
		/// </summary>
		public override string ToString()
		{
			return "Void";
		}
	}
}
