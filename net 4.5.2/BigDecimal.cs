using SSUnlimited.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace System.Numerics
{
	/// <summary>
	/// Represents a decimal number with an arbitrary precision.</br>
	/// Basically a hyprid between BigInteger and Decimal.
	/// </summary>
	public struct BigDecimal : IComparable, IComparable<BigDecimal>, IEquatable<BigDecimal>, IFormattable
	{
		#region Internal values
		// All these are needed for BigDecimal to function properly.
		internal BigInteger _value; // The true unscaled value (is the Mantissa).
		internal BigInteger _scale; // Where the decimal point is.
		internal BigInteger _precision; // How many digits are allowed (maximum) over the decimal (mainly used for recursive operations like Divide).
		private byte _flags; // NaN, Inf, -Inf... thats all you need to know.
		#endregion

		#region Properties
		/// <summary>
		/// The unscaled value of the BigDecimal.
		/// </summary>
		public BigInteger Mantissa { get => _value; set  { _value = value; _flags = 0; } }
		
		/// <summary>
		/// The decimal point location of the BigDecimal.
		/// </summary>
		public BigInteger Exponent
		{
			get => _scale;
			set
			{
				if (IsNaNOrInfinity(this))
					throw new InvalidOperationException("Cannot set the exponent of NaN or Infinity");

				if (value < 0)
					throw new ArgumentOutOfRangeException("Exponent cannot be negative");
				
				_scale = value;
			}
		}
		
		/// <summary>
		/// How many digits can exist after the decimal point (Decimal precision).</br>
		/// If set to <c>0</c>, the precision will be unlimited (Higher precision = more memory usage + slower calculations (Primarly with divide)).
		/// </summary>
		public BigInteger Precision { get => _precision; set { if (value < 0 ) throw new ArgumentOutOfRangeException("Precision cannot be negative"); _precision = value; } }

		/// <summary>
		/// Returns the length of the BigDecimal Mantissa (how many digits are in the Mantissa).
		/// </summary>
		public BigInteger MantissaLength
		{
			get
			{ 
				if (IsNaNOrInfinity(this))
					return 0;
				
				BigInteger len = BigInteger.Zero;

				for (BigInteger i = _value; i > 0; i /= 10)
					len++;
				
				return len;
			}
		}

		/// <summary>
		/// Returns the length of the Whole Number part of BigDecimal (how many digits are in the Whole Number).
		/// </summary>
		public BigInteger Length { get => MantissaLength - _scale; }
		#endregion

		#region Constants
		private static readonly byte nan = 0b_0000_0001; // 0x01, NaN flag.
		private static readonly byte inf = 0b_0000_0010; // 0x02, Positive Infinity flag.

		// No need to define negative consts since the negative variants are calculated using _value.Sign.
		// private static readonly byte neginf = -inf; // 0x82/-0x02, Negative Infinity flag.

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
		public static BigDecimal NaN { get => new BigDecimal(BigInteger.One) { _flags = nan }; }

		/// <summary>
		/// Represents Positive Infinity (<c>∞</c>).
		/// </summary>
		public static BigDecimal PositiveInfinity { get => new BigDecimal(BigInteger.One) { _flags = inf }; }

		/// <summary>
		/// Represents Negative Infinity (<c>-∞</c>).
		/// </summary>
		public static BigDecimal NegativeInfinity { get => new BigDecimal(BigInteger.MinusOne) { _flags = inf }; }

		/// <summary>
		/// Represents the Theoretical Max Value (that the device can handle).
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		// TODO: Actually implement this.
		// Should make a BigDecimal value that basically takes up all the memory as a Theoretical Maximum.
		public static BigDecimal TheoreticalMaxValue { get => throw new NotImplementedException(); }

		/// <summary>
		/// Represents the Theoretical Min Value (that the device can handle).
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		// Should make a BigDecimal value that basically takes up all the memory as a Theoretical Minimum.
		public static BigDecimal TheoreticalMinValue { get => throw new NotImplementedException(); }

		/// <summary>
		/// Represents the smallest possible Theoretical value that is greater than zero (<c>1.0E+∞</c>).
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		// Should make a BigDecimal value that basically takes up all the memory as a Theoretical Epsilon.
		public static BigDecimal TheoreticalEpsilon { get => throw new NotImplementedException(); }

		#region Useless constants (for now)
		// Serves no point other than..... its funy.

		/// <summary>
		/// Represents the largest possible value (<c>0e+∞</c>).
		/// </summary>
		/// <exception cref="System.OutOfMemoryException"></exception>
		public static BigDecimal MaxValue { get => throw new OutOfMemoryException(); }

		/// <summary>
		/// Represents the smallest possible negative value (<c>0e-∞</c>).
		/// </summary>
		/// <exception cref="System.OutOfMemoryException"></exception>
		public static BigDecimal MinValue { get => throw new OutOfMemoryException(); }

		/// <summary>
		/// Represents the smallest possible value that is greater than zero (<c>1.0E+∞</c>).
		/// </summary>
		/// <exception cref="System.OutOfMemoryException"></exception>
		public static BigDecimal Epsilon { get => throw new OutOfMemoryException(); }

		/// <summary>
		/// Returns a BigDecimal with the Mantissa set to <c>0</c> but with MAXIMUM precision.
		/// </summary>
		/// <exception cref="System.OutOfMemoryException"></exception>
		public static BigDecimal MaxPrecision { get => throw new OutOfMemoryException(); }
		#endregion // "Pointless" consts.
		#endregion

		#region Return properties
		/// <summary>
		/// Indicates whether the value of the current BigDecimal is an even number.
		/// </summary>
		/// <returns><c>true</c> if the value of the BigDecimal is even.</br>otherwise, <c>false</c>.
		public bool IsEven { get => _value.IsEven && !IsNaNOrInfinity(this); }

		/// <summary>
		/// Indicates whether the value of the current BigDecimal is -1
		/// </summary>
		/// <returns><c>true</c> if the value of the BigDecimal is -1.</br>otherwise, <c>false</c>.
		public bool IsMinusOne { get => _value == -1 && _scale.IsZero && !IsNaNOrInfinity(this); }

		/// <summary>
		/// Indicates whether the value of the current BigDecimal is 1
		/// </summary>
		/// <returns><c>true</c> if the value of the BigDecimal is -1.</br>otherwise, <c>false</c>.
		public bool IsOne { get => _value.IsOne && _scale.IsZero  && !IsNaNOrInfinity(this); }

		/// <summary>
		/// Indicates whether the value of the current BigDecimal is a power of two.
		/// </summary>
		/// <returns><c>true</c> if the value of the BigDecimal is a power of two.</br>otherwise, <c>false</c>.
		public bool IsPowerOfTwo { get => _value.IsPowerOfTwo && !IsNaNOrInfinity(this); }
		
		/// <summary>
		/// Indicates whether the value of the current Bigdecimal is 0
		/// </summary>
		/// <returns><c>true</c> if the value of the BigDecimal is 0.</br>otherwise, <c>false</c>.
		public bool IsZero { get => _value.IsZero && !IsNaNOrInfinity(this); }

		/// <summary>
		/// Gets a number that indicates the sign (negative, positive, or zero) of the current BigDecimal Mantissa.
		/// </summary>
		/// <returns>A number that indicates the sign of the BigDecimal Mantissa.</br>1 if the Mantissa is positive.</br>-1 if the Mantissa is negative.</br>0 is the mantissa is zero.</returns>
		public int Sign { get => _value.Sign; }
		#endregion

		#region Constructors
		public BigDecimal(byte[] mantissa, byte[] exponent, byte[] precision)
		{
			if (mantissa.Length == 0 || mantissa.LongLength == 0)
				throw new ArgumentException("Mantissa array length cannot be 0.", nameof(mantissa));
			if (exponent.Length == 0 || exponent.LongLength == 0)
				throw new ArgumentException("Exponent array length cannot be 0.", nameof(exponent));
			if (precision.Length == 0 || precision.LongLength == 0)
				throw new ArgumentException("Precision array length cannot be 0.", nameof(precision));

			_value = new BigInteger(mantissa);
			_scale = new BigInteger(exponent);
			_precision = new BigInteger(precision);
			_flags = 0;
		}

		public BigDecimal(int mantissa) => this = new BigDecimal(mantissa, 0, 256);

		public BigDecimal(int mantissa, int exponent) => this = new BigDecimal(mantissa, exponent, 256);

		public BigDecimal(int mantissa, int exponent, int precision)
		{
			if (exponent < 0)
				throw new ArgumentOutOfRangeException(nameof(exponent), "Exponent cannot be negative.");
			if (precision < 0)
				throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative.");

			_value = mantissa;
			_scale = exponent;
			_precision = precision;
			_flags = 0;
		}

		public BigDecimal(long mantissa) => this = new BigDecimal(mantissa, 0, 256);

		public BigDecimal(long mantissa, long exponent) => this = new BigDecimal(mantissa, exponent, 256);

		public BigDecimal(long mantissa, long exponent, long precision)
		{
			if (exponent < 0)
				throw new ArgumentOutOfRangeException(nameof(exponent), "Exponent cannot be negative.");
			if (precision < 0)
				throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative.");

			_value = mantissa;
			_scale = exponent;
			_precision = precision;
			_flags = 0;
		}

		public BigDecimal(BigInteger mantissa) => this = new BigDecimal(mantissa, 0, 256);

		public BigDecimal(BigInteger mantissa, BigInteger exponent) => this = new BigDecimal(mantissa, exponent, 256);

		public BigDecimal(BigInteger mantissa, BigInteger exponent, BigInteger precision)
		{
			if (exponent < 0)
				throw new ArgumentOutOfRangeException(nameof(exponent), "Exponent cannot be negative.");
			if (precision < 0)
				throw new ArgumentOutOfRangeException(nameof(precision), "Precision cannot be negative.");

			_value = mantissa;
			_scale = exponent;
			_precision = precision;
			_flags = 0;
		}

		public BigDecimal(double value)
		{
			if (double.IsNaN(value))
			{
				_value = 1; _scale = 0; _precision = 256; _flags = nan; return;
			}

			if (double.IsNegativeInfinity(value))
			{
				_value = -1; _scale = 0; _precision = 256; _flags = inf; return;
			}

			if (double.IsPositiveInfinity(value))
			{
				_value = 1; _scale = 0; _precision = 256; _flags = inf; return;
			}

			// TODO: Figure out how convert the large double scales

			this = new BigDecimal((decimal)value);
		}

		public BigDecimal(decimal value)
		{
			int[] bits = decimal.GetBits(value);

			// Get the sign bit of the decimal value (which is stored in the 32nd bit of the flags).
			sbyte sign = (sbyte)Math.Sign(bits[3]);

			// Convert the signed integers into unsigned integers.
			uint lo = (uint)bits[0];
			uint mid = (uint)bits[1];
			uint hi = (uint)bits[2];

			// Contruct a new BigInteger from the 3 unsigned integers we got before (oooooo bitwise magic).
			BigInteger mantissa = (BigInteger)lo | ((BigInteger)mid << 32) | ((BigInteger)hi << 64);

			_value = sign < 0 ? ~mantissa + 1 : mantissa;
			_scale = (uint)(bits[3] & 0x00FF0000) >> 16; // Get the scale of the decimal value (which is stored in the 16th to 20th bit (around that range)).
			_precision = 256;
			_flags = 0;
		}
		#endregion

		#region Flag checking
		/// <summary>
		/// Returns a value indicating whether the specified number is Not a Number (<c>NaN</c>), Positive Infinity (<c>+∞</c>) or Negative Infinity (<c>-∞</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is Not a Number (<c>NaN</c>), Positive Infinity (<c>+∞</c>) or Negative Infinity (<c>-∞</c>); otherwise, <c>false</c>.</returns>
		public static bool IsNaNOrInfinity(BigDecimal value) => value._flags != 0;

		/// <summary>
		/// Returns a value indicating whether the specified number is Not a Number (<c>NaN</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is Not a Number (<c>NaN</c>).</br>otherwise, <c>false</c>.</returns>
		public static bool IsNaN(BigDecimal value) => value._flags == nan;

		/// <summary>
		/// Returns a value indicating whether the specified number is Positive Infinity (<c>+∞</c>) or Negative Infinity (<c>-∞</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is Positive Infinity (<c>+∞</c>) or Negative Infinity (<c>-∞</c>); otherwise, <c>false</c>.</returns>
		public static bool IsInfinity(BigDecimal value) => value._flags == inf;

		/// <summary>
		/// Returns a value indicating whether the specified number is Positive Infinity (<c>+∞</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is Positive Infinity (<c>+∞</c>); otherwise, <c>false</c>.</returns>
		public static bool IsPositiveInfinity(BigDecimal value) => (value._value.Sign >= 0) && value._flags == inf;

		/// <summary>
		/// Returns a value indicating whether the specified number is Negative Infinity (<c>-∞</c>).
		/// </summary>
		/// <returns><c>true</c> if <paramref name="value"/> is Negative Infinity (<c>-∞</c>); otherwise, <c>false</c>.</returns>
		public static bool IsNegativeInfinity(BigDecimal value) => value._value.Sign <= -1 && value._flags == inf;
		#endregion

		#region Value manipulation
		/// <summary>
		/// Strips pointless zeros from the end of the number.</br>
		/// Provided the "pointless" zeros are in the fractional part of the number.
		/// </summary>
		internal void Normalize()
		{
			BigInteger div = BigInteger.DivRem(BigInteger.Abs(_value), SMath.Pow(10, _scale), out BigInteger fraction);
		
			if (_value.IsZero || _scale.IsZero || IsNaNOrInfinity(this))
			{
				// Value is zero or has no fractional part.
				_scale = 0;
				return;
			}

			if (fraction.IsZero && _scale > 0)
			{
				// Value has no fractional part.
				_scale = 0;
				_value = div * _value.Sign;
				return;
			}

			// Check if the fractional part is already normalized.
			if (fraction % 10 != 0)
			{
				Console.WriteLine("Fractional part is already normalized.");
				// Fractional part is already normalized.
				return;
			}

			// Lets create a simple variable for testing how much the fraction needs to be divided by.
			BigInteger testingValue = fraction;

			// Lets do the process of dividing the testingValue by 10 ^ i then multiplying by 10 ^ i until the value is missing a digit
			// Then thats when we know, we found our "normalized" mantissa.
			for (BigInteger i = 1; i < _scale; i++)
			{
				testingValue /= 10;

				if (testingValue * SMath.Pow(10, i) != fraction)
				{
					// We've found the point where the testingValue lost a digit
					BigInteger newValue = fraction / SMath.Pow(10, i - 1);

					if (div.IsZero)
					{
						// Whole value of the Mantissa doesnt exist.
						_value = newValue * _value.Sign;
						_scale = _scale - i + 1;
						return;
					}
					else
					{
						// Whole value of the Mantissa does exist.
						_value = div * SMath.Pow(10, _scale - i + 1) + newValue * _value.Sign;
						_scale = _scale - i + 1;
						return;
					}
				}
			}

		}

		/// <summary>
		/// Makes both values have the same scale.
		/// </summary>
		internal static void MatchScale(ref BigDecimal a, ref BigDecimal b)
		{
			if (IsNaNOrInfinity(a) || IsNaNOrInfinity(b))
				return;

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
		public int CompareTo(object obj)
		{
			if (obj is not BigDecimal other)
				throw new ArgumentException("Object must be of type BigDecimal", nameof(obj));
			
			return CompareTo(other);
		}

		public int CompareTo(BigDecimal other)
		{
			// Match scales now.
			BigDecimal[] values = new BigDecimal[] { this, other };
			MatchScale(ref values[0], ref values[1]);

			// Lets check the flags first.
			if (IsNaN(this) || IsNaN(other))
				return 0;

			if (IsInfinity(this) && IsInfinity(other))
			{
				// Our Infinity is bigger than the other Infinity.
				if (_value.Sign >= 0 && other._value.Sign < 0)
					return 1;
				// Our Infinity is smaller than the other Infinity.
				else if (_value.Sign < 0 && other._value.Sign >= 0)
					return -1;
				// Both Infinities are the same.
				else 
					return 0;
			}

			if (IsInfinity(this) && !IsInfinity(other))
				return _value.Sign;

			if (!IsInfinity(this) && IsInfinity(other))
				return -other._value.Sign;

			// Compare the values
			if (_value > other._value)
				return 1;

			if (_value < other._value)
				return -1;

			return 0;

			// TODO: add support for flags
		}

		public override bool Equals(object? obj)
		{
			if (obj is not BigDecimal other)
				return false;

			return Equals(other);
		}

		public bool Equals(BigDecimal other)
		{
			// Check flags first.
			if (IsNaN(this) || IsNaN(other))
				return false;
			
			if (IsInfinity(this) && IsInfinity(other))
			{
				// Our Infinity is bigger than the other Infinity.
				if (_value.Sign >= 0 && other._value.Sign < 0)
					return false;
				// Our Infinity is smaller than the other Infinity.
				else if (_value.Sign < 0 && other._value.Sign >= 0)
					return false;
				// Both Infinities are the same.
				else 
					return true;
			}

			if ((IsInfinity(this) && !IsInfinity(other)) || (!IsInfinity(this) && IsInfinity(other)))
				return false;

			// Match scales now.
			BigDecimal[] values = new BigDecimal[] { this, other };
			MatchScale(ref values[0], ref values[1]);

			return values[0]._value == values[1]._value;
		}

		public override int GetHashCode()
		{
			if (IsNaN(this))
				return _flags ^ unchecked(0x7FFF0004);

			if (IsInfinity(this))
				return _flags ^ (Sign == -1 ? -83164 : -935726);

			return _value.GetHashCode() ^ _scale.GetHashCode(); 

			//(((h1 << 5) + h1) ^ h2);
		}

		public override string ToString() => BigDecimalFormatter.Format(this, null, null);

		public string ToString(string? format) => BigDecimalFormatter.Format(this, format, null);

		public string ToString(string? format, IFormatProvider? provider) => BigDecimalFormatter.Format(this, format, provider);
		#endregion

		#region Mathimatical methods
		public static BigDecimal Round(BigDecimal value, BigInteger digits, MidpointRounding mode)
		{
			if (digits < 0)
				throw new ArgumentOutOfRangeException(nameof(digits), "Value must be greater than or equal to 0");

			if (IsNaNOrInfinity(value) || value.Equals(Zero) || value._scale.IsZero)
				return value;

			BigInteger whole = BigInteger.DivRem(BigInteger.Abs(value._value), SMath.Pow(10, value._scale), out BigInteger fraction);

			if (fraction == 0)
				return value;

			if (value._scale < digits)
				return value;

			if (digits.IsZero)
				{
					switch (mode)
					{
						case MidpointRounding.ToEven:
							if (fraction >= (SMath.Pow(10, value._scale - 1) * 6))
								return new BigDecimal((whole + 1) * value._value.Sign, 0, value._precision);
							return new BigDecimal(whole * value._value.Sign, 0, value._precision);

						case MidpointRounding.AwayFromZero:
							if (fraction >= (SMath.Pow(10, value._scale - 1) * 5))
								return new BigDecimal((whole + 1) * value._value.Sign, 0, value._precision);
							return new BigDecimal(whole * value._value.Sign, 0, value._precision);
					}
				}
			
			

			return BigDecimal.Zero;
		}

		public static BigDecimal Truncate(BigDecimal value) => Truncate(value, 0);

		/// <summary>
		/// Truncates the BigDecimal by its precision.
		/// </summary>
		internal void TruncatePrecision()
		{
			this = Truncate(this, _precision);
		}

		public static BigDecimal Truncate(BigDecimal value, BigInteger digits)
		{
			if (digits < 0)
				throw new ArgumentOutOfRangeException(nameof(digits), "Value must be greater than or equal to 0");

			if (IsNaNOrInfinity(value) || value.Equals(Zero) || value._scale.IsZero)
				return value;

			BigInteger whole = BigInteger.DivRem(BigInteger.Abs(value._value), SMath.Pow(10, value._scale), out BigInteger fraction);

			if (fraction == 0)
				return value;

			if (digits == 0)
				return new BigDecimal(whole * value._value.Sign, 0, value._precision);

			BigInteger reScale = value._scale;

			if (value._precision > 0)
				while (reScale > value._precision)
				{
					fraction /= 10;
					reScale--;
				}

			while (reScale > digits)
			{
				fraction /= 10;
				reScale--;
			}

			return new BigDecimal((whole * SMath.Pow(10, reScale) + fraction) * value._value.Sign, reScale, value._precision);
		}
		#endregion

		#region Arithmetic methods
		public static BigDecimal Add(BigDecimal left, BigDecimal right)
		{
			if (IsNaNOrInfinity(left) && IsNaNOrInfinity(right))
				return new BigDecimal(1, 0, BigInteger.Max(left._precision, right._precision)) { _flags = nan };

			if (IsNaNOrInfinity(left) && !IsNaNOrInfinity(right))
				return new BigDecimal(left._value, left._scale, BigInteger.Max(left._precision, right._precision)) { _flags = left._flags };

			if (!IsNaNOrInfinity(left) && IsNaNOrInfinity(right))
				return new BigDecimal(right._value, right._scale, BigInteger.Max(left._precision, right._precision)) { _flags = right._flags };
			
			MatchScale(ref left, ref right);

			BigDecimal result = new BigDecimal(left._value + right._value, left._scale, BigInteger.Max(left._precision, right._precision));

			result.Normalize();

			return result;
		}

		public static BigDecimal Subtract(BigDecimal left, BigDecimal right)
		{
			if (IsNaNOrInfinity(left) && IsNaNOrInfinity(right))
				return new BigDecimal(1, 0, BigInteger.Max(left._precision, right._precision)) { _flags = nan };
			
			if (IsNaNOrInfinity(left) && !IsNaNOrInfinity(right))
				return new BigDecimal(left._value, left._scale, BigInteger.Max(left._precision, right._precision)) { _flags = left._flags };
			
			if (!IsNaNOrInfinity(left) && IsNaNOrInfinity(right))
				return new BigDecimal(right._value, right._scale, BigInteger.Max(left._precision, right._precision)) { _flags = right._flags };

			MatchScale(ref left, ref right);

			BigDecimal result = new BigDecimal(left._value - right._value, left._scale, BigInteger.Max(left._precision, right._precision));

			result.Normalize();

			return result;
		}

		public static BigDecimal Multiply(BigDecimal left, BigDecimal right)
		{
			if (IsNaNOrInfinity(left) && IsNaNOrInfinity(right))
				return new BigDecimal(1, 0, BigInteger.Max(left._precision, right._precision)) { _flags = nan };

			if (IsNaNOrInfinity(left) && !IsNaNOrInfinity(right))
				return new BigDecimal(left._value, left._scale, BigInteger.Max(left._precision, right._precision)) { _flags = left._flags };
				
			if (!IsNaNOrInfinity(left) && IsNaNOrInfinity(right))
				return new BigDecimal(right._value, right._scale, BigInteger.Max(left._precision, right._precision)) { _flags = right._flags };

			BigDecimal result = new BigDecimal(left._value * right._value, left._scale + right._scale, BigInteger.Max(left._precision, right._precision));

			result.Normalize();

			return result;
		}

		public static BigDecimal Divide(BigDecimal dividend, BigDecimal divisor)
		{
			// Start of checks that checks if the stuff is good

			if (dividend._precision.IsZero)
				throw new ZeroPrecisionException();

			if ((IsPositiveInfinity(dividend) && IsPositiveInfinity(divisor)) || (IsNegativeInfinity(dividend) && IsNegativeInfinity(divisor)))
				return new BigDecimal(1, 0, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = inf };

			if ((IsNegativeInfinity(dividend) && IsPositiveInfinity(divisor)) || (IsPositiveInfinity(dividend) && IsNegativeInfinity(divisor)))
				return new BigDecimal(-1, 0, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = inf };

			if (IsNaN(dividend) || IsNaN(divisor))
				return new BigDecimal(1, 0, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = nan };

			if (IsInfinity(dividend) && !IsInfinity(divisor))
				return new BigDecimal(dividend._value, dividend._scale, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = dividend._flags };

			if (!IsInfinity(dividend) && IsInfinity(divisor))
				return new BigDecimal(divisor._value, divisor._scale, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = divisor._flags };

			BigDecimal[] values = new BigDecimal[] { dividend, divisor };
			values[0].Normalize();
			values[1].Normalize();

			if (values[0]._value.IsZero && values[1]._value.IsZero)
				return new BigDecimal(1, 0, values[0]._precision) { _flags = nan };
			
			if (values[0]._value.Sign >= 0 && values[1]._value.IsZero)
				return new BigDecimal(1, 0, values[0]._precision) { _flags = inf };

			if (values[0]._value.Sign < 0 && values[1]._value.IsZero)
				return new BigDecimal(-1, 0, values[0]._precision) { _flags = inf };

			// End of checks that checks if stuff is good

			dividend.Normalize();
			divisor.Normalize();

			// -- Divide operation loop --
			// (Mostly written by https://github.com/KtheVeg)

			bool resultNegative = dividend._value.Sign != divisor._value.Sign;
			// Division follows a simple rule: if the signs are different, the result is negative. 
			// The operations are carried out otherwise in the same way

			dividend._value = BigInteger.Abs(dividend._value);
			divisor._value = BigInteger.Abs(divisor._value);
			// Now that we have the proper signs, we can work with the absolute values of the
			// numbers. Since the operations will be the exact same, we can just work with the
			// numbers as if they're positive.

			// Math is fun. precalulus courses are fun...

			BigDecimal output = BigDecimal.Zero;
			output._precision = dividend._precision; // :)

			Console.WriteLine("Function Executed!");
			Console.WriteLine(dividend);
			Console.WriteLine(divisor);

			while (BigDecimal.Subtract(dividend, divisor) >= BigDecimal.Zero) 
			{
				Console.WriteLine("Did an int-pass loop");
				dividend = BigDecimal.Subtract(dividend, divisor);
				output._value++;
				
			}

			// When the dividend is less than the divisor, we have the integer part of the result.

			// -- Decimal Calculation loop --
			// Here, the decimal will be calculated by multiplying the dividend mantissa by 10,
			// adding 1 to scale, and then subtracting the divisor from the dividend until the
			// needed precision is reached. If we somehow go over, we'll use the truncate method
			// to get the number in the right boundry.

			
			BigInteger repeatCount = 0;
			while (output._scale < output._precision)
			{
				repeatCount++;
				Console.WriteLine($"---- ITERATION {repeatCount} ----");
				Console.WriteLine("Did an decimal-pass loop");
				BigInteger timesSubtractedThisRun = 0; // This will be added on-top of the current mantissa, as scale increases
				
				// Expand the mantissa (and scale) until we have enough room to fit the divisor
				for (BigInteger i=0; i < divisor.MantissaLength; i++)
				{
					Console.WriteLine("Did For-Loop Pass");
					dividend = dividend * BigDecimal.Ten;
					output._scale++;
					output._value *= 10;
				}
				Console.WriteLine("1: " + dividend);
				while ((dividend - divisor) >= BigDecimal.Zero) 
				{
					Console.WriteLine("Did Second int-Sub pass");
					dividend = dividend - divisor;
					timesSubtractedThisRun++;
				}
				
				Console.WriteLine("2: " + dividend);
				output._value += timesSubtractedThisRun;
				Console.WriteLine("3: " + output);
				if (dividend == BigDecimal.Zero) break;
			}
			
			

			// Truncate the output to fit within the precision limitations
			output.TruncatePrecision();

			if (resultNegative) output = new BigDecimal(0, 0, output._precision) - output;

			return output;
			// return new BigDecimal(dividend._value, dividend._scale, dividend._precision);
		}

		public static BigDecimal Modulo(BigDecimal dividend, BigDecimal divisor)
		{
			// Start of checks that checks if the stuff is good

			if (dividend._precision.IsZero)
				throw new ZeroPrecisionException();

			if ((IsPositiveInfinity(dividend) && IsPositiveInfinity(divisor)) || (IsNegativeInfinity(dividend) && IsNegativeInfinity(divisor)))
				return new BigDecimal(1, 0, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = inf };

			if ((IsNegativeInfinity(dividend) && IsPositiveInfinity(divisor)) || (IsPositiveInfinity(dividend) && IsNegativeInfinity(divisor)))
				return new BigDecimal(-1, 0, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = inf };

			if (IsNaN(dividend) || IsNaN(divisor))
				return new BigDecimal(1, 0, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = nan };

			if (IsInfinity(dividend) && !IsInfinity(divisor))
				return new BigDecimal(dividend._value, dividend._scale, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = dividend._flags };

			if (!IsInfinity(dividend) && IsInfinity(divisor))
				return new BigDecimal(divisor._value, divisor._scale, BigInteger.Max(dividend._precision, divisor._precision)) { _flags = divisor._flags };

			BigDecimal[] values = new BigDecimal[] { dividend, divisor };
			values[0].Normalize();
			values[1].Normalize();

			if (values[0]._value.IsZero && values[1]._value.IsZero)
				return new BigDecimal(1, 0, values[0]._precision) { _flags = nan };
			
			if (values[0]._value.Sign >= 0 && values[1]._value.IsZero)
				return new BigDecimal(1, 0, values[0]._precision) { _flags = inf };

			if (values[0]._value.Sign < 0 && values[1]._value.IsZero)
				return new BigDecimal(-1, 0, values[0]._precision) { _flags = inf };

			// End of checks that checks if stuff is good

			bool negativeOutput = dividend < BigDecimal.Zero;

			dividend._value = BigInteger.Abs(dividend._value);
			divisor._value = BigInteger.Abs(divisor._value);

			while (BigDecimal.Subtract(dividend, divisor) >= BigDecimal.Zero) 
			{
				Console.WriteLine("Did an int-pass loop");
				dividend = BigDecimal.Subtract(dividend, divisor);
			}

			if (negativeOutput) dividend = new BigDecimal(0, 0, dividend._precision) - dividend;

			return dividend;
		}

		public static BigDecimal Negate(BigDecimal value) => new BigDecimal(-value._value, value._scale, value._precision) { _flags = value._flags };
		#endregion

		#region Operator overloads
		// Arithmetic
		public static BigDecimal operator +(BigDecimal left, BigDecimal right) => Add(left, right);
		public static BigDecimal operator ++(BigDecimal value) => Add(value, One);
		public static BigDecimal operator -(BigDecimal left, BigDecimal right) => Subtract(left, right);
		public static BigDecimal operator --(BigDecimal value) => Subtract(value, One);
		public static BigDecimal operator *(BigDecimal left, BigDecimal right) => Multiply(left, right);
		public static BigDecimal operator /(BigDecimal left, BigDecimal right) => Divide(left, right);
		public static BigDecimal operator %(BigDecimal left, BigDecimal right) => Modulo(left, right);
		
		// Comparison
		public static bool operator ==(BigDecimal left, BigDecimal right) => left.Equals(right);
		public static bool operator !=(BigDecimal left, BigDecimal right) => !left.Equals(right);
		public static bool operator >(BigDecimal left, BigDecimal right) => left.CompareTo(right) > 0;
		public static bool operator >=(BigDecimal left, BigDecimal right) => left.CompareTo(right) >= 0;
		public static bool operator <(BigDecimal left, BigDecimal right) => left.CompareTo(right) < 0;
		public static bool operator <=(BigDecimal left, BigDecimal right) => left.CompareTo(right) <= 0;
		#endregion
	}

	public class ZeroPrecisionException : Exception
	{
		public ZeroPrecisionException() : base("Precision cannot be zero for this operation.") { }
	}
}