// BigDecimalFormatter Provides a shorthand for formatting BigDecimal values
// Whether its formatting to a custom string or parsing from a given string value
// ========================================================
// List of BigDecimal String Formatting Options and their effects

using SSUnlimited.Utils;
using System;
using System.Globalization;
using System.Text;

namespace System.Numerics
{
	internal static class BigDecimalFormatter
	{
		internal static string Format(BigDecimal value, string? format, IFormatProvider? formatProvider)
		{
			if (string.IsNullOrEmpty(format) || format.ToLower() == "g" || formatProvider == null)
				return NormalFormat(value);

			return "";
		}

		private static string NormalFormat(BigDecimal value)
		{
			if (BigDecimal.IsPositiveNaN(value)) return "NaN";
			if (BigDecimal.IsNegativeNaN(value)) return "-NaN";
			if (BigDecimal.IsPositiveInfinity(value)) return "∞";
			if (BigDecimal.IsNegativeInfinity(value)) return "-∞";

			if (value._scale.IsZero)
				return value._value.ToString();

			string valueStr = BigInteger.Abs(value._value).ToString();
			StringBuilder sb = new StringBuilder((value._value < 0) ? "-" : string.Empty);

			if (valueStr.Length > value._scale)
			{
				Console.WriteLine("length > scale");
				// Get both the whole number and the fraction
				BigInteger wholeNum = BigInteger.DivRem(BigInteger.Abs(value._value), SMath.Pow(10, value._scale), out BigInteger fraction);
				sb.Append(wholeNum.ToString());

				if (fraction != 0)
				{
					sb.Append('.');
					StringBuilder fractionStr = new StringBuilder(fraction.ToString());
					BigInteger length = value._scale - (BigInteger)fractionStr.Length;

					for (BigInteger i = 0; i < length; i++)
						fractionStr.Insert(0, "0");
					sb.Append(fractionStr);
				}

				return sb.ToString();
			}

			Console.WriteLine("length < scale");

			sb.Append("0.");

			BigInteger zeros = value._scale - (BigInteger)valueStr.Length;
			for (BigInteger i = 0; i < zeros; i++)
				sb.Append('0');
			sb.Append(valueStr);

			return sb.ToString();
		}
	}
}

/*
namespace System.Numerics
{
    /// <summary>
    /// Provides system for formatting BigDecimal values
    /// </summary>
    internal static class BigDecimalFormatter
    {
        internal static string Format(BigDecimal value, string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrEmpty(format) || format.ToLower() == "g" || formatProvider == null)
                return NormalFormat(value);

			// TODO: Add custom formatting options

            return null;
        }

        private static string NormalFormat(BigDecimal value)
        {
			if (BigDecimal.IsPositiveNaN(value)) return "NaN";
			if (BigDecimal.IsNegativeNaN(value)) return "-NaN";
			if (BigDecimal.IsPositiveInfinity(value)) return "∞";
			if (BigDecimal.IsNegativeInfinity(value)) return "-∞";
			if (BigDecimal.DeltaIsAbsolute(value)) return "Δ";

            value.Normalize();

			if (value._scale == 0)
				return value._value.ToString();

			string valueStr = BigInteger.Abs(value._value).ToString();
			StringBuilder sb = new StringBuilder((value._value < 0) ? "-" : string.Empty);

			if (valueStr.Length > value._scale)
			{
				// Get both the whole number and the fraction
				BigInteger wholeNum = BigInteger.DivRem(BigInteger.Abs(value._value), SMath.Pow(10, value._scale), out BigInteger fraction);
				string wholeStr = wholeNum.ToString();
				sb.Append(wholeStr);

				if (fraction != 0)
				{
					sb.Append('.');
					StringBuilder fractionStr = new StringBuilder(fraction.ToString());
					BigInteger length = value._scale - (BigInteger)fractionStr.Length;

					for (BigInteger i = 0; i < length; i++)
						fractionStr.Insert(0, "0");
					sb.Append(fractionStr);
				}

				return sb.ToString();
			}

			sb.Append("0.");

			BigInteger zeros = value._scale - (BigInteger)valueStr.Length;
			for (BigInteger i = 0; i < zeros; i++)
				sb.Append('0');
			sb.Append(valueStr);

			return sb.ToString();
        }

		internal static bool TryParse(string value, NumberStyles style, NumberFormatInfo info, out BigDecimal result)
		{
			result = BigDecimal.Zero;
			return false;
		}


		internal static BigDecimal Parse(string value, NumberStyles? styles, IFormatProvider? provider)
		{
			switch (value.ToLower())
			{
				case "nan": return BigDecimal.NaN;
				case "-nan": return BigDecimal.NegativeNaN;
				case "∞": case "inf": case "infinity": return BigDecimal.PositiveInfinity;
				case "-∞": case "-inf": case "-infinity": return BigDecimal.NegativeInfinity;
				case "delta": return BigDecimal.AbsoluteDelta;
			}

			if (styles == NumberStyles.None)
			return BigDecimal.Zero;

			return BigDecimal.Zero;

			/*
			NumberStyles.AllowCurrencySymbol: $123.45
			NumberStyles.AllowDecimalPoint: 123.45
			NumberStyles.AllowExponent: 1.2345e2
			NumberStyles.AllowHexSpecifier: 0x12345
			NumberStyles.AllowLeadingSign: -123.45
			NumberStyles.AllowLeadingWhite: " 123.45"
			NumberStyles.AllowParentheses: "(123.45)"
			NumberStyles.Any: Any of the above
			NumberStyles.Currency: Same as AllowCurrencySymbol | AllowLeadingSign | AllowTrailingSign | AllowParentheses
			NumberStyles.Float: Same as AllowDecimalPoint | AllowExponent
			NumberStyles.HexNumber: Same as AllowHexSpecifier
			NumberStyles.Integer: Same as AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign
			NumberStyles.None: No styles
			NumberStyles.Number: Same as AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowTrailingSign | AllowDecimalPoint | AllowThousands | AllowExponent
			
		}
    }
}*/