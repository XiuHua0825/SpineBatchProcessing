# SpineBatchProcessing
在Unity內動態生成atlas圖片與文字檔，並產生Spine動畫角色

### 範例執行說明
1. 範例角色名稱：Nimffer_01、Nimffer_02
2. 把拆解的素材部位放到 Assets/Resources/角色名稱/images/
3. 把角色的.json放到 Assets/Resources/角色名稱/
4. 到 Hierarchy/SpineManager，在 `SpineBatchProcessing` 的 `Spine Character Names` 填入角色名稱
5. 運行場景，產生兩隻角色
---
### 專案設定
- Spine version: 3.8.75
  - 把 `SkeletonJson.cs`、`SkeletonBinary.cs` 裡面對於 3.8.75的版本限制註解掉
  - 編輯 `Spine/Skeleton` 的 `Shader`，把 `_StraightAlphaInput` 的預設值從 0 變成 1
- Spine-Unity version: spine-unity 3.8 for Unity 2017.1-2020.3
- System.Drawing.dll: 如果沒有的話，少了這個PackAtlas.cs會無法使用
---
### 執行方式
1. 進入點
```cs
SpineBatchProcessing.cs
  Start(){...}
```
2. 執行產生 **合併的大圖.png檔** 和 **.atlas.txt檔** 
```cs
SpineBatchProcessing.cs
  Start(){
    ...
    Packer packer = new Packer();
    imageSourcePath = "Assets/Resources/path";
    packer.Process(imageSourcePath, "*", textureSize, padding, IsShowDebug);
    packer.SaveAtlasses("path/fileName.png");
    ...
  }
```
```cs
PackAtlas.cs
  Process(){...}
  SaveAtlasses(){...}
```
3. 重新讀取Asset，導入產生的檔案
4. 取得並導入 **.json檔** 
5. 執行產生 **Spine動畫角色** 
```cs
SpineBatchProcessing.cs
  Start(){
    ...
    CreateCharacter();
    ...
  }
  CreateCharacter(){
    ...
    // 產生對應的 **AtlasData** ，指定 **.atlas.txt** 
    // 產生對應的 **Material** ，指定 **shader**、指定 **texture** ，並賦予前面產生的 **AtlasData** 
    // 產生對應的 **SkeletonData ** ，指定前面產生的 **AtlasData** 、指定 **.json** 
    // 產生 **角色的空物件** ，指定座標、縮放
    // 產生對應的 **SkeletonAnimation** ，指定前面產生的 **SkeletonData** ，並賦予給前面產生的 **角色的空物件** 
  }
```
6. 設定預設動畫
```cs
  playerAnim.AnimationState.SetAnimation(0, "idle", true);
```
---
### 其他
- 註解掉Spine的版本限制後，測試使用尚無影響，若有問題可能要考慮使用其他版本的Spine和spine-unity
- 每隻角色必須對應一個json檔，可重複使用第一組製作的檔案，但要更改裡面的部位名稱，以符合對應角色的素材
- 必須確定要重複使用的這些角色，拆解出的 **部位數量(bones、slots)** 、產生的 **動畫動作(animations)** 、和 **動畫動作表演方式(keyframe)** 需都一致，若有任何不同就需要產生新的.json
---
### 相關連結
[Download spine-unity.unitypackage](http://zh.esotericsoftware.com/spine-unity-download/)  
[How to use System.Drawing in Unity](https://blog.csdn.net/qq_33869036/article/details/106743924)  
[PS > Spine > Unity](https://hackmd.io/@DwzSfM4EQ4Ory_RDx-zH3w/r1uPzJhiV?type=view#%E8%BC%B8%E5%87%BA)  
[Run unity-spine](http://zh.esotericsoftware.com/spine-unity)  
