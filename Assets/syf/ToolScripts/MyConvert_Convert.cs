using UnityEngine;
using System;
using System.Text;
using System.Collections;

public class MyConvert_Convert
{
    public static ushort ToUInt16(object s)
    {
        return Convert.ToUInt16(s);
    }
    public static short ToInt16(object s)
    {
		return Convert.ToInt16(s);
    }
    public static uint ToUInt32(object s)
    {
		return Convert.ToUInt32(s);
    }


    public static string ToString(object s)
    {
		return Convert.ToString(s);
    }

    public static float ToSingle(object s)
    {
		return Convert.ToSingle(s);
    }

    public static int ToInt32(object s)
    {
		return Convert.ToInt32(s);
    }

    public static byte ToByte(object s)
    {       
		return Convert.ToByte(s);
    }
    public static bool ToBoolean(object s)
    {
		return Convert.ToBoolean(s);
    }
	
	//encoding编码:UTF-8,UNICODE
	public static byte[] StringToByteArray(string s,string encoding)
	{
		return Encoding.GetEncoding(encoding).GetBytes(s);
	}
	
	//encoding编码:UTF-8,UNICODE
	public static string ByteArrayToString(byte[] array,string encoding)
	{
		return Encoding.GetEncoding(encoding).GetString(array);
	}

    public static string UnicodeToString2(byte[] bytes)
    {
        string text = "";

        text = Encoding.Unicode.GetString(bytes);

        for (int i = 0; i < text.Length; ++i)
        {
            if (text[i] == '\0')
            {
                return text.Substring(0, i);
            }
        }
        return text;
    }

    public static String ToUTF8String(byte[] byText)
    {
        return System.Text.Encoding.UTF8.GetString(byText);
    }

    public static byte[] StringToUTF8(string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);

        return bytes;
    }
}

