using System;
using System.Text;

namespace System.Numerics.Vectors
{
	// A collection of Integer Vectors that use BigInteger instead of 32/64 Big Integers.

	/// <summary>
	/// Representation of a 2D Vector using BigIntegers.
	/// </summary>
	public struct BigVector2Int : IFormattable
	{
		#region Values
		private BigInteger x;
		private BigInteger y;
		#endregion

		#region Properties
		public BigInteger X { get => x; set => x = value; }
		public BigInteger Y { get => y; set => y = value; }
		public BigInteger this[int index]
		{
			get => index switch
			{ 0 => x, 1 => y, _ => throw new IndexOutOfRangeException() };
			set => _ = index switch
			{ 0 => x = value, 1 => y = value, _ => throw new IndexOutOfRangeException() };
		}
		#endregion

		#region Constructors
		public BigVector2Int(BigInteger x, BigInteger y)
		{
			this.x = x;
			this.y = y;
		}
		#endregion

		#region Constants
		public static BigVector2Int Left { get => new BigVector2Int(-1, 0); }
		public static BigVector2Int Right { get => new BigVector2Int(1, 0); }
		public static BigVector2Int Up { get => new BigVector2Int(0, 1); }
		public static BigVector2Int Down { get => new BigVector2Int(0, -1); }
		public static BigVector2Int Zero { get => new BigVector2Int(0, 0); }
		public static BigVector2Int One { get => new BigVector2Int(1, 1); }
		public static BigVector2Int MinusOne { get => new BigVector2Int(-1, -1); }
		#endregion

		#region Instance methods
		public override bool Equals(object? obj) => obj is BigVector2Int vector && this == vector;

		public override int GetHashCode() => -0; // TODO: Implement this

		public override string ToString() => ToString(null, null);

		public string ToString(string? format) => ToString(format, null);

		public string ToString(string? format, IFormatProvider? provider) => $"({x.ToString(format, provider)}, {y.ToString(format, provider)})";
		#endregion

		#region Static methods
		public static BigVector2Int Abs(BigVector2Int value) => new BigVector2Int(BigInteger.Abs(value.X), BigInteger.Abs(value.Y));
		#endregion

		#region Operator overloads
		public static BigVector2Int operator +(BigVector2Int a, BigVector2Int b) => new BigVector2Int(a.X + b.X, a.Y + b.Y);
		public static BigVector2Int operator +(BigVector2Int a, int b) => new BigVector2Int(a.X + b, a.Y + b);
		public static BigVector2Int operator +(BigVector2Int a, long b) => new BigVector2Int(a.X + b, a.Y + b);
		public static BigVector2Int operator +(BigVector2Int a, BigInteger b) => new BigVector2Int(a.X + b, a.Y + b);
		public static BigVector2Int operator +(BigVector2Int a) => a;

		public static BigVector2Int operator -(BigVector2Int a, BigVector2Int b) => new BigVector2Int(a.X - b.X, a.Y - b.Y);
		public static BigVector2Int operator -(BigVector2Int a, int b) => new BigVector2Int(a.X - b, a.Y - b);
		public static BigVector2Int operator -(BigVector2Int a, long b) => new BigVector2Int(a.X - b, a.Y - b);
		public static BigVector2Int operator -(BigVector2Int a, BigInteger b) => new BigVector2Int(a.X - b, a.Y - b);
		public static BigVector2Int operator -(BigVector2Int a) => new BigVector2Int(-a.X, -a.Y);

		public static BigVector2Int operator *(BigVector2Int a, BigVector2Int b) => new BigVector2Int(a.X * b.X, a.Y * b.Y);
		public static BigVector2Int operator *(BigVector2Int a, int b) => new BigVector2Int(a.X * b, a.Y * b);
		public static BigVector2Int operator *(BigVector2Int a, long b) => new BigVector2Int(a.X * b, a.Y * b);
		public static BigVector2Int operator *(BigVector2Int a, BigInteger b) => new BigVector2Int(a.X * b, a.Y * b);


		public static BigVector2Int operator /(BigVector2Int a, BigVector2Int b) => new BigVector2Int(a.X / b.X, a.Y / b.Y);
		public static BigVector2Int operator /(BigVector2Int a, int b) => new BigVector2Int(a.X / b, a.Y / b);
		public static BigVector2Int operator /(BigVector2Int a, long b) => new BigVector2Int(a.X / b, a.Y / b);
		public static BigVector2Int operator /(BigVector2Int a, BigInteger b) => new BigVector2Int(a.X / b, a.Y / b);

