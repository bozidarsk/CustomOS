// using System.Runtime;
// using System.Runtime.InteropServices;

// namespace Runtime;

// // strcasecmp
// // strerror
// // strncat
// // strncmp
// // strncpy
// // strtoll
// // strtoull
// // strtoul

// internal static unsafe class String 
// {
// 	[DllImport("*")] private static extern void* memcpy(void* destination, void* source, nuint size);
// 	[DllImport("*")] private static extern void* memset(void* destination, byte value, nuint size);
// 	[DllImport("*")] private static extern void* malloc(nuint size);

// 	[RuntimeExport("strlen")]
// 	private static nuint strlen(byte* str) 
// 	{
// 		nuint size = 0;
// 		while (str[size++] != 0);
// 		return size - 1;
// 	}

// 	[RuntimeExport("strcat")]
// 	private static byte* strcat(byte* destination, byte* source) 
// 	{
// 		strcpy(destination + strlen(destination), source);
// 		return destination;
// 	}

// 	[RuntimeExport("strcmp")]
// 	private static int strcmp(sbyte* a, sbyte* b) 
// 	{
// 		for (nuint i = 0; true; i++) 
// 		{
// 			int diff = a[i] - b[i];

// 			if (diff != 0)
// 				return diff;

// 			if (a[i] == 0 && b[i] == 0)
// 				break;
// 		}

// 		return 0;
// 	}

// 	[RuntimeExport("strcpy")]
// 	private static byte* strcpy(byte* destination, byte* source) 
// 	{
// 		for (nuint i = 0; true; i++) 
// 		{
// 			destination[i] = source[i];

// 			if (source[i] == 0)
// 				break;
// 		}

// 		return destination;
// 	}

// 	[RuntimeExport("strdup")]
// 	private static byte* strdup(byte* str) 
// 	{
// 		return strcpy(malloc(strlen(str)), str);
// 	}

// 	[RuntimeExport("strrchr")]
// 	private static byte* strrchr(byte* str, int c) 
// 	{
// 		nuint index = 0;
// 		bool found = false;

// 		for (nuint i = 0; str[i] != 0; i++) 
// 		{
// 			if (str[i] == (byte)c) 
// 			{
// 				index = i;
// 				found = true;
// 			}
// 		}

// 		return found ? str + index : (byte*)0;
// 	}

// 	[RuntimeExport("strchr")]
// 	private static byte* strchr(byte* str, int c) 
// 	{
// 		for (nuint i = 0; str[i] != 0; i++)
// 			if (str[i] == (byte)c)
// 				return str + i;

// 		return (byte*)0;
// 	}

// 	[RuntimeExport("strstr")]
// 	private static byte* strstr(byte* str, byte* value) 
// 	{
// 		if (value[0] == 0)
// 			return str;

// 		for (nuint i = 0; str[i] != 0; i++) 
// 		{
// 			if (str[i] != value[i])
// 				continue;

// 			if (strcmp((sbyte*)(str + i), (sbyte*)value) == 0)
// 				return str + i;
// 		}

// 		return (byte*)0;
// 	}

// 	// https://github.com/bminor/glibc/blob/207d64feb26279e152c50744e3c37e68491aca99/string/strspn.c#L30
// 	[RuntimeExport("strspn")]
// 	private static nuint strspn(byte* str, byte* accept) 
// 	{
// 		if (accept[0] == 0)
// 			return 0;

// 		if (__glibc_unlikely(accept[1] == 0)) 
// 		{
// 			byte* a = str;
// 			for (; *str == *accept; str++);
// 			return str - a;
// 		}

// 		/* Use multiple small memsets to enable inlining on most targets.  */
// 		byte* table = stackalloc byte[256];
// 		byte* p = memset(table, 0, 64);
// 		memset(p + 64, 0, 64);
// 		memset(p + 128, 0, 64);
// 		memset(p + 192, 0, 64);

// 		byte* s = (byte*)accept;
// 		/* Different from strcspn it does not add the NULL on the table
// 		 so can avoid check if str[i] is NULL, since table['\0'] will
// 		 be 0 and thus stopping the loop check.  */
// 		do
// 			p[*s++] = 1;
// 		while (*s);

