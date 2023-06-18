namespace System.Collections.Generic
{
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Adds the specified key and value to the dictionary if the key does not already exist.
		/// </summary>
		/// <returns><c>true</c> if the key was added; otherwise, <c>false</c>.</returns>
		public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			// This method exists in later versions of .NET but why not bring it to an older version.
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			if (dictionary.ContainsKey(key))
				return false;

			dictionary.Add(key, value);
			return true;
		}
		
		public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			if (dictionary.ContainsKey(key))
				return dictionary[key];

			dictionary.Add(key, value);
			return value;
		}
	}
}