		public static BigVector2Int operator %(BigVector2Int a, BigVector2Int b) => new BigVector2Int(a.X % b.X, a.Y % b.Y);
		public static BigVector2Int operator %(BigVector2Int a, int b) => new BigVector2Int(a.X % b, a.Y % b);
		public static BigVector2Int operator %(BigVector2Int a, long b) => new BigVector2Int(a.X % b, a.Y % b);
		public static BigVector2Int operator %(BigVector2Int a, BigInteger b) => new BigVector2Int(a.X % b, a.Y % b);

		public static bool operator ==(BigVector2Int a, BigVector2Int b) => a.X == b.X && a.Y == b.Y;
		public static bool operator !=(BigVector2Int a, BigVector2Int b) => a.X != b.X || a.Y != b.Y;
		#endregion
	}

	/// <summary>
	/// Representation of a 3D Vector using BigIntegers.
	/// </summary>
	public struct BigVector3Int : IFormattable
	{
		#region Values
		private BigInteger x;
		private BigInteger y;
		private BigInteger z;
		#endregion

		#region Properties
		public BigInteger X { get => x; set => x = value; }
		public BigInteger Y { get => y; set => y = value; }
		public BigInteger Z { get => z; set => z = value; }
		public BigInteger this[int index]
		{
			get => index switch
			{ 0 => x, 1 => y, 2 => z, _ => throw new IndexOutOfRangeException() };
			set => _ = index switch
			{ 0 => x = value, 1 => y = value, 2 => z = value, _ => throw new IndexOutOfRangeException() };
		}
		#endregion