// 		s = (byte*)str;
// 		if (!p[s[0]]) return 0;
// 		if (!p[s[1]]) return 1;
// 		if (!p[s[2]]) return 2;
// 		if (!p[s[3]]) return 3;

// 		// s = (byte*)PTR_ALIGN_DOWN(s, 4);
// 		s = (byte*)(((nuint)s) & -((nuint)4));

// 		uint c0, c1, c2, c3;
// 		do 
// 		{
// 			s += 4;
// 			c0 = p[s[0]];
// 			c1 = p[s[1]];
// 			c2 = p[s[2]];
// 			c3 = p[s[3]];
// 		} while ((c0 & c1 & c2 & c3) != 0);

// 		nuint count = s - (byte*)str;
// 		return (c0 & c1) == 0 ? count + c0 : count + c2 + 2;
// 	}

// 	// https://github.com/bminor/glibc/blob/207d64feb26279e152c50744e3c37e68491aca99/string/strcspn.c#L32
// 	[RuntimeExport("strcspn")]
// 	private static nuint strcspn(byte* str, byte* reject) 
// 	{
// 		if (__glibc_unlikely (reject[0] == '\0') || __glibc_unlikely (reject[1] == '\0')) 
// 			return __strchrnul (str, reject [0]) - str;

// 		/* Use multiple small memsets to enable inlining on most targets.  */
// 		byte* table = stackalloc byte[256];
// 		byte* p = memset(table, 0, 64);
// 		memset(p + 64, 0, 64);
// 		memset(p + 128, 0, 64);
// 		memset(p + 192, 0, 64);

// 		byte* s = (byte*)reject;
// 		byte tmp;
// 		do
// 			p[tmp = *s++] = 1;
// 		while (tmp);

// 		s = (byte*)str;
// 		if (p[s[0]]) return 0;
// 		if (p[s[1]]) return 1;
// 		if (p[s[2]]) return 2;
// 		if (p[s[3]]) return 3;

// 		s = (byte* ) PTR_ALIGN_DOWN (s, 4);

// 		uint c0, c1, c2, c3;
// 		do
// 		{
// 		  s += 4;
// 		  c0 = p[s[0]];
// 		  c1 = p[s[1]];
// 		  c2 = p[s[2]];
// 		  c3 = p[s[3]];
// 		}
// 		while ((c0 | c1 | c2 | c3) == 0);

// 		nuint count = s - (byte*)str;
// 		return (c0 | c1) != 0 ? count - c0 + 1 : count - c2 + 3;
// 	}

// 	// https://github.com/bminor/glibc/blob/207d64feb26279e152c50744e3c37e68491aca99/string/strtok_r.c#L41
// 	[RuntimeExport("strtok_r")]
// 	private static byte* strtok_r(byte* s, byte* delim, out byte* save_ptr) 
// 	{
// 		byte* end;

// 		if (s == (byte*)0)
// 			s = save_ptr;

// 		if (*s == 0) 
// 		{
// 			save_ptr = s;
// 			return (byte*)0;
// 		}

// 		/* Scan leading delimiters.  */
// 		s += strspn(s, delim);
// 		if (*s == 0) 
// 		{
// 			save_ptr = s;
// 			return (byte*)0;
// 		}

// 		/* Find the end of the token.  */
// 		end = s + strcspn(s, delim);
// 		if (*end == 0) 
// 		{
// 			save_ptr = end;
// 			return s;
// 		}

// 		/* Terminate the token and make SAVE_PTR point past it.  */
// 		*end = 0;
// 		save_ptr = end + 1;
// 		return s;
// 	}

// 	// [RuntimeExport("strtoll")]
// 	// private static long strtoll(byte* str, out byte* end, int base) 
// 	// {

// 	// }

// 	// [RuntimeExport("strtoull")]
// 	// private static ulong strtoull(byte* str, out byte* end, int base) 
// 	// {

// 	// }

// 	// [RuntimeExport("strtol")]
// 	// private static nint strtol(byte* str, out byte* end, int base) 
// 	// {

// 	// }
// }
