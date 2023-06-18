using Newtonsoft.Json;
using SSUnlimited.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
















/*
namespace System.Numerics
{
	public struct BigDecimal : IFormattable
	{
		#region Internal values
		internal BigInteger _value;
		internal BigInteger _scale;
		internal BigInteger _precision;
		private byte _flags;
		#endregion

		#region Properties
		/// <summary>
		/// The unscaled value of the BigDecimal.
		/// </summary>
		public BigInteger Mantissa { get => _value; set => _value = value; }

		/// <summary>
		/// The decimal point location of the BigDecimal.
		/// </summary>
		public BigInteger Exponent { get => _scale; set => _scale = BigInteger.Abs(value); }

		/// <summary>
		/// How many digits can exist after the decimal point (Decimal precision).</br>
		/// If set to <c>0</c>, the precision will be unlimited (Higher precision = more memory usage + slower calculations (Primarly with divide)).
		/// </summary>
		public BigInteger Precision { get => _precision; set => _precision = value < 0 ? 0 : value; }
		#endregion

		#region Constants
		private static readonly byte nan = 0b_0000_0001; // 0x01
		private static readonly byte negnan = 0b_1000_0001;	// 0x81
		private static readonly byte posinf = 0b_0000_0010; // 0x02
		private static readonly byte neginf = 0b_1000_0010; // 0x82

		/// <summary>
		/// Represents the value (<c>0</c>).
		/// </summary>
		public static BigDecimal Zero { get => new BigDecimal(BigInteger.Zero); }

		/// <summary>
		/// Represents the value (<c>1</c>).
		/// </summary>
		public static BigDecimal One { get => new BigDecimal(BigInteger.One); }

		/// <summary>
		/// Represents the value (<c>-1</c>).
		/// </summary>
		public static BigDecimal MinusOne { get => new BigDecimal(BigInteger.MinusOne); }

		/// <summary>
		/// Represents the value (<c>10</c>).
		/// </summary>
		public static BigDecimal Ten { get => new BigDecimal(BigInteger.One * 10); }

		/// <summary>
		/// Represents the value (<c>-10</c>).
		/// </summary>
		public static BigDecimal MinusTen { get => new BigDecimal(BigInteger.MinusOne * 10); }

		/// <summary>
		/// Represents Not a Number (<c>NaN</c>).
		/// </summary>
		public static BigDecimal NaN { get => new BigDecimal() { _flags = nan }; }

		/// <summary>
		/// Represents Not a (negative) Number (<c>-NaN</c>).
		/// </summary>
		public static BigDecimal NegativeNaN { get => new BigDecimal() { _flags = negnan }; }

		/// <summary>
		/// Represents positive infinity (<c>∞</c>).
		/// </summary>
		public static BigDecimal PositiveInfinity { get => new BigDecimal() { _flags = posinf }; }

		/// <summary>
		/// Represents negative infinity (<c>-∞</c>0.
		/// </summary>
		public static BigDecimal NegativeInfinity { get => new BigDecimal() { _flags = neginf }; }

		/// <summary>
		/// Represents the largest possible value (<c>0e+∞</c>).
		/// </summary>
		public static BigDecimal MaxValue { get => throw new OutOfMemoryException(); }

		/// <summary>
		/// Represents the largest possible negative value (<c>0e-∞</c>).
		/// </summary>
		public static BigDecimal MinValue { get => throw new OutOfMemoryException(); }

		/// <summary>
		/// Represents the smallest possible value that is greater than zero of BigDecimal.
		/// </summary>
		public static BigDecimal Epsilon { get => throw new OutOfMemoryException(); }
		#endregion

		#region Return Properti
		public bool IsZero { get { /*Normalize(); return _value.IsZero && _scale.IsZero; } }
		public bool IsOne { get { /*Normalize(); return _value.IsOne && _scale.IsZero; } }
		public bool IsMinusOne { get { /*Normalize(); return _value == -1 && _scale.IsZero; } }
		public int Sign { get => _value.Sign; }
		#endregion

		#region Constructors
		public BigDecimal(int mantissa, int exponent, int precision)
		{
			if (exponent < 0)
				throw new ArgumentOutOfRangeException(nameof(exponent), "Exponent cannot be negative");
			if (precision < 0)
				throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");

			_value = mantissa;
			_scale = exponent;
			_precision = precision;
			_flags = 0;
		}

		public BigDecimal(long mantissa, long exponent, long precision)
		{
			if (exponent < 0)
				throw new ArgumentOutOfRangeException(nameof(exponent), "Exponent cannot be negative");
			if (precision < 0)
				throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");

			Console.WriteLine(exponent);

			_value = mantissa;
			_scale = exponent;
			_precision = precision;
			_flags = 0;
		}

		public BigDecimal(BigInteger mantissa) => this = new BigDecimal(mantissa, 0, 256);

		public BigDecimal(BigInteger mantissa, BigInteger exponent, BigInteger precision)
		{
			if (exponent < 0)
				throw new ArgumentOutOfRangeException(nameof(exponent), "Exponent cannot be negative");
			if (precision < 0)
				throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative");

			_value = mantissa;
			_scale = exponent;
			_precision = precision;
			_flags = 0;
		}

		public BigDecimal(double value) => this = new BigDecimal((decimal)value);

		public BigDecimal(decimal value)
		{
			int[] bits = decimal.GetBits(value);

			// Get the sign of the decimal value (which is stored in the 32nd bit)
			sbyte negative = (sbyte)Math.Sign(bits[3]);

			// Convert the signed integers to unsigned integers
			uint lo = (uint)bits[0];
			uint mid = (uint)bits[1];
			uint hi = (uint)bits[2];

			// BigInteger testVal = hi;
			// Console.WriteLine(testVal);
			// testVal <<= 32;
			// testVal |= mid;
			// Console.WriteLine(testVal);
			// testVal <<= 32;
			// testVal |= lo;
			// Console.WriteLine(testVal);

			// Construct a new BigInteger from the 3 unsigned integers (ooo bitwise magic)
			// BigInteger newValue = ((BigInteger)hi << 32) | mid << 32 | lo;
			Console.WriteLine(newValue);

			_value = negative < 0 ? ~newValue + 1 : newValue;
			_scale = (uint)(bits[3] & 0x00FF0000) >> 16; // Get the scale of the decimal value (which is stored in the 16th to 20th bit)
			_precision = 256;
		}
		#endregion

		#region Flag checking bools
		/// <summary>
		/// Returns a value indicating whether the specified number is not a number (<c>NaN</c>) or not a (negative) number (<c>-NaN</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is not a number (<c>NaN</c>) or not a (negative) number (<c>-NaN</c>); otherwise, <c>false</c>.</returns>
		public static bool IsNaN(BigDecimal value) => value._flags == nan || value._flags == negnan;

		/// <summary>
		/// Returns a value indicating whether the specified number is not a number (<c>NaN</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is not a number (<c>NaN</c>); otherwise, <c>false</c>.</returns>
		public static bool IsPositiveNaN(BigDecimal value) => value._flags == nan;

		/// <summary>
		/// Returns a value indicating whether the specified number is not a (negative) number (<c>-NaN</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is not a (negative) number (<c>-NaN</c>); otherwise, <c>false</c>.</returns>
		public static bool IsNegativeNaN(BigDecimal value) => value._flags == negnan;

		/// <summary>
		/// Returns a value indicating whether the specified number is positive infinity (<c>+∞</c>) or negative infinity (<c>-∞</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is positive infinity (<c>+∞</c>) or negative infinity (<c>-∞</c>); otherwise, <c>false</c>.</returns>
		public static bool IsInfinity(BigDecimal value) => value._flags == posinf || value._flags == neginf;

		/// <summary>
		/// Returns a value indicating whether the specified number is positive infinity (<c>+∞</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is positive infinity (<c>+∞</c>); otherwise, <c>false</c>.</returns>
		public static bool IsPositiveInfinity(BigDecimal value) => value._flags == posinf;

		/// <summary>
		/// Returns a value indicating whether the specified number is negative infinity (<c>-∞</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is negative infinity (<c>-∞</c>); otherwise, <c>false</c>.</returns>
		public static bool IsNegativeInfinity(BigDecimal value) => value._flags == neginf;

		/// <summary>
		/// Returns a bool based on whether the BigDecimal._flags value is negative / 0.<br/>
		/// FINON is the abbreviation for FlagIsNegativeOrNot
		/// </summary>
		/// <returns>False if its anything but negative or 0.</returns>
		private static bool FINON(BigDecimal value) => value._flags == 0x80 || value._flags == 0;
		#endregion

		#region Rescale methods
		public void Normalize()
		{
			if (FINON(this))
				return;
		}

		/// <summary>
		/// Increases the scale of <paramref name="a"/> or <paramref name="b"/> to the highest scale of the two.
		/// </summary>
		public static void AlignScales(ref BigDecimal a, ref BigDecimal b)
		{
			if (FINON(a) || FINON(b))
				return;

			if (a._scale == b._scale)
				return;
			
			if (a._scale < b._scale)
			{
				a._value *= SMath.Pow(10, b._scale - a._scale);
				a._scale = b._scale;
				return;
			}

			b._value *= SMath.Pow(10, a._scale - b._scale);
			b._scale = a._scale;
		}
		#endregion

		#region Public static methods
		// Basic math operations
		/// <summary>
		/// Returns the absolute value the specified number.
		/// </summary>
		public static BigDecimal Abs(BigDecimal value) => new BigDecimal(BigInteger.Abs(value._value), value._scale, value._precision) { _flags = value._flags };

		// Arithmetic methods
		/// <summary>
		/// Returns the sum of two <see cref="System.Numerics.BigDecimal"/> values.
		/// </summary>
		public static BigDecimal Add(BigDecimal left, BigDecimal right)
		{
			if (left._scale == right._scale)
				return new BigDecimal(left._value + right._value, left._scale, left._precision);

			if (left._scale < right._scale)
			{
				left._value *= SMath.Pow(10, right._scale - left._scale);
				return new BigDecimal(left._value + right._value, right._scale, BigInteger.Max(left._precision, right._precision));
			}

			right._value *= SMath.Pow(10, left._scale - right._scale);
			return new BigDecimal(left._value + right._value, left._scale, BigInteger.Max(left._precision, right._precision));
		}

		/// <summary>
		/// Returns the difference between two <see cref="System.Numerics.BigDecimal"/> values.
		/// </summary>
		public static BigDecimal Subtract(BigDecimal left, BigDecimal right)
		{
			if (left._scale == right._scale)
				return new BigDecimal(left._value - right._value, left._scale, left._precision);

			if (left._scale < right._scale)
			{
				left._value *= SMath.Pow(10, right._scale - left._scale);
				return new BigDecimal(left._value - right._value, right._scale, BigInteger.Max(left._precision, right._precision));
			}

			right._value *= SMath.Pow(10, left._scale - right._scale);
			return new BigDecimal(left._value - right._value, left._scale, BigInteger.Max(left._precision, right._precision));
		}

		public static BigDecimal Negate(BigDecimal value)
		{
			return new BigDecimal(-value._value, value._scale, value._precision)
			{
				_flags = (byte)((int)value._flags ^ (int)0x80)
			};
		}
		#endregion

		#region Public instance methods
		public override string ToString() => BigDecimalFormatter.Format(this, null, null);
		public string ToString(string? format, IFormatProvider? provider) => BigDecimalFormatter.Format(this, format, provider);
		#endregion

		#region Operator overloads
		// Implicit conversions (To BigDecimal)
		// Explicit conversions (To BigDecimal)
		// Implicit conversions (From BigDecimal)
		// Explicit conversions (From BigDecimal)
		// Arithmetic operators
		public static BigDecimal operator +(BigDecimal left, BigDecimal right) => Add(left, right);
		public static BigDecimal operator +(BigDecimal value) => value;
		public static BigDecimal operator ++(BigDecimal value) => Add(value, BigDecimal.One);
		public static BigDecimal operator -(BigDecimal left, BigDecimal right) => Subtract(left, right);
		public static BigDecimal operator -(BigDecimal value) => Negate(value);
		public static BigDecimal operator --(BigDecimal value) => Subtract(value, BigDecimal.One);
		// Comparison operators
		// Bitwise operators
		#endregion
	}
}
/*
namespace System.Numerics
{
	/// <summary>
	/// A class that represents a decimal number with an arbitrary precision</br>
	/// Basically a hybrid between BigInteger and Decimal
	/// </summary>
	[Serializable]
	public struct BigDecimal : IComparable, IComparable<BigDecimal>, IComparer<BigDecimal>, IEquatable<BigDecimal>, IFormattable//, ISerializable
	{
		// Contains the base values for BigDecimal
		#region Values
		internal BigInteger _value;

		// Decimal point location
		internal BigInteger _scale;

		// Decimal precision
		internal BigInteger _precision;

		// Extra bits for functionality with NaN, Infinity, etc
		internal byte _flags;

		/// <summary>
		/// How many digits can exist after the decimal point (Decimal precision)</br>
		/// If set to <c>0</c>, the precision will be unlimited (Higher precision = more memory usage + slower calculations).
		/// </summary>
		public BigInteger Precision { get => _precision; set => _precision = value < 0 ? 0 : value; }

		/// <summary>
		/// Controls whether the number will be normalized where possible
		/// </summary>
		public bool AutoNormalize { get; set; }
		#endregion

		#region Constants
		private static readonly BigDecimal zero = new BigDecimal(0, 0, 0);
		private static readonly BigDecimal one = new BigDecimal(1, 0, 0);
		private static readonly BigDecimal minusOne = new BigDecimal(-1, 0, 0);

		private const byte nan = unchecked(0x01);
		private const byte negnan = unchecked(0x81);
		private const byte infinity = unchecked(0x02);
		private const byte neginfinity = unchecked(0x82);
		private const byte absolute = unchecked(0xff);
		#endregion

		#region Public constants
		public static BigDecimal Zero { get => zero; }
		public static BigDecimal One { get => one; }
		public static BigDecimal MinusOne { get => minusOne; }

		public static BigDecimal NaN { get => new BigDecimal(-1, 0, 0) { _flags = nan }; }
		public static BigDecimal NegativeNaN { get => new BigDecimal(-1, 0, 0) { _flags = negnan }; }
		public static BigDecimal PositiveInfinity { get => new BigDecimal(1, 0, 0) { _flags = infinity }; }
		public static BigDecimal NegativeInfinity { get => new BigDecimal(-1, 0, 0) { _flags = neginfinity }; }

		// Doing this cuz it's funy :D
		public static BigDecimal MaxValue { get => throw new OutOfMemoryException(); }
		public static BigDecimal MinValue { get => throw new OutOfMemoryException(); }
		public static BigDecimal Epsilon { get => throw new OutOfMemoryException(); }

		// Dont know what the point of "AbsoluteDelta" is, but I added it so lol
		public static BigDecimal AbsoluteDelta { get => new BigDecimal(1, 0, 0) { _flags = absolute }; }
		#endregion

		#region Constructors
		public BigDecimal(int value) => this = new BigDecimal(value, 0, 256);

		public BigDecimal(int value, int scale, int precision) => this = new BigDecimal((BigInteger)value, scale, precision);

		public BigDecimal(long value, long scale, long precision) => this = new BigDecimal((BigInteger)value, scale, precision);

		public BigDecimal(BigInteger value) => this = new BigDecimal(value, 0, 256);

		public BigDecimal(BigInteger value, BigInteger scale, BigInteger precision)
		{
			_value = value; _scale = scale; _precision = precision; _flags = 0x00;
		}

		public BigDecimal(double value) => this = new BigDecimal((decimal)value);

		public BigDecimal(decimal value)
		{
			int[] bits = decimal.GetBits(value);

			// Get the sign of the decimal value (which is stored in the 32nd bit)
			sbyte negative = (sbyte)Math.Sign(bits[3]);

			// Convert the signed integers to unsigned integers
			uint lo = (uint)bits[0];
			uint mid = (uint)bits[1];
			uint hi = (uint)bits[2];

			// Construct a new BigInteger from the 3 unsigned integers (ooo bitwise magic)
			BigInteger newValue = ((BigInteger)hi << 32) | mid << 32 | lo;

			_value = negative < 0 ? ~newValue + 1 : newValue;
			_scale = (uint)(bits[3] & 0x00FF0000) >> 16; // Get the scale of the decimal value (which is stored in the 16th to 20th bit)
			_precision = 256;
		}
		#endregion

		#region Flag checks
		public static bool IsNaN(BigDecimal value) => value._flags == nan || value._flags == negnan;
		public static bool IsPositiveNaN(BigDecimal value) => value._flags == nan;
		public static bool IsNegativeNaN(BigDecimal value) => value._flags == negnan;
		public static bool IsInfinity(BigDecimal value) => value._flags == infinity || value._flags == neginfinity;
		public static bool IsPositiveInfinity(BigDecimal value) => value._flags == infinity;
		public static bool IsNegativeInfinity(BigDecimal value) => value._flags == neginfinity;
		public static bool DeltaIsAbsolute(BigDecimal value) => value._flags == absolute;
		#endregion

		#region Manipulation
		/// <summary>
		/// Strips pointless zeros from the end of the number. </br>
		/// Does nothing if <see cref="AutoNormalize"/> is <c>false</c>.
		/// </summary>
		public void Normalize(bool ignoreAutoNormalize = false)
		{
			if (!ignoreAutoNormalize && !AutoNormalize) return;

			// NOTE: There are probably better ways to do this, but this is the best I could come up with
			if (_value == 0 || _scale == 0)
			{
				// The value is already normalized
				_scale = 0;
				return;
			}

			BigInteger pow = SMath.Pow(10, _scale);
			BigInteger absValue = BigInteger.Abs(_value);

			if (pow > absValue)
			{
				// This happens when the number is something like 0.0000023
				// Get the number of digits after the number
				BigInteger scaleValue = absValue;

				for (BigInteger i = 1; i < _scale; i++)
				{
					scaleValue /= 10;

					if (scaleValue * SMath.Pow(10, i) != absValue)
					{
						BigInteger newValue = absValue / SMath.Pow(10, i - 1);

						_value = newValue * _value.Sign;
						_scale -= i - 1;
						break;
					}
				}
			}

			// Otherwise, normalize the number like normal

			// Get the number of digits before the decimal point
			long digitsBeforeDecimal = (long)((BigInteger)BigInteger.Log10(absValue) - _scale);
			BigInteger testValue = absValue / pow;

			if ((testValue * pow) == absValue)
			{
				Console.WriteLine("Value is has been normalized");
				_value = testValue;
				_scale = 0;
				return;
			}
		}

		/// <summary>
		/// Make both values share the same scale, the opposite of <see cref="Normalize"/>
		/// </summary>
		public static void MatchScale(ref BigDecimal a, ref BigDecimal b)
		{
			if (a._scale == b._scale)
				return;
			
			if (a._scale > b._scale)
			{
				b._value *= SMath.Pow(10, a._scale - b._scale);
				b._scale = a._scale;
				return;
			}

			a._value *= SMath.Pow(10, b._scale - a._scale);
			a._scale = b._scale;
		}

		

		#endregion
		
		#region Instance methods
		public override bool Equals(object obj)
		{
			if (obj is not BigDecimal other)
				return false;

			return Equals(other);
		}

		public bool Equals(BigDecimal other)
		{
			// Make the scales match so we don't need to compare them
			MatchScale(ref this, ref other);

			// Here precision doesnt need to be checked since it's just a calculation property
			if (_value != other._value)
				return false;

			if (_flags != other._flags)
				return false;

			Normalize(true);
			other.Normalize(true);

			return true;
		}

		public int CompareTo(object obj)
		{
			if (obj is not BigDecimal other)
				throw new ArgumentException("Object is not a BigDecimal", nameof(obj));
			
			return CompareTo(other);
		}

		public int CompareTo(BigDecimal other)
		{
			// Make the scales match so we don't need to compare them
			MatchScale(ref this, ref other);

			if (_value > other._value)
				return 1;

			if (_value < other._value)
				return -1;

			// Not sure how flag comparison should work

			Normalize(true);
			other.Normalize(true);

			// if (_value == other._value || _flags == other._flags)
			return 0;
		}

		public int Compare(BigDecimal x, BigDecimal y) => x.CompareTo(y);

		public override int GetHashCode()
		{
			Normalize(true);
			return _value.GetHashCode() ^ _flags.GetHashCode(); // Just for now until a proper hash function is made
		}

		public override string ToString() => BigDecimalFormatter.Format(this, null, null);

		public string ToString(string? format) => BigDecimalFormatter.Format(this, format, null);

		public string ToString(string? format, IFormatProvider? formatProvider) => BigDecimalFormatter.Format(this, format, formatProvider);
		#endregion

		#region Serialization
		// public void GetObjectData(SerializationInfo info, StreamingContext context)
		#endregion

		#region Parsing methods
		public static BigDecimal Parse(string s) => Parse(s, NumberStyles.Any, null);
		public static BigDecimal Parse(string s, NumberStyles style) => Parse(s, style, null);
		public static BigDecimal Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Any, provider);
		public static BigDecimal Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			if (string.IsNullOrEmpty(s))
				throw new ArgumentNullException(nameof(s));

			return BigDecimalFormatter.Parse(s, style, provider);
		}

		public static bool TryParse(string s, out BigDecimal result) => TryParse(s, NumberStyles.Number, null, out result);
		public static bool TryParse(string s, NumberStyles style, IFormatProvider? provider, out BigDecimal result)
		{
			if (string.IsNullOrEmpty(s))
			{
				result = default;
				return false;
			}

			// return BigDecimalFormatter.TryParse(s, style, provider, out result);
			result = BigDecimal.Zero;
			return false;
		}
		#endregion

		#region Mathimatical methods
		public static BigDecimal Abs(BigDecimal value)
		{
			value.Normalize();
			return new BigDecimal(BigInteger.Abs(value._value), value._scale, value._precision);
		}
		#endregion

		// Arithmetic Operator Methods
		// Contains: +, -, *, / (WIP), --
		#region Arithmetic operator methods
		public static BigDecimal Add(BigDecimal left, BigDecimal right)
		{
			left.Normalize();
			right.Normalize();

			if (left._scale == right._scale)
			{
				Console.WriteLine("Scales are the same");
				return new BigDecimal(left._value + right._value, left._scale, left._precision);
			}
			else if (left._scale < right._scale)
			{
				Console.WriteLine("Left scale is less than right scale");
				left._value *= SMath.Pow(10, (right._scale - left._scale));
				return new BigDecimal(left._value + right._value, right._scale, BigInteger.Max(left._precision, right._precision));
			} // if (left._scale > right._scale)

			Console.WriteLine("Left scale is greater than right scale");
			right._value *= SMath.Pow(10, (left._scale - right._scale));
			return new BigDecimal(left._value + right._value, left._scale, BigInteger.Max(left._precision, right._precision));
		}

		public static BigDecimal Substract(BigDecimal left, BigDecimal right)
		{
			left.Normalize();
			right.Normalize();

			if (left._scale == right._scale)
			{
				Console.WriteLine("Scales are the same");
				return new BigDecimal(left._value - right._value, left._scale, left._precision);
			}
			else if (left._scale < right._scale)
			{
				Console.WriteLine("Left scale is less than right scale");
				left._value *= SMath.Pow(10, (right._scale - left._scale));
				return new BigDecimal(left._value - right._value, right._scale, BigInteger.Max(left._precision, right._precision));
			} // if (left._scale > right._scale)

			Console.WriteLine("Left scale is greater than right scale");
			right._value *= SMath.Pow(10, (left._scale - right._scale));
			return new BigDecimal(left._value - right._value, left._scale, BigInteger.Max(left._precision, right._precision));
		}

		public static BigDecimal Multiply(BigDecimal left, BigDecimal right)
		{
			left.Normalize();
			right.Normalize();

			BigInteger newScale = left._scale + right._scale;
			BigInteger newPrecision = BigInteger.Max(left._precision, right._precision);

			BigInteger unscaledValue = left._value * right._value;
			BigInteger scaledValue = BigInteger.Divide(unscaledValue, SMath.Pow(10, newScale));

			return new BigDecimal(scaledValue, (int)newScale, newPrecision);

			// return left;
		}

		public static BigDecimal Divide(BigDecimal dividend, BigDecimal divisor)
		{
			// TODO: Properly implement BigDecimal division
			// im not big brain enough to do this rn
			dividend.Normalize();
			divisor.Normalize();

			if (dividend._precision == 0 || divisor._precision == 0)
				throw new Exception("Precision cannot be zero for BigDecimal division");

			if (dividend._value == 0 && divisor._value == 0)
				return new BigDecimal(-1, 0, dividend._precision) { _flags = nan };

			if (dividend._value == -0 && divisor._value == -0)
				return new BigDecimal(1, 0, dividend._precision) { _flags = negnan };

			if (dividend._value > 0 && divisor._value == 0)
				return new BigDecimal(1, 0, dividend._precision) { _flags = infinity };
			
			if (dividend._value < 0 && divisor._value == 0)
				return new BigDecimal(-1, 0, dividend._precision) { _flags = neginfinity };

			// Compare the flags to see if they are 0b_1000_0011 (any of these bits)
			if (dividend._flags != 0 && divisor._flags != 0)
				return new BigDecimal(0, 0, dividend._precision) { _flags = absolute };

			if (dividend._value == 0)
				return new BigDecimal(0, 0, dividend._precision);

			return dividend;
		}

		public static BigDecimal Mod(BigDecimal dividend, BigDecimal divisor)
		{
			// TODO: Properly implement BigDecimal modulo
			dividend.Normalize();
			divisor.Normalize();

			return new BigDecimal(dividend._value % divisor._value, dividend._scale, dividend._precision);
		}

		public static BigDecimal Negate(BigDecimal value)
		{
			value.Normalize();
			return new BigDecimal(-value._value, value._scale, value._precision)
			{
				_flags = (byte)((int)value._flags ^ (int)0x80)
			};
		}
		#endregion

		#region Operator overloads
		// Implicit conversions (To BigDecimal)
		public static implicit operator BigDecimal(sbyte value)
		{
			return new BigDecimal(value);
		}

		public static implicit operator BigDecimal(byte value)
		{
			return new BigDecimal(value);
		}

		public static implicit operator BigDecimal(short value)
		{
			return new BigDecimal(value);
		}

		public static implicit operator BigDecimal(ushort value)
		{
			return new BigDecimal(value);
		}

		public static implicit operator BigDecimal(int value)
		{
			return new BigDecimal(value);
		}

		public static implicit operator BigDecimal(uint value)
		{
			return new BigDecimal((long)value, 0, 0);
		}

		public static implicit operator BigDecimal(long value)
		{
			return new BigDecimal(value, 0, 0);
		}

		public static implicit operator BigDecimal(ulong value)
		{
			return new BigDecimal((BigInteger)value, 0, 0);
		}

		public static implicit operator BigDecimal(BigInteger value)
		{
			return new BigDecimal(value, 0, 0);
		}

		public static implicit operator BigDecimal(decimal value)
		{
			return new BigDecimal(value);
		}

		// Explicit conversions (To BigDecimal)
		public static explicit operator BigDecimal(float value)
		{
			if (float.IsNaN(value)) return BigDecimal.NaN;
			if (float.IsPositiveInfinity(value)) return BigDecimal.PositiveInfinity;
			if (float.IsNegativeInfinity(value)) return BigDecimal.NegativeInfinity;
			return new BigDecimal((double)value);
		}

		public static explicit operator BigDecimal(double value)
		{
			if (double.IsNaN(value)) return BigDecimal.NaN;
			if (double.IsPositiveInfinity(value)) return BigDecimal.PositiveInfinity;
			if (double.IsNegativeInfinity(value)) return BigDecimal.NegativeInfinity;
			return new BigDecimal(value);
		}

		public static BigDecimal operator +(BigDecimal left, BigDecimal right)
		{
			return Add(left, right);
		}

		public static BigDecimal operator +(BigDecimal value)
		{
			return value;
		}

		public static BigDecimal operator ++(BigDecimal value)
		{
			return Add(value, one);
		}

		public static BigDecimal operator -(BigDecimal left, BigDecimal right)
		{
			return Substract(left, right);
		}

		public static BigDecimal operator --(BigDecimal value)
		{
			return Substract(value, one);
		}

		// public static BigDecimal operator *(BigDecimal left, BigDecimal right)
		// {
		// 	return Multiply(left, right);
		// }

		// public static BigDecimal operator /(BigDecimal left, BigDecimal right)
		// {
		// 	return Divide(left, right);
		// }

		// public static BigDecimal operator %(BigDecimal left, BigDecimal right)
		// {
		// 	return Mod(left, right);
		// }

		public static BigDecimal operator -(BigDecimal value)
		{
			return Negate(value);
		}

		public static bool operator <(BigDecimal left, BigDecimal right)
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator <=(BigDecimal left, BigDecimal right)
		{
			return left.CompareTo(right) <= 0;
		}

		public static bool operator >(BigDecimal left, BigDecimal right)
		{
			return left.CompareTo(right) > 0;
		}

		public static bool operator >=(BigDecimal left, BigDecimal right)
		{
			return left.CompareTo(right) >= 0;
		}

		public static bool operator ==(BigDecimal left, BigDecimal right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BigDecimal left, BigDecimal right)
		{
			return !left.Equals(right);
		}
		#endregion
	}
}*/