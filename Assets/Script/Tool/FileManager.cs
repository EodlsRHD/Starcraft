using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;

public struct UnZipMapFile
{
    public bool isSuccess;
    public MapData mapData;
    public TerrainData terrainData;
}

public class FileManager : MonoBehaviour
{
    private readonly string _txtFileExtension = ".txt";

    public void Initialize()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string gamePath = path + @"/Unity_StarCraft2";

        DirectoryInfo diGame = new DirectoryInfo(gamePath);

        if (diGame.Exists == false)
        {
            diGame.Create();

            string mapsPath = path + @"/Unity_StarCraft2/maps";
            DirectoryInfo diMaps = new DirectoryInfo(mapsPath);

            if (diMaps.Exists == false)
            {
                diMaps.Create();
            }
        }
    }

    public bool FileCheck(string path)
    {
        return File.Exists(path);
    }

    public string ReadFile(string Path)
    {
        return File.ReadAllText(Path);
    }

    public string JsonSerialize(object body)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(body);
    }

    public T JsonDeserialize<T>(T t, string json)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, t);
    }

    public UnZipMapFile UnZipFile(string path)
    {
        UnZipMapFile file = new UnZipMapFile();

        if(FileCheck(path) == false)
        {
            file.isSuccess = false;
            return file;
        }

        file.isSuccess = true;

        string temporaryFilePath = path.Split(".")[0];
        string fileName = Path.GetFileNameWithoutExtension(path);
        
        DirectoryInfo dInfo =  Directory.CreateDirectory(temporaryFilePath);

        ZipFile.ExtractToDirectory(path, dInfo.FullName);

        List<FileInfo> files = dInfo.EnumerateFiles().ToList();

        for (int i = 0; i < files.Count; i++)
        {
            if (Path.GetExtension(files[i].Name).Equals(_txtFileExtension))
            {
                using (StreamReader sr = new StreamReader(files[i].FullName))
                {
                    file.mapData = JsonDeserialize(new MapData(), sr.ReadToEnd());
                }
            }
        }

        files.ForEach(f => f.Delete());
        dInfo.EnumerateDirectories().ToList().ForEach(d => d.Delete(true));
        Directory.Delete(temporaryFilePath, true);

        return file;
    }

    public T ByteArrayToStruct<T>(byte[] buffer) where T : class
    {
        int size = Marshal.SizeOf(typeof(T));
        if (size > buffer.Length)
        {
            throw new Exception();
        }

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(buffer, 0, ptr, size);
        T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return obj;
    }

    public Sprite ConvertTextureToSprite(Texture texture)
    {
        Texture2D texture2D = (Texture2D)texture;
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(texture2D.width * 0.5f, texture2D.height * 0.5f));
    }
}