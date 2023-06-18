using System;
using System.Numerics;

namespace SSUnlimited.Utils
{
	public static class SMath
	{
		#region Clamp
		// Compressed if statement
		// Compares if value is less than min, if so return min
		// otherwise compares if value is greater than max, if so return max
		// finally if neither of those are true, return the original value

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static sbyte Clamp(sbyte value, sbyte min, sbyte max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static byte Clamp(byte value, byte min, byte max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static short Clamp(short value, short min, short max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static ushort Clamp(ushort value, ushort min, ushort max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static int Clamp(int value, int min, int max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static uint Clamp(uint value, uint min, uint max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static long Clamp(long value, long min, long max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static ulong Clamp(ulong value, ulong min, ulong max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static float Clamp(float value, float min, float max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static double Clamp(double value, double min, double max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static decimal Clamp(decimal value, decimal min, decimal max) => value < min ? min : value > max ? max : value;

		/// <summary>
		/// Clamps a value between a minimum and maximum value
		/// </summary>
		/// <param name="value">The value to clamp</param>
		/// <param name="min">The minimum value</param>
		/// <param name="max">The maximum value</param>
		public static BigInteger Clamp(BigInteger value, BigInteger min, BigInteger max) => value < min ? min : value > max ? max : value;
		#endregion

		public const decimal E = 2.718281828459045235360287471352662497757247093699959574966967627724076630353m;

		#region Pow
		public static BigInteger Pow(BigInteger value, BigInteger power)
		{
			if (power < 0)
				throw new ArgumentOutOfRangeException(nameof(power), "Power cannot be less than 0");
			if (power == 0)
				return 1;
			if (power == 1)
				return value;
			BigInteger result = value;
			for (BigInteger i = 1; i < power; i++)
				result *= value;
			return result;
		}

		public static decimal Pow(decimal value, decimal power)
		{
			if (power < 0)
				throw new ArgumentOutOfRangeException(nameof(power), "Power cannot be less than 0");
			if (power == 0)
				return 1;
			if (power == 1)
				return value;
			decimal result = value;
			for (decimal i = 1; i < power; i++)
				result *= value;
			return result;
		}
		#endregion

		public const decimal PI = 3.141592653589793238462643383279502884197169399375105820974944592307816406286m;
		// public static decimal Acos(decimal value)
		// {

		// }

		// Math.Acos(): double (returns double) (REQUIRES Asin)
		// Math.Asin(): double (returns double)
		// Math.Atan(): double (returns double)
		// Math.Atan2(): double double (returns double)
		// Math.BigMul(): int int (returns long)
		// Math.Ceiling(): double, decimal (returns there respective type)
		// Math.Cos(): double (returns double)
		// Math.Cosh(): double (returns double)
		// Math.DivRem(): int int out int, long long out long (returns there respective type)
		// Math.Equals(): object object (returns bool)
		// Math.Exp(): double (returns double)
		// Math.Floor(): double, decimal (returns there respective type)
		// Math.IEEERemainder(): double double (returns double)
		// Math.Log(): double, double double (returns double)
		// Math.Log10(): double (returns double)
		// Math.Pow(): double double (returns double)
		// Math.ReferenceEquals(): object object (returns bool)
		// Math.Round(): double, double int, double MidpointRounding, double int MidpointRounding,
		//		decimal, decimal int, decimal MidpointRounding, decimal int MidpointRounding (returns there respective type)
		// Math.Sign(): sbyte, short, int, long, float, double, decimal (returns int)
		// Math.Sin(): double (returns double)
		// Math.Sinh(): double (returns double)
		// Math.Sqrt(): double (returns double)
		// Math.Tan(): double (returns double)
		// Math.Tanh(): double (returns double)
		// Math.Truncate(): double, decimal (returns there respective type)
	}
}