using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class ChangeTextureType : AssetPostprocessor
{
    void OnPreprocessTexture()
    {

        if (assetPath.Contains("Screenshots"))
        {
            TextureImporter importer = assetImporter as TextureImporter;
            importer.textureType = TextureImporterType.Advanced;
            importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            importer.isReadable = true;
            importer.filterMode = FilterMode.Point;
            importer.npotScale = TextureImporterNPOTScale.None;

            Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
            if (asset)
            {
                EditorUtility.SetDirty(asset);
            }
            else
            {
                importer.textureType = TextureImporterType.Advanced;
            }
        }

    }
}