		#region Constructors
		public BigVector3Int(BigInteger x, BigInteger y, BigInteger z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		#endregion

		#region Constants
		public static BigVector3Int Left { get => new BigVector3Int(-1, 0, 0); }
		public static BigVector3Int Right { get => new BigVector3Int(1, 0, 0); }
		public static BigVector3Int Up { get => new BigVector3Int(0, 1, 0); }
		public static BigVector3Int Down { get => new BigVector3Int(0, -1, 0); }
		public static BigVector3Int Forward { get => new BigVector3Int(0, 0, 1); }
		public static BigVector3Int Backward { get => new BigVector3Int(0, 0, -1); }
		public static BigVector3Int Zero { get => new BigVector3Int(0, 0, 0); }
		public static BigVector3Int One { get => new BigVector3Int(1, 1, 1); }
		public static BigVector3Int MinusOne { get => new BigVector3Int(-1, -1, -1); }
		#endregion

		#region Instance methods
		public override bool Equals(object? obj) => obj is BigVector3Int vector && this == vector;

		public override int GetHashCode() => -0; // TODO: Implement

		public override string ToString() => ToString(null, null);

		public string ToString(string? format) => ToString(format, null);

		public string ToString(string? format, IFormatProvider? provider) => $"({x.ToString(format, provider)}, {y.ToString(format, provider)}, {z.ToString(format, provider)})";
		#endregion

		#region Static methods
		public static BigVector3Int Abs(BigVector3Int value) => new BigVector3Int(BigInteger.Abs(value.X), BigInteger.Abs(value.Y), BigInteger.Abs(value.Z));
		#endregion

		#region Operator overloads
		public static BigVector3Int operator +(BigVector3Int a, BigVector3Int b) => new BigVector3Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		public static BigVector3Int operator +(BigVector3Int a, int b) => new BigVector3Int(a.X + b, a.Y + b, a.Z + b);
		public static BigVector3Int operator +(BigVector3Int a, long b) => new BigVector3Int(a.X + b, a.Y + b, a.Z + b);
		public static BigVector3Int operator +(BigVector3Int a, BigInteger b) => new BigVector3Int(a.X + b, a.Y + b, a.Z + b);
		public static BigVector3Int operator +(BigVector3Int a) => a;

		public static BigVector3Int operator -(BigVector3Int a, BigVector3Int b) => new BigVector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		public static BigVector3Int operator -(BigVector3Int a, int b) => new BigVector3Int(a.X - b, a.Y - b, a.Z - b);
		public static BigVector3Int operator -(BigVector3Int a, long b) => new BigVector3Int(a.X - b, a.Y - b, a.Z - b);
		public static BigVector3Int operator -(BigVector3Int a, BigInteger b) => new BigVector3Int(a.X - b, a.Y - b, a.Z - b);
		public static BigVector3Int operator -(BigVector3Int a) => new BigVector3Int(-a.X, -a.Y, -a.Z);

		public static BigVector3Int operator *(BigVector3Int a, BigVector3Int b) => new BigVector3Int(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		public static BigVector3Int operator *(BigVector3Int a, int b) => new BigVector3Int(a.X * b, a.Y * b, a.Z * b);
		public static BigVector3Int operator *(BigVector3Int a, long b) => new BigVector3Int(a.X * b, a.Y * b, a.Z * b);
		public static BigVector3Int operator *(BigVector3Int a, BigInteger b) => new BigVector3Int(a.X * b, a.Y * b, a.Z * b);

		public static BigVector3Int operator /(BigVector3Int a, BigVector3Int b) => new BigVector3Int(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
		public static BigVector3Int operator /(BigVector3Int a, int b) => new BigVector3Int(a.X / b, a.Y / b, a.Z / b);
		public static BigVector3Int operator /(BigVector3Int a, long b) => new BigVector3Int(a.X / b, a.Y / b, a.Z / b);
		public static BigVector3Int operator /(BigVector3Int a, BigInteger b) => new BigVector3Int(a.X / b, a.Y / b, a.Z / b);

		public static BigVector3Int operator %(BigVector3Int a, BigVector3Int b) => new BigVector3Int(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
		public static BigVector3Int operator %(BigVector3Int a, int b) => new BigVector3Int(a.X % b, a.Y % b, a.Z % b);
		public static BigVector3Int operator %(BigVector3Int a, long b) => new BigVector3Int(a.X % b, a.Y % b, a.Z % b);
		public static BigVector3Int operator %(BigVector3Int a, BigInteger b) => new BigVector3Int(a.X % b, a.Y % b, a.Z % b);

		public static bool operator ==(BigVector3Int a, BigVector3Int b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
		public static bool operator !=(BigVector3Int a, BigVector3Int b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;
		#endregion Operator overloads
	}

	/// <summary>
	/// Representation of a 4D Vector using BigIntegers.
	/// </summary>
	public struct BigVector4Int : IFormattable
	{
		#region Values
		private BigInteger x;
		private BigInteger y;
		private BigInteger z;
		private BigInteger w;
		#endregion

		#region Properties
		public BigInteger X { get => x; set => x = value; }
		public BigInteger Y { get => y; set => y = value; }
		public BigInteger Z { get => z; set => z = value; }
		public BigInteger W { get => w; set => w = value; }
		public BigInteger this[int index]
		{
			get => index switch
			{ 0 => x, 1 => y, 2 => z, 3 => w, _ => throw new IndexOutOfRangeException() };
			set => _ = index switch
			{ 0 => x = value, 1 => y = value, 2 => z = value, 3 => w = value, _ => throw new IndexOutOfRangeException() };
		}
		#endregion

		#region Constructors
		public BigVector4Int(BigInteger x, BigInteger y, BigInteger z, BigInteger w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
		#endregion

		#region Constants
		public static BigVector4Int Left { get => new BigVector4Int(-1, 0, 0, 0); }
		public static BigVector4Int Right { get => new BigVector4Int(1, 0, 0, 0); }
		public static BigVector4Int Up { get => new BigVector4Int(0, 1, 0, 0); }
		public static BigVector4Int Down { get => new BigVector4Int(0, -1, 0, 0); }
		public static BigVector4Int Forward { get => new BigVector4Int(0, 0, 1, 0); }
		public static BigVector4Int Backward { get => new BigVector4Int(0, 0, -1, 0); }
		public static BigVector4Int Increase { get => new BigVector4Int(0, 0, 0, 1); }
		public static BigVector4Int Decrease { get => new BigVector4Int(0, 0, 0, -1); }
		public static BigVector4Int Zero { get => new BigVector4Int(0, 0, 0, 0); }
		public static BigVector4Int One { get => new BigVector4Int(1, 1, 1, 1); }
		public static BigVector4Int MinusOne { get => new BigVector4Int(-1, -1, -1, -1); }
		#endregion

		#region Instance methods
		public override bool Equals(object? obj) => obj is BigVector4Int vector && this == vector;

		public override int GetHashCode() => -0; // TODO: Implement

		public override string ToString() => ToString(null, null);

		public string ToString(string? format) => ToString(format, null);

		public string ToString(string? format, IFormatProvider? provider) => $"({x.ToString(format, provider)}, {y.ToString(format, provider)}, {z.ToString(format, provider)}, {w.ToString(format, provider)})";
		#endregion

		#region Static methods
		public static BigVector4Int Abs(BigVector4Int vector) => new BigVector4Int(BigInteger.Abs(vector.X), BigInteger.Abs(vector.Y), BigInteger.Abs(vector.Z), BigInteger.Abs(vector.W));
		#endregion
		
		#region Operator overloads
		public static BigVector4Int operator +(BigVector4Int a, BigVector4Int b) => new BigVector4Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
		public static BigVector4Int operator +(BigVector4Int a, int b) => new BigVector4Int(a.X + b, a.Y + b, a.Z + b, a.W + b);
		public static BigVector4Int operator +(BigVector4Int a, long b) => new BigVector4Int(a.X + b, a.Y + b, a.Z + b, a.W + b);
		public static BigVector4Int operator +(BigVector4Int a, BigInteger b) => new BigVector4Int(a.X + b, a.Y + b, a.Z + b, a.W + b);
		public static BigVector4Int operator +(BigVector4Int a) => a;

		public static BigVector4Int operator -(BigVector4Int a, BigVector4Int b) => new BigVector4Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
		public static BigVector4Int operator -(BigVector4Int a, int b) => new BigVector4Int(a.X - b, a.Y - b, a.Z - b, a.W - b);
		public static BigVector4Int operator -(BigVector4Int a, long b) => new BigVector4Int(a.X - b, a.Y - b, a.Z - b, a.W - b);
		public static BigVector4Int operator -(BigVector4Int a, BigInteger b) => new BigVector4Int(a.X - b, a.Y - b, a.Z - b, a.W - b);
		public static BigVector4Int operator -(BigVector4Int a) => new BigVector4Int(-a.X, -a.Y, -a.Z, -a.W);

		public static BigVector4Int operator *(BigVector4Int a, BigVector4Int b) => new BigVector4Int(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
		public static BigVector4Int operator *(BigVector4Int a, int b) => new BigVector4Int(a.X * b, a.Y * b, a.Z * b, a.W * b);
		public static BigVector4Int operator *(BigVector4Int a, long b) => new BigVector4Int(a.X * b, a.Y * b, a.Z * b, a.W * b);
		public static BigVector4Int operator *(BigVector4Int a, BigInteger b) => new BigVector4Int(a.X * b, a.Y * b, a.Z * b, a.W * b);

		public static BigVector4Int operator /(BigVector4Int a, BigVector4Int b) => new BigVector4Int(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
		public static BigVector4Int operator /(BigVector4Int a, int b) => new BigVector4Int(a.X / b, a.Y / b, a.Z / b, a.W / b);
		public static BigVector4Int operator /(BigVector4Int a, long b) => new BigVector4Int(a.X / b, a.Y / b, a.Z / b, a.W / b);
		public static BigVector4Int operator /(BigVector4Int a, BigInteger b) => new BigVector4Int(a.X / b, a.Y / b, a.Z / b, a.W / b);

		public static BigVector4Int operator %(BigVector4Int a, BigVector4Int b) => new BigVector4Int(a.X % b.X, a.Y % b.Y, a.Z % b.Z, a.W % b.W);
		public static BigVector4Int operator %(BigVector4Int a, int b) => new BigVector4Int(a.X % b, a.Y % b, a.Z % b, a.W % b);
		public static BigVector4Int operator %(BigVector4Int a, long b) => new BigVector4Int(a.X % b, a.Y % b, a.Z % b, a.W % b);
		public static BigVector4Int operator %(BigVector4Int a, BigInteger b) => new BigVector4Int(a.X % b, a.Y % b, a.Z % b, a.W % b);

		public static bool operator ==(BigVector4Int a, BigVector4Int b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
		public static bool operator !=(BigVector4Int a, BigVector4Int b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z || a.W != b.W;
		#endregion
	}
}