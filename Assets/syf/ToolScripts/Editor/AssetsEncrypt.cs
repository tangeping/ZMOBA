using UnityEngine;
using System.Collections;
using System.IO;

public static class AssetsEncrypt{

    private const int SWAP_INDEX = 8;
    private const int MIN_LENGTH = 128;

    public static string ExtenBuf = ".buf";

    public static string EncryptBeforeBuildAssetsBundle(string fileFullPath)
    {
        string encryptPath = fileFullPath;

        if(!File.Exists(fileFullPath))
        {
            Debug.LogError("Can not find file path : " + fileFullPath);
            return "";
        }

        FileStream fileStreamLoad = new FileStream(fileFullPath, FileMode.Open, FileAccess.ReadWrite);
        long length = fileStreamLoad.Length;

        if(length <= SWAP_INDEX * 2)
            return "";


        byte[] bytes = new byte[length];
        fileStreamLoad.Read(bytes, 0, (int)length);

        for (int i = 0; i < SWAP_INDEX; i++)
        {
            bytes[i] -= (byte)(i % 16);
        }

        fileStreamLoad.Write(bytes,0,(int)length);

        fileStreamLoad.Close();

        return encryptPath;
    }

    public static string EncryptAfterBuildAssetsBundle(string fileFullPath,string newPath)
    {
		if(!File.Exists(fileFullPath))
		{
			Debug.LogError("Can not find file path : " + fileFullPath);
			return "";
		}
		
		FileStream fileStreamLoad = new FileStream(fileFullPath, FileMode.Open, FileAccess.ReadWrite);
		long length = fileStreamLoad.Length;

        Debug.Log(length);

		if(length <= SWAP_INDEX * 2)
			return "";
		
		byte[] bytes = new byte[length];
		fileStreamLoad.Read(bytes, 0, (int)length);
		
		for (int i = 1; i <= SWAP_INDEX; i++)
		{
            bytes[(int)length - i - 32] -= (byte)(i % 16);
		}
		
		fileStreamLoad.Close();

        fileStreamLoad = new FileStream(newPath, FileMode.Create, FileAccess.Write);

        fileStreamLoad.Write(bytes, 0, (int)length);

        fileStreamLoad.Close();

        bytes = null;

        return newPath;
    }

    public static void EncryptBytes(byte[] bytes)
    {
        int length = bytes.Length;
        if (length <= SWAP_INDEX * 2)
            return;

        byte b;
        for (int i = 1; i <= SWAP_INDEX; i++)
        {
            b = bytes[(int)length - i];
            bytes[(int)length - i] = bytes[i];
            bytes[i] = b;
        }
    }

    public static byte[] ReadFileToByte(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("ReadFileToByte Can not find file : " + path);
            return null;
        }

        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        int len = (int)fileStream.Length;
        byte[] bytes = new byte[len];
        fileStream.Read(bytes, 0, len);
        fileStream.Close();

        return bytes;
    }

    public static bool WriteByteToFile(byte[] bytes, string path)
    {
        int len = -1;
        if (bytes != null)
        {
            len = bytes.Length;
        }

        if (len <= 0)
        {
            Debug.LogError("WriteByteToFile bytes is null");
            return false;
        }

        FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
        fileStream.Write(bytes, 0, len);
        fileStream.Close();

        return true;
    }
}
