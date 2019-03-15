using UnityEngine;
using UnityEditor;
using System.Collections;

public class MassSetTextureImporter
{
    [MenuItem("Scripts/Mass Set TextureImporter")]
    static void Execute()
    {
        TextureImporterType TextureType = TextureImporterType.Default;
        TextureImporterFormat TextureFormat;
        
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (!(o is Texture2D)) continue;
            string path = AssetDatabase.GetAssetPath(o);

            Debug.Log(path);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            importer.mipmapEnabled = false;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.textureType = TextureType;
            importer.isReadable = false;
         
            //importer.SetPlatformTextureSettings(RuntimePlatform.OSXEditor.ToString(), 1024, TextureImporterFormat.PVRTC_RGBA4);
            //importer.SetPlatformTextureSettings(RuntimePlatform.WindowsEditor.ToString(), 1024, TextureImporterFormat.DXT5);

            if (importer.grayscaleToAlpha)
            {
                TextureFormat = TextureImporterFormat.Alpha8;
            }
            else if (importer.DoesSourceTextureHaveAlpha())
            {
                TextureFormat = TextureImporterFormat.AutomaticCompressed;
            }
            else
            {
#if UNITY_IPHONE
                TextureFormat = TextureImporterFormat.PVRTC_RGB4;
#elif UNITY_ANDROID
                TextureFormat = TextureImporterFormat.ETC_RGB4;
#else
                TextureFormat = TextureImporterFormat.RGB24;
#endif
            }

            importer.textureFormat = TextureFormat;

            AssetDatabase.ImportAsset(path);
           
        }
        AssetDatabase.Refresh();
    }


    public static void ChangeTextureFormat(Object obj)
    {
        Object[] dependObjects;
        dependObjects = EditorUtility.CollectDependencies(new Object[] { obj });
        foreach (Object val in dependObjects)
        {
            if (val is Texture2D)
            {
                
                TextureImporterFormat TextureFormat;

                string path = AssetDatabase.GetAssetPath(val);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                importer.mipmapEnabled = false;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.textureType = TextureImporterType.Default;
                importer.isReadable = false;


                if (importer.grayscaleToAlpha)
                {
                    TextureFormat = TextureImporterFormat.Alpha8;
                }
                else if (importer.DoesSourceTextureHaveAlpha())
                {
                    TextureFormat = TextureImporterFormat.AutomaticCompressed;
                }
                else
                {
#if UNITY_IPHONE
                TextureFormat = TextureImporterFormat.PVRTC_RGB4;
#elif UNITY_ANDROID
                    TextureFormat = TextureImporterFormat.ETC_RGB4;
#else
                TextureFormat = TextureImporterFormat.RGB24;
#endif
                }

                importer.textureFormat = TextureFormat;

                AssetDatabase.ImportAsset(path);
            }
        }
        AssetDatabase.Refresh();
    }


}
