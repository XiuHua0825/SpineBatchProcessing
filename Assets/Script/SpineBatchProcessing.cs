﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using TexturePacker;

public class SpineBatchProcessing : MonoBehaviour
{
    public string[] SpineCharacterNames;
    private string imageSourcePath; // 拆解素材的資料夾
    private int textureSize = 1024;
    private int padding = 1;
    public bool IsShowDebug = false;


    /// 拆分部位位置: Assets/Resources/動畫名/images/..
    /// 產生的Atlas位置: Assets/Resources/動畫名/..
    /// 產稱的Spine動畫相關物件: Assets/Resources/動畫名/..

    void Start(){
        foreach (var item in SpineCharacterNames)
        {
            //Step1. create Atlas
            Packer packer = new Packer();
            imageSourcePath = "Assets/Resources/Spines/" + item + "/images";
            packer.Process(imageSourcePath, "*", textureSize, padding, IsShowDebug);
            packer.SaveAtlasses("Assets/Resources/Spines/" + item + "/" + item + ".atlas");
            Debug.LogFormat("Set {0} atlas done.", item + ".atlas");

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
        materials [0] = new Material (Shader.Find ("Spine/Skeleton"));
        Texture aa = (Texture)Resources.Load (name, typeof(Texture2D));
        materials [0].mainTexture = aa;

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

        playerAnim.loop = true;
        Debug.Log("Create character start");
    }
}