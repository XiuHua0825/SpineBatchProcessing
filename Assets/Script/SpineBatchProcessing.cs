using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine;
using Spine.Unity;
using TexturePacker;

public class SpineBatchProcessing : MonoBehaviour
{
    public string[] SpineCharacterNames;
    private string rootPath = "Assets/Resources/Spines/";       // 放素材和檔案的根目錄
    private string spinePath;                                   // 動畫所需檔案的資料夾
    private string imagesSourcePath;                            // 拆解素材的資料夾: Assets/Resources/動畫名/images/..
    private string atlasTxtFileName;                            // 打包後的AtlasTxt的檔案名 
    private string atlasPngFileName;                            // 打包後的AtlasPng的檔案名 
    private string atlasTxtFilePath;                            // 打包後的AtlasTxT的檔案位置(含檔名): Assets/Resources/動畫名/..
    private string atlasPngFilePath;                            // 打包後的AtlasPng的檔案位置(含檔名): Assets/Resources/動畫名/..
    private int textureSize = 1024;
    private int padding = 2;
    private float scale = 1;
    public bool createLogFile = false;

    void Start(){
        foreach (var item in SpineCharacterNames)
        {
            spinePath = rootPath + item + "/";
            imagesSourcePath = spinePath + "images";
            atlasTxtFileName = item + ".atlas.txt";
            atlasPngFileName = item + ".png";
            atlasTxtFilePath = spinePath + atlasTxtFileName;
            atlasPngFilePath = spinePath + atlasPngFileName;

            //Step1. create .atlas.txt
            Packer packer = new Packer();
            packer.Process(imagesSourcePath, "*", textureSize, padding, scale, createLogFile);
            packer.SaveAtlasses(atlasTxtFilePath, atlasPngFileName);
            AssetDatabase.Refresh();

            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(atlasPngFilePath);
            importer.maxTextureSize = 1024;
            importer.crunchedCompression = true;
            importer.compressionQuality = 0;
            importer.SaveAndReimport();

            //Step2. create Json

            //Step4. create chararcter spine skeleton animation
            CreateCharacter(item);
        }
    }

    public void CreateCharacter(string _name)   //_name = 素材檔案的名字，素材放在Resources/Spines資料夾底
    {
        SkeletonAnimation playerAnim;
        SkeletonDataAsset playerData;
        SpineAtlasAsset atlasdata;
        string name = "Spines/" + _name + "/" + _name;

        atlasdata = ScriptableObject.CreateInstance<SpineAtlasAsset> ();
        playerData = ScriptableObject.CreateInstance<SkeletonDataAsset> ();
        playerData.fromAnimation = new string[0];
        playerData.toAnimation = new string[0];
        playerData.duration = new float[0];
        playerData.scale = 0.01f;
        playerData.defaultMix = 0.15f;
        
        atlasdata.atlasFile = (TextAsset)Resources.Load (name + ".atlas", typeof(TextAsset));

        Material[] materials = new Material[1];
        materials [0] = new Material (Shader.Find("Spine/Skeleton"));
        Texture aa = (Texture)Resources.Load (name, typeof(Texture2D));
        materials [0].mainTexture = aa;

        atlasdata.materials = new Material[1];
        atlasdata.materials = materials;

        playerData.atlasAssets = new AtlasAssetBase[1];
        playerData.atlasAssets[0] = atlasdata;
        playerData.skeletonJSON = (TextAsset)Resources.Load (name, typeof(TextAsset));

        GameObject player = new GameObject();
        player.name = name;
        player.transform.localPosition = Vector3.zero;
        player.transform.localScale = new Vector3 (1f, 1f, 1f);

        playerAnim = player.AddComponent<SkeletonAnimation>();
        playerAnim.skeletonDataAsset = playerData;
        playerAnim.calculateTangents = true;

        playerAnim.AnimationState.SetAnimation(0, "idle", true);
        playerAnim.loop = true;
        Debug.LogFormat("--------------產生{0}Spine動畫角色完成--------------", name);
    }
}
