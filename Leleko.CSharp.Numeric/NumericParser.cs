using System;
using System.Collections.Generic;

namespace Leleko.CSharp
{
	/// <summary>
	/// delegate for purser
	/// </summary>
	/// <typeparam name="TOutput">extracting type</typeparam>
	/// <param name="s">expression</param>
	/// <param name="index">start index</param>
	/// <param name="value">result</param>
	/// <returns>success or not</returns>©©©©
	public delegate bool ParseFunc<TOutput>(string s,ref int index,out TOutput value);
	

	/// <summary>
	/// parse provider for numbers types
	/// </summary>
	static public class NumericParser
	{
		/// <summary>
		/// separators of decimal part
		/// </summary>
		[Flags]
		public enum NumberSeparator : byte
		{
			/// <summary>
			/// разделители недопускаются
			/// </summary>
			None = 0x00,
			/// <summary>
			/// '.'
			/// </summary>
			Point = 0x01,
			/// <summary>
			/// ','
			/// </summary>
			Comma = 0x02,
			/// <summary>
			/// {',';'.'}
			/// </summary>
			All = Point | Comma,
		}

		/// <summary>
		/// extract {bool} variable from string with index
		/// </summary>
		/// <param name="s">expression</param>
		/// <param name="index">start index</param>
		/// <param name="value">result</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, ref int index, out bool value)
		{
			int i = index, length = s.Length;
			value = false;
			to_skip_spaces:
			if (i < length)
			{
				switch (s[i++])
				{
					case '\t':
					case '\n':
					case '\r':
					case ' ':
						goto to_skip_spaces;
					case '0':
						index = i;
						return true;
					case '1':
						index = i;
						value = true;
						return true;
					case 'T':
						if (length > i + 2)
						{
							if (s[i] == 'R')
							{
								if (s[++i] == 'U' && s[++i] == 'E')
								{
									index = i + 1;
									value = true;
									return true;
								}
								else
									break;
							}
							else if (s[i] == 'r')
								goto to_t_after_check;
							else
								break;
						}
						else
							break;
					case 't':
						if (length < i + 3 || s[i] != 'r')
							break;
						to_t_after_check:
						if (s[++i] == 'u' && s[++i] == 'e')
						{
							index = i + 1;
							value = true;
							return true;
						}
						else
							break;
					case 'F':
						if (length > i + 3)
						{
							if (s[i] == 'A')
							{
								if (s[++i] == 'L')
								{
									if (s[++i] == 'S' && s[++i] == 'E')
									{
										index = i + 1;
										value = false;
										return true;
									}
									else
										break;
								}
								else if (s[i] == 'l')
								{
									if (s[++i] == 's' && s[++i] == 'e')
									{
										index = i + 1;
										value = false;
										return true;
									}
									else
										break;
								}
								else
									break;
							}
							else if (s[i] == 'a')
								goto to_f_after_check;
							else
								break;
						}
						else
							break;
					case 'f':
						if (length < i + 4 || s[i] != 'a')
							break;
						to_f_after_check:
						if (s[++i] == 'l' && s[++i] == 's' && s[++i] == 'e')
						{
							index = i + 1;
							return true;
						}
						else
							break;
				}
			}
			return false;
		}
		/// <summary>
		/// convert string to {bool} variable
		/// </summary>
		/// <param name="s">expression</param>
		/// <param name="value">result</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, out bool value)
		{
			int i = 0;
			if (TryParse(s, ref i, out value))
			{
				for(;i < s.Length;i++)
				{
					char c = s[i];
					if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
						return false;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// extract {int} variable from string with index
		/// </summary>
		/// <param name="s">expression</param>
		/// <param name="index">start index</param>
		/// <param name="value">result</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, ref int index, out int value)
		{
			char c;
			int i = index, length = s.Length;
			value = 0;
			int locvalue = 0;
			bool sign = true;
			for (;i < length;i++)
			{
				c = s[i];
				if (c == '-') 
				{ 
					sign = false;
					i++;
					break; 
				}
				else if (c == '+') 
				{
					i++;
					break;
				}
				else if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
					break;
			}
			if (i < length)
			{
				c = s[i];
				if ('0'<=c && c<='9')
					unchecked
					{
						locvalue = (c - '0');
					}
				else return false;
				while (++i < length )
				{
					c = s[i];
					if ('0'<=c && c<='9')
						unchecked
						{
							locvalue = locvalue * 10 + (c - '0');
						}
					else break;
				}
				index = i;
				if (sign)
				{
					value = locvalue;
					return true;
				}
				else
				{
					value = -locvalue;
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// convert string to {int} variable
		/// </summary>
		/// <param name="s">expression</param>
		/// <param name="value">result</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, out int value)
		{
			int i = 0;
			if (TryParse(s, ref i, out value))
			{
				for(;i < s.Length;i++)
				{
					char c = s[i];
					if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
						return false;
				}
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// extract {long} variable from string with index
		/// </summary>
		/// <param name="s">expression</param>
		/// <param name="index">start index</param>
		/// <param name="value">result</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, ref int index, out long value)
		{
			char c;
			int i = index, length = s.Length;
			value = 0;
			long locvalue = 0;
			bool sign = true;
			for (;i < length;i++)
			{
				c = s[i];
				if (c == '-') 
				{ 
					sign = false;
					i++;
					break; 
				}
				else if (c == '+') 
				{
					i++;
					break;
				}
				else if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
					break;
			}
			if (i < length)
			{
				c = s[i];
				if ('0'<=c && c<='9')
					unchecked
				{
					locvalue = (c - '0');
				}
				else return false;
				while (++i < length )
				{
					c = s[i];
					if ('0'<=c && c<='9')
						unchecked
					{
						locvalue = locvalue * 10 + (c - '0');
					}
					else break;
				}
				index = i;
				if (sign)
				{
					value = locvalue;
					return true;
				}
				else
				{
					value = -locvalue;
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// convert string to {long} variable
		/// </summary>
		/// <param name="s">expression</param>
		/// <param name="value">result</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, out long value)
		{
			int i = 0;
			if (TryParse(s, ref i, out value))
			{
				for(;i < s.Length;i++)
				{
					char c = s[i];
					if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
						return false;
				}
				return true;
			}
			return false;
		}
	
		static double Pow10D(int pow)
		{
			if (pow > 308)
				return double.PositiveInfinity;
			else
			{
				double value = 1;
				if ((pow & 1) == 1)
					value = 1e+1;         //1
				if (((pow >> 1) & 1) == 1)
					value *= 1e+2; //2
				if (((pow >> 2) & 1) == 1)
					value *= 1e+4; //4
				if (((pow >> 3) & 1) == 1)
					value *= 1e+8; //8
				if (((pow >> 4) & 1) == 1)
					value *= 1e+16; //16
				if (((pow >> 5) & 1) == 1)
					value *= 1e+32; //32
				if (((pow >> 6) & 1) == 1)
					value *= 1e+64; //64
				if (((pow >> 7) & 1) == 1)
					value *= 1e+128; //128
				if (((pow >> 8) & 1) == 1)
					value *= 1e+256; //256
				return value;
			}
		}
		/// <summary>
		/// extract {long} variable from string with index
		/// </summary>
		/// <param name="s">expression</param>
		/// <param name="index">start index</param>
		/// <param name="value">result</param>
		/// <param name="numberSeparator">separator of decimal</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, ref int index, out double value, NumberSeparator numberSeparator)
		{
			char c;
			int i = index, length = s.Length;
			value = 0;

			double locvalue = 0, mlt = 1d;
			int degree = 0, label = 0;
			bool sign = true;
			bool sign_degree = true;
				
			for (;i < length;i++)
			{
				c = s[i];
				if (c == '-') 
				{ 
					sign = false;
					i++;
					break; 
				}
				else if (c == '+') 
				{
					i++;
					break;
				}
				else if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
					break;
			}
			if (i < length)
			{
				c = s[i];
				if ('0'<=c && c<='9')
				{
					unchecked
					{
						locvalue = (c - '0');
					}
				}
				#region [.,]
				else if (
					('.' == c && ((numberSeparator & NumberSeparator.Point) == NumberSeparator.Point)) ||
					(',' == c && ((numberSeparator & NumberSeparator.Comma) == NumberSeparator.Comma)))
				{
					label = i;
					goto to_parse_real;
				}
				#endregion
				#region [NaN]
				else if ((char)65533 == c)
					goto to_try_parse_nan_success;
				else if (('N' == c || 'n' == c) && (length > 3))
					goto to_try_parse_nan;
				#endregion
				#region [Infinity]
				else if ((char)8734 == c)
					goto to_try_parse_infinity_success;
				else if (('I' == c || 'i' == c) && (length - i > 7))
					goto to_try_parse_infinity;
				#endregion
				else return false;
				while (++i < length )
				{
					c = s[i];
					if ('0'<=c && c<='9')
						unchecked
						{
							locvalue = locvalue * 10 + (c - '0');
						}
					#region [.,]
					else if (
						('.' == c && ((numberSeparator & NumberSeparator.Point) == NumberSeparator.Point)) ||
						(',' == c && ((numberSeparator & NumberSeparator.Comma) == NumberSeparator.Comma)))
					{
						label = i;
						goto to_parse_real;
					}
					#endregion
					#region [Ee]
					else if ('E'==c || 'e'==c)
						goto to_parse_degree;
					#endregion
					else goto to_return;
				}
				#region [To Return]
			to_return:
				index = i;
				if (sign)
					value = locvalue;
				else
					value = -locvalue;
				return true;
				#endregion
				#region [To Real][.][,]
			to_parse_real:
				while (++i < length)
				{
					mlt *= 0.1d;
					c = s[i];
					if ('0'<=c && c<='9')
						unchecked
						{
							locvalue = locvalue + (c - '0') * mlt;
						}
					#region [Ee]
					else if ('E'==c || 'e'==c)
						goto to_parse_degree;
					#endregion
					else if (i == label + 1)
						return false;
					else
						goto to_return;
					
				}
				goto to_return;
				#endregion
				#region [To degree][E][e]
			to_parse_degree:
				if (++i < length)
				{
					c = s[i];
					if ('+'==c || '-'==c)
					{
						sign_degree = (c == '+');
						if ((++i < length) && ('0'<=(c=s[i]) && c<='9')) 
							unchecked
							{
								degree = (c - '0');
							}
						else 
						{	
							i = -2;
							goto to_return;
						}
					}
					else if ('0'<=c && c<='9')
						unchecked
						{
							degree = (c - '0');
						}
					else 
					{
						i--;
						goto to_return;
					}
					while((++i < length) && ('0'<=(c=s[i]) && c<='9'))
						unchecked
						{
							degree = degree * 10 + (c - '0');
						}
					if (sign_degree)
					{
						locvalue *= Pow10D(degree);
						goto to_return;
					}
					else
					{
						locvalue /= Pow10D(degree);
						goto to_return;
					}
				}
				#endregion
				#region [To Try Nan]
			to_try_parse_nan:
				if ( 'N' == c)
				{
					if ('a'==s[++i])
					{
						if ('N'==s[++i] || 'n'==s[i])
							goto to_try_parse_nan_success;
						else return false;
					}
					else if ('A' == s[i])
					{
						if ('N'==s[++i] || 'n'==s[i])
							goto to_try_parse_nan_success;
						else return false;
					}
					else return false;
				}
				else if ( 'n' == c && 'a' == s[++i] && 'n' == s[++i])
					goto to_try_parse_nan_success;
				else return false;
			to_try_parse_nan_success:
				{
					i++;
					locvalue = double.NaN;
					goto to_return;
				}
				#endregion
				#region [To Try Infinity]
			to_try_parse_infinity:
				if ( 'I' == c)
				{
					if ('N'==s[++i])
					{
						if ('F'==s[++i] && 'I'==s[++i] && 'N'==s[++i] && 'I'==s[++i] && 'T'==s[++i] && 'Y'==s[++i])
							goto to_try_parse_infinity_success;
						else return false;
					}
					else if ('n'==s[i])
					{
						goto to_try_parse_infinity_lower_case;
					}
					else return false;
				}
				else if ( 'i' == c )
				{
					if ('n'==s[++i])
						goto to_try_parse_infinity_lower_case;
					else return false;
				}
				else return false;
			to_try_parse_infinity_lower_case:
				{
					if ('f'==s[++i] && 'i'==s[++i] && 'n'==s[++i] && 'i'==s[++i] && 't'==s[++i] && 'y'==s[++i])
						goto to_try_parse_infinity_success;
					else return false;
				}
			to_try_parse_infinity_success:
				{
					i++;
					locvalue = double.PositiveInfinity;
					goto to_return;
				}
				#endregion
			}
			return false;
		}
		/// <summary>
		/// convert string to {long} variable
		/// </summary>
		/// <param name="s">expression</param>
		/// <param name="value">result</param>
		/// <param name="numberSeparator">separator of decimal</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, out double value, NumberSeparator numberSeparator)
		{
			int i = 0;
			if (TryParse(s, ref i, out value, numberSeparator))
			{
				for(;i < s.Length;i++)
				{
					char c = s[i];
					if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
						return false;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// convert string to {TimeSpan} variable
		/// </summary>
		/// <param name="s">expression in standart format {dd.hh:mm:ss.ffff}</param>
		/// <param name="index">start index</param>
		/// <param name="value">result</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, ref int index, out TimeSpan value)
		{
			long ticks;
			int i = index, length = s.Length;
			if (TryParse(s, ref i, out ticks))
			{
				if (i < length)
				{
					long subticks;
					if (s[i] == '.')
					{
						ticks *= TimeSpan.TicksPerDay;	// дни
						i++;
						if (TryParse(s, ref i, out subticks))
							checked { ticks += subticks * TimeSpan.TicksPerHour; }
						else
							goto to_fail;
					}
					else if (s[i] == ':')
						checked { ticks *= TimeSpan.TicksPerHour; } // часы
					else
						goto to_success; // считаем что число - количество тиков


					if 	(i<length && s[i++] == ':' && TryParse(s, ref i, out subticks))
					{
						checked { ticks += subticks * TimeSpan.TicksPerMinute; }
						if (i<length && s[i++] == ':' && TryParse(s, ref i, out subticks))
						{
							checked { ticks += subticks * TimeSpan.TicksPerSecond; }
							if (i<length && s[i] == '.')
							{
								i++;
								if (TryParse(s, ref i, out subticks))
									checked { ticks += subticks * TimeSpan.TicksPerMillisecond; }
								else
									i--;
							}
							goto to_success;
						}
					}
					goto to_fail;
				}
			to_success:
				value = new TimeSpan(ticks);
				index = i;
				return true;
			}
		to_fail:
			value = default(TimeSpan);
			return false;
		}
		/// <summary>
		/// convert string to {TimeSpan} variable
		/// 
		/// </summary>
		/// <param name="s">expression in standart format {dd.hh:mm:ss.ffff}</param>
		/// <param name="value">result</param>
		/// <returns>success or not</returns>
		static public bool TryParse(string s, out TimeSpan value)
		{
			int i = 0;
			if (TryParse(s, ref i, out value))
			{
				for(;i < s.Length;i++)
				{
					char c = s[i];
					if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
						return false;
				}
				return true;
			}
			return false;
		}

	}

	/// <summary>
	/// parse provider for array and collection
	/// </summary>
	public static class CollectionParser
	{
		/// <summary>
		/// extract {collection[TOutput]} from string with index
		/// </summary>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="s">expression</param>
		/// <param name="index">start index</param>
		/// <param name="separator">separator of elements in collection</param>
		/// <param name="parseElementFn">fuction for pursing elements in [Touput]</param>
		/// <param name="elementsCount">number of elements was resulted</param>
		/// <param name="outCollection">collection who added resulted elements</param>
		/// <returns>success or not</returns>
		static public bool TryParse<TOutput>(string s, ref int index, char separator, ParseFunc<TOutput> parseElementFn, out int elementsCount, ICollection<TOutput> outCollection)
		{
			int i = index, length = s.Length;
			TOutput value;
			elementsCount = 0;
			
			while ((i<length) && parseElementFn(s, ref i, out value))
			{
				outCollection.Add(value);
				elementsCount++;

				for(;i < length;i++)
				{
					char c = s[i];
					if (c == separator)
					{
						i++;
						break;
					}
					else if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
					{
						index = i;
						return false;
					}
				}
			}
			index = i;
			return true;
		}
		
		/// <summary>
		/// extract {collection[TOutput]} from string
		/// </summary>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="s">expression</param>
		/// <param name="separator">separator of elements in collection</param>
		/// <param name="parseElementFn">fuction for pursing elements in [Touput]</param>
		/// <param name="elementsCount">number of elements was resulted</param>
		/// <param name="outCollection">collection who added resulted elements</param>
		/// <returns>success or not</returns>
		static public bool TryParse<TOutput>(string s, char separator, ParseFunc<TOutput> parseElementFn, out int elementsCount, ICollection<TOutput> outCollection)
		{
			int i = 0;
			if (TryParse(s, ref i, separator, parseElementFn, out elementsCount, outCollection))
			{
				for(;i < s.Length;i++)
				{
					char c = s[i];
					if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
						return false;
				}
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// extract {collection[TOutput]} from string with index
		/// </summary>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="s">expression</param>
		/// <param name="index">start index</param>
		/// <param name="separator">separator of elements in collection</param>
		/// <param name="parseElementFn">fuction for pursing elements in [Touput]</param>
		/// <param name="elementsCount">number of elements was resulted</param>
		/// <param name="outCollection">collection who added resulted elements</param>
		/// <returns>success or not</returns>
		static public bool TryParse<TOutput>(string s, ref int index, string separator, ParseFunc<TOutput> parseElementFn, out int elementsCount, ICollection<TOutput> outCollection)
		{
			int i = index, length = s.Length;
			TOutput value;
			elementsCount = 0;
			
			while ((i<length) && parseElementFn(s, ref i, out value))
			{
				outCollection.Add(value);
				elementsCount++;

				for(;i < length;i++)
				{
					char c = s[i];
					if (string.Compare(s, i, separator, 0, separator.Length) == 0)
					{
						i += separator.Length;
						break;
					}
					else if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
					{
						index = i;
						return false;
					}
				}
			}
			index = i;
			return true;
		}
		
		/// <summary>
		/// extract {collection[TOutput]} from string with index
		/// </summary>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="s">expression</param>
		/// <param name="separator">separator of elements in collection</param>
		/// <param name="parseElementFn">fuction for pursing elements in [Touput]</param>
		/// <param name="elementsCount">number of elements was resulted</param>
		/// <param name="outCollection">collection who added resulted elements</param>
		/// <returns>success or not</returns>
		static public bool TryParse<TOutput>(string s, string separator, ParseFunc<TOutput> parseElementFn, out int elementsCount, ICollection<TOutput> outCollection)
		{
			int i = 0;
			if (TryParse(s, ref i, separator, parseElementFn, out elementsCount, outCollection))
			{
				for(;i < s.Length;i++)
				{
					char c = s[i];
					if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
						return false;
				}
				return true;
			}
			return false;
		}
	}
}