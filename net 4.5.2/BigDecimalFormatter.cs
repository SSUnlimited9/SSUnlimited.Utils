using SSUnlimited.Utils;
using System.Text;

namespace System.Numerics
{
	internal static class BigDecimalFormatter
	{
		internal static string Format(BigDecimal value, string? format, IFormatProvider? provider)
		{
			#pragma warning disable CS8602 // Dereference of a possibly null reference.
			if (string.IsNullOrEmpty(format) || format.ToLower() == "g" || provider == null)
				return NormalFormat(value);
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

					for (BigInteger i = 0; i < length; i++)
						sb.Append('0');

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


			/*string valueStr = BigInteger.Abs(value._value).ToString();
			StringBuilder stringBuilder = new StringBuilder((value._value < 0) ? "-" : string.Empty);

			if (value.MantissaLength > value._scale)
			{
				// Get both the whole number and the fraction
				BigInteger wholeNum = BigInteger.DivRem(BigInteger.Abs(value._value), SMath.Pow(10, value._scale), out BigInteger fraction);
				stringBuilder.Append(wholeNum.ToString());

				if (fraction != 0)
				{
					stringBuilder.Append('.');
					StringBuilder fractionStr = new StringBuilder(fraction.ToString());
					BigInteger length = value._scale - (BigInteger)fractionStr.Length;

					for (BigInteger i = 0; i < length; i++)
						fractionStr.Insert(0, "0");
					stringBuilder.Append(fractionStr);
				}

				return stringBuilder.ToString();
			}

			stringBuilder.Append("0.");

			BigInteger zeros = value._scale - value.MantissaLength;

			for (BigInteger i = 0; i < zeros; i++)
				stringBuilder.Append('0');

			stringBuilder.Append(valueStr);

			return stringBuilder.ToString();*/
		}
	}
}