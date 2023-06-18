namespace System.Numerics
{
	/// <summary>
	/// Represents infinity in a way that doesn't take up all the memory in the universe
	/// </summary>
	public struct Infinity : IFormattable
	{
		byte[] _bits;

		public Infinity() => _bits = new byte[4];

		public override string ToString() => "∞";

		public string ToString(string? format, IFormatProvider? formatProvider) => "∞";        
	}
}