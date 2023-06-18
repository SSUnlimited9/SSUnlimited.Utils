using System;
using System.Numerics;

namespace SSUnlimited.Utils
{
	/// <summary>
	/// <see cref="System.Random"/> but better
	/// </summary>
	public static class RandomPH
	{
		// 0x00 = Always get seed from internal clock
		// 0x01 = Seed is from user (but also use internal clock)
		private static byte _state = 0x00;
		private static long _seed = DateTime.Now.Ticks;

		public static void ResetState()
		{
			_state = 0x00;
			_seed = DateTime.Now.Ticks;
		}

		public static void SetState(long seed)
		{
			_state = 0x01;
			_seed = seed;
		}
	}

	/// <summary>
	/// <see cref="System.Random"/> but better
	/// </summary>
	public class IRandom : object
	{
		// Seed stuff (for random number generation)
		protected decimal? _seed;
		protected decimal? _nextSeed;
		public string Seed
		{
			get => /*_seed.ToString();*/ "";
			set
			{ if (decimal.TryParse(value, out decimal seed)) _seed = seed; else _seed = (decimal)value.GetHashCode(); }
		}
		
		protected Random _random;

		protected Type _randomType = typeof(int);
		public Return ReturnType
		{
			get
			{
				if (_randomType == typeof(int)) return Return.Int; if (_randomType == typeof(uint)) return Return.UInt;
				if (_randomType == typeof(long)) return Return.Long; if (_randomType == typeof(ulong)) return Return.ULong;
				if (_randomType == typeof(float)) return Return.Float; if (_randomType == typeof(decimal)) return Return.Decimal;
				// if (_randomType == typeof(BigInteger)) return Return.BigInteger; if (_randomType == typeof(BigDecimal)) return Return.BigDecimal;
				return Return.Int;
			}
			set
			{
				if (value == Return.Int) _randomType = typeof(int); if (value == Return.UInt) _randomType = typeof(uint);
				if (value == Return.Long) _randomType = typeof(long); if (value == Return.ULong) _randomType = typeof(ulong);
				if (value == Return.Float) _randomType = typeof(float); if (value == Return.Decimal) _randomType = typeof(decimal);
				// if (value == Return.BigInteger) _randomType = typeof(BigInteger); if (value == Return.BigDecimal) _randomType = typeof(BigDecimal);
			}
		}

		public IRandom(int seed)
		{
			_seed = seed;
			_randomType = typeof(int);
			_random = new Random();
		}

		public IRandom (long seed)
		{
			_seed = seed;
			_randomType = typeof(long);
			_random = new Random();
		}

		public IRandom(float seed)
		{
			_seed = (decimal)seed;
			_randomType = typeof(float);
			_random = new Random();
		}

		public IRandom(decimal seed)
		{
			_seed = seed;
			_randomType = typeof(decimal);
			_random = new Random();
		}

		public IRandom(int seed, Return returnType)
		{
			_seed = seed;
			ReturnType = returnType;
			_random = new Random();
		}

		public IRandom(long seed, Return returnType)
		{
			_seed = seed;
			ReturnType = returnType;
			_random = new Random();
		}

		public IRandom(float seed, Return returnType)
		{
			_seed = (decimal)seed;
			ReturnType = returnType;
			_random = new Random();
		}

		public IRandom(decimal seed, Return returnType)
		{
			_seed = seed;
			ReturnType = returnType;
			_random = new Random();
		}

		// Random number generation
		/// <summary>
		/// Generates a random number from the selected type <see cref="SSUnlimited.Utils.IRandom.Return"/>
		/// </summary>
		// public object Next()
		// {
		// 	switch (ReturnType)
		// 	{
		// 		case Return.SByte: return SByteRNG(sbyte.MinValue, sbyte.MaxValue); case Return.Byte: return ByteRNG(byte.MinValue, byte.MaxValue);
		// 		case Return.Short: return ShortRNG(); case Return.UShort: return UShortRNG();
		// 		default: return IntRNG(int.MinValue, int.MaxValue); case Return.UInt: return UIntRNG();
		// 		case Return.Long: return LongRNG(); case Return.ULong: return ULongRNG();
		// 		case Return.Float: return FloatRNG(); case Return.Double: return DoubleRNG(); case Return.Decimal: return DecimalRNG();
		// 		case Return.BigInteger: return BigIntegerRNG(); case Return.BigDecimal: return BigDecimalRNG();
		// 		case Return.Infinity: return InfinityRNG();
		// 	}
		// }

		public enum Return
		{
			SByte, Byte, Short, UShort,
			Int, UInt, Long, ULong,
			Float, Double, Decimal,
			BigInteger, BigDecimal, Infinity
		}

		// protected decimal? _seed;
		// public string Seed
		// {
		// 	get => _seed.ToString();
		// 	set { if (decimal.TryParse(value, out decimal seed)) _seed = seed; else _seed = (decimal)value.GetHashCode(); }
		// }

		// // This value changes every time Next() is called (and is used to calculate the next seed)
		// protected decimal? _multiplierSeed;

		// // What numeric type to return
		// protected Type _randomType;
		// public IRandomType ReturnType
		// {
		// 	get
		// 	{
		// 		if (_randomType == typeof(sbyte)) return IRandomType.SByte; if (_randomType == typeof(byte)) return IRandomType.Byte;
		// 		if (_randomType == typeof(short)) return IRandomType.Short; if (_randomType == typeof(ushort)) return IRandomType.UShort;
		// 		if (_randomType == typeof(int)) return IRandomType.Int; if (_randomType == typeof(uint)) return IRandomType.UInt;
		// 		if (_randomType == typeof(long)) return IRandomType.Long; if (_randomType == typeof(ulong)) return IRandomType.ULong;
		// 		if (_randomType == typeof(float)) return IRandomType.Float; if (_randomType == typeof(double)) return IRandomType.Double; if (_randomType == typeof(decimal)) return IRandomType.Decimal;
		// 		if (_randomType == typeof(BigInteger)) return IRandomType.BigInteger; if (_randomType == typeof(BigDecimal)) return IRandomType.BigDecimal;
		// 		return IRandomType.Int;
		// 	}
		// 	set
		// 	{
		// 		if (value == IRandomType.SByte) _randomType = typeof(sbyte); if (value == IRandomType.Byte) _randomType = typeof(byte);
		// 		if (value == IRandomType.Short) _randomType = typeof(short); if (value == IRandomType.UShort) _randomType = typeof(ushort);
		// 		if (value == IRandomType.Int) _randomType = typeof(int); if (value == IRandomType.UInt) _randomType = typeof(uint);
		// 		if (value == IRandomType.Long) _randomType = typeof(long); if (value == IRandomType.ULong) _randomType = typeof(ulong);
		// 		if (value == IRandomType.Float) _randomType = typeof(float); if (value == IRandomType.Double) _randomType = typeof(double); if (value == IRandomType.Decimal) _randomType = typeof(decimal);
		// 		if (value == IRandomType.BigInteger) _randomType = typeof(BigInteger); if (value == IRandomType.BigDecimal) _randomType = typeof(BigDecimal);
		// 	}
		// }
		

		// public IRandom(sbyte seed)
		// {
		// 	_seed = (decimal)seed;
		// 	_randomType = typeof(int);
		// }
		// // public IRandom(byte seed) => _seed = seed;
		// // public IRandom(short seed) => _seed = seed;
		// // public IRandom(ushort seed) => _seed = seed;
		// // public IRandom(int seed) => _seed = seed;
		// // public IRandom(uint seed) => _seed = seed;
		// // public IRandom(long seed) => _seed = seed;
		// // public IRandom(ulong seed) => _seed = seed;
		// // public IRandom(float seed) => _seed = (decimal)seed;
		// // public IRandom(double seed) => _seed = (decimal)seed;
		// // public IRandom(decimal seed)
		// // {

		// // }
		// // public IRandom(BigInteger seed) => Seed = seed.ToString();
		// // public IRandom(BigDecimal seed) => Seed = seed.ToString();
		// // public IRandom(string seed) => Seed = seed;
		// // public IRandom(char seed) => Seed = seed.ToString();

		// public enum IRandomType
		// {
		// 	SByte, Byte, Short, UShort, Int, UInt, Long, ULong,
		// 	Float, Double, Decimal,
		// 	BigInteger, BigDecimal
		// }



		// // public T Next<T>()
		// // {
		// // 	return (T)Convert.ChangeType(Next(), typeof(T));
		// // }
	}
}