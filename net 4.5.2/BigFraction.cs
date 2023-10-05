using System.Dynamic;
using System.Text;

namespace System.Numerics
{
	/// <summary>
	/// Utility struct for BigDecimal to represent fractional numbers.\n
	/// Can also be used for other purposes.
	/// </summary>
	public struct BigFraction : IFormattable
	{
		#region Internal values
		internal BigInteger _numerator;
		internal BigInteger _denominator;
		#endregion

		#region Properties
		public BigInteger Numerator
		{ get => _numerator; set => _numerator = value; }

		public BigInteger Denominator
		{ get => _denominator; set => _denominator = value; }
		#endregion

		#region Internal consts
		private static readonly BigFraction _zero = new BigFraction(BigInteger.Zero, BigInteger.One);
		private static readonly BigFraction _fractionalNaN = new BigFraction(BigInteger.Zero, BigInteger.Zero);
		private static readonly BigFraction _one = new BigFraction(BigInteger.One, BigInteger.One);
		private static readonly BigFraction _minusOne = new BigFraction(BigInteger.MinusOne, BigInteger.One);
		private static readonly BigFraction _ten = new BigFraction(BigInteger.One * 10, BigInteger.One);
		private static readonly BigFraction _posInfinity = new BigFraction(BigInteger.One, BigInteger.Zero);
		private static readonly BigFraction _negInfinity = new BigFraction(BigInteger.MinusOne, BigInteger.Zero);
		private static readonly BigFraction _half = new BigFraction(BigInteger.One, BigInteger.One * 2);
		private static readonly BigFraction _fractionOneThree = new BigFraction(BigInteger.One, BigInteger.One * 3);
		#endregion

		#region Public consts
		public static BigFraction Zero { get => _zero; }
		public static BigFraction NaN { get => _fractionalNaN; }
		public static BigFraction One { get => _one; }
		public static BigFraction MinusOne { get => _minusOne; }
		public static BigFraction Ten { get => _ten; }
		public static BigFraction PositiveInfinity { get => _posInfinity; }
		public static BigFraction NegativeInfinity { get => _negInfinity; }
		public static BigFraction Half { get => _half; }
		public static BigFraction OneThird { get => _fractionOneThree; }
		#endregion

		#region Constructors
		public BigFraction(BigInteger numerator, BigInteger denominator)
		{
			_numerator = numerator;
			_denominator = denominator;
		}

		public BigFraction(BigInteger numerator) : this(numerator, BigInteger.One) { }
		#endregion

		/// <summary>
		/// Reduces the fraction to its simplest form.
		/// </summary>
		/// <param name="fraction">The BigFraction instance to simplify</param>
		/// <returns>A Reduced/Simplified fraction.</returns>
		public static BigFraction Simplify(BigFraction fraction)
		{
			BigInteger gcd = BigInteger.GreatestCommonDivisor(fraction._numerator, fraction._denominator);
			return new BigFraction(fraction._numerator / gcd, fraction._denominator / gcd);
		}

		/// <summary>
		/// Matches the denominators of the two fractions to be the same and scales the numerators accordingly.
		/// </summary>
		/// <param name="left">The first fraction to match.</param>
		/// <param name="right">The second fraction to match.</param>
		/// <returns>Two fractions with the same denominator.</returns>
		public static void MatchDenominator(ref BigFraction left, ref BigFraction right)
		{
			BigInteger gcd = BigInteger.GreatestCommonDivisor(left._denominator, right._denominator);
			BigInteger lcm = left._denominator * right._denominator / gcd;

			left._numerator *= lcm / left._denominator;
			right._numerator *= lcm / right._denominator;

			left._denominator = lcm;
			right._denominator = lcm;
		}

		public override string ToString()
		{
			return ToString("a", null);
		}

		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			if (format == null)
				format = "a";

			StringBuilder stringBuilder = new StringBuilder();

			string biFormat = "G";

			if (format.Length > 2)
				biFormat = format.Substring(2);

			if (format[0] == 'a')
			{
				stringBuilder.Append((_numerator / _denominator).ToString(biFormat, formatProvider));
				stringBuilder.Append(' ');
				stringBuilder.Append((_numerator % _denominator).ToString(biFormat, formatProvider));
				stringBuilder.Append('/');
				stringBuilder.Append(_denominator.ToString(biFormat, formatProvider));

				return stringBuilder.ToString();
			}

			if (format[0] == 'b')
			{
				stringBuilder.Append(_numerator.ToString(biFormat, formatProvider));
				stringBuilder.Append('/');
				stringBuilder.Append(_denominator.ToString(biFormat, formatProvider));

				return stringBuilder.ToString();
			}

			throw new FormatException($"The {format[0]} format string is not supported.");
		}

		public static BigFraction Add(BigFraction left, BigFraction right, bool simplify = false)
		{
			BigInteger numerator = (left._numerator * right._denominator) + (right._numerator * left._denominator);
			BigInteger denominator = left._denominator * right._denominator;

			if (simplify)
				return Simplify(new BigFraction(numerator, denominator));

			return new BigFraction(numerator, denominator);
		}

		public static BigFraction Subtract(BigFraction left, BigFraction right, bool simplify = false)
		{
			BigInteger numerator = (left._numerator * right._denominator) - (right._numerator * left._denominator);
			BigInteger denominator = left._denominator * right._denominator;

			if (simplify)
				return Simplify(new BigFraction(numerator, denominator));

			return new BigFraction(numerator, denominator);
		}

		public static BigFraction Multiply(BigFraction left, BigFraction right, bool simplify = false)
		{
			BigInteger numerator = left._numerator * right._numerator;
			BigInteger denominator = left._denominator * right._denominator;

			if (simplify)
				return Simplify(new BigFraction(numerator, denominator));

			return new BigFraction(numerator, denominator);
		}

		public static BigFraction Divide(BigFraction left, BigFraction right, bool simplify = false)
		{
			BigInteger numerator = left._numerator * right._denominator;
			BigInteger denominator = left._denominator * right._numerator;

			if (simplify)
				return Simplify(new BigFraction(numerator, denominator));

			return new BigFraction(numerator, denominator);
		}

		public static BigFraction Negate(BigFraction fraction)
		{
			return new BigFraction(-fraction._numerator, fraction._denominator);
		}

		public static BigDecimal ConvertToBigDecimal(BigFraction fraction, BigInteger precision)
		{
			return new BigDecimal(fraction._numerator) / new BigDecimal(fraction._denominator);
		}

		public static BigFraction operator +(BigFraction left, BigFraction right) => Add(left, right, true);
		public static BigFraction operator ++(BigFraction fraction) => Add(fraction, One, true);
		public static BigFraction operator -(BigFraction left, BigFraction right) => Subtract(left, right, true);
		public static BigFraction operator --(BigFraction fraction) => Subtract(fraction, One, true);

		public static BigFraction operator *(BigFraction left, BigFraction right) => Multiply(left, right, true);
		public static BigFraction operator /(BigFraction left, BigFraction right) => Divide(left, right, true);


	}
}