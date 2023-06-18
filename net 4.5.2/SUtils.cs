using System;
using System.Text.RegularExpressions;

namespace SSUnlimited.Utils
{
	public static class SUtils
	{
		#region Random String

		/// <summary>
		/// Generates a random string of the specified length.
		/// </summary>
		/// <param name="length">The length of the string to generate.</param>
		/// <returns>A randomized string from the given parameters</returns>
		public static string GetRandomString(int length, bool useSpecialChars = false)
		{
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			if (useSpecialChars)
				chars += "!@#$%^&*()_+{}|:<>?[];',./`~ \"\\";
			return GetRandomString(length, chars, -1);
		}

		/// <summary>
		/// Generates a random string of the specified length.
		/// </summary>
		/// <param name="length">The length of the string to generate.</param>
		/// <param name="chars">The characters to use in the string.</param>
		/// <returns>A randomized string from the given parameters</returns>
		public static string GetRandomString(int length, string chars)
		{
			return GetRandomString(length, chars, -1);
		}

		/// <summary>
		/// Generates a random string of the specified length.
		/// </summary>
		/// <param name="length">The length of the string to generate.</param>
		/// <param name="chars">The characters to use in the string.</param>
		/// <param name="seed">The seed to use for the random number generator</br>-1 Will be counted as the current time.</param>
		/// <returns>A randomized string from the given parameters</returns>
		public static string GetRandomString(int length, string chars, int seed)
		{
			if (length < 1) throw new ArgumentException("Length must be greater than 0.", nameof(length));
			if (string.IsNullOrEmpty(chars)) throw new ArgumentException("Characters must not be null or empty", nameof(chars));
			if (seed < -1) throw new ArgumentException("Seed must be greater than or equal to -1", nameof(seed));

			Random random = seed == -1 ? new Random((int)DateTime.Now.Ticks) : new Random(seed);
			string result = string.Empty;

			for (int i = 0; i < length; i++)
			{
				result += chars[random.Next(chars.Length)];
			}

			return result;
		}

		/// <summary>
		/// Generates a random string of the specified length.
		/// </summary>
		/// <param name="length">The length of the string to generate.</param>
		/// <param name="chars">The characters to use in the string.</param>
		/// <returns>A randomized string from the given parameters</returns>
		public static string GetRandomString(int length, char[] chars)
		{
			return GetRandomString(length, new String(chars), -1);
		}

		/// <summary>
		/// Generates a random string of the specified length.
		/// </summary>
		/// <param name="length">The length of the string to generate.</param>
		/// <param name="chars">The characters to use in the string.</param>
		/// <param name="seed">The seed to use for the random number generator</br>-1 Will be counted as the current time.</param>
		/// <returns>A randomized string from the given parameters</returns>
		public static string GetRandomString(int length, char[] chars, int seed)
		{
			return GetRandomString(length, new String(chars), seed);
		}

		#endregion
	}
}