using SSUnlimited.Utils;
using System.Text;
using System.Globalization;

namespace System.Numerics
{
	internal static class BigDecimalFormatter
	{
		internal static string Format(BigDecimal value, string? format, IFormatProvider? provider)
		{
			#pragma warning disable CS8602 // Dereference of a possibly null reference.
			if (string.IsNullOrEmpty(format) || format.ToLower() == "g" || provider == null)
				return NormalFormat(value);

			switch (format.ToLower()[0])
			{
				
			}
			#pragma warning restore CS8602 // Dereference of a possibly null reference.

			return "";
		}

		private static string NormalFormat(BigDecimal value)
		{
			if (BigDecimal.IsNaN(value)) return "NaN";
			if (BigDecimal.IsPositiveInfinity(value)) return "∞";
			if (BigDecimal.IsNegativeInfinity(value)) return "-∞";

			if (value._scale.IsZero)
				return value._value.ToString();

			StringBuilder sb = new StringBuilder((value.Sign == -1) ? "-" : string.Empty);

			BigInteger whole = BigInteger.DivRem(BigInteger.Abs(value._value), SMath.Pow(10, value._scale), out BigInteger fraction);

			if (whole != 0)
			{
				sb.Append(whole.ToString());

				if (fraction != 0)
				{
					sb.Append('.');
					BigInteger length = value._scale - SMath.GetLength(fraction);

					// for (BigInteger i = 0; i < length; i++)
					//	sb.Append('0');
					if (length > int.MaxValue)
					{
						while (length > int.MaxValue)
						{
							sb.Append('0', int.MaxValue);
							length -= int.MaxValue;
						}
						sb.Append('0', (int)length);
					}

					sb.Append(fraction.ToString());
				}

				return sb.ToString();
			}

			sb.Append("0.");

			BigInteger zeros = value._scale - SMath.GetLength(fraction);

			for (BigInteger i = 0; i < zeros; i++)
				sb.Append('0');
			
			sb.Append(fraction.ToString());

			return sb.ToString();
		}
	
		internal static bool TryParse(string value, NumberStyles style, IFormatProvider provider, out BigDecimal result)
		{
			result = BigDecimal.Zero;

			NumberFormatInfo info = NumberFormatInfo.GetInstance(provider);

			TryValidateNumberStylesAndInfo(style, info);

			if (string.IsNullOrEmpty(value))
				return false;

			switch (value)
			{
				case "NaN":
					result = BigDecimal.NaN;
					return true;

				case "∞":
					result = BigDecimal.PositiveInfinity;
					return true;
				
				case "-∞":
					result = BigDecimal.NegativeInfinity;
					return true;
			}

			

			// NumberStyles.AllowCurrencySymbol - Allows the currency symbol (ex $, €, £, etc.)
			// NumberStyles.AllowDecimalPoint - Allows the decimal point (ex 1.5, 2.55)
			// NumberStyles.AllowExponent - Allows the exponent (ex 1e+5, 2e-10)
			// NumberStyles.AllowLeadingSign - Allows the leading sign (ex -1, +1)
			// NumberStyles.AllowLeadingWhite - Allows the leading white space (ex "  1", "  2")
			// NumberStyles.AllowParentheses - Allows the parentheses (ex (1), (2))
			// NumberStyles.AllowThousands - Allows the thousands separator (ex 1,000, 2,000)
			// NumberStyles.AllowTrailingSign - Allows the trailing sign (ex 1-, 2+)
			// NumberStyles.AllowTrailingWhite - Allows the trailing white space (ex "1  ", "2  ")
			// NumberStyles.AllowHexSpecifier - Allows the hexadecimal specifier (ex 0x1, 0x2)
			// NumberStyles.Any - Allows all of the above
			// NumberStyles.Float - Allows the decimal point, exponent, leading sign, trailing sign, and leading white space
			// NumberStyles.HexNumber - Allows the hexadecimal specifier, leading white space, and trailing white space
			// NumberStyles.Integer - Allows the leading sign, trailing sign, and leading white space
			// NumberStyles.None - Allows none of the above
			// NumberStyles.Number - Allows the currency symbol, decimal point, exponent, leading sign, trailing sign, leading white space, and trailing white space

			// info.CurrencyDecimalDigits - The number of decimal places in the currency

			return false;
		}

		private static void TryValidateNumberStylesAndInfo(NumberStyles style, NumberFormatInfo info)
		{
			if (style == NumberStyles.AllowHexSpecifier)
				throw new ArgumentException("The number style AllowHexSpecifier is not supported on BigDecimal data types.");
		}
	}
}