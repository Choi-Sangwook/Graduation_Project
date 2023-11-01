using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;
using System;

public class MapListManager : MonoBehaviour
{
    // Start is called before the first frame update

    public MapListManager manager;
    public GameObject contents;
    public GameObject text;
    public ToggleGroup group;
    public Image mapImage;
    public ToggleButtonController toggleButtonController;
    string folderPath;
    string maptitle;
    private bool isSelectBtnOn;

    void Start()
    {
        ////스크롤 컨텐츠에 해당 제목 넣기
        //for (int i = 0; i < 3; i++)
        //{
        //    text.GetComponent<TMP_Text>().text = "text"+i;
        //    Instantiate(text, contents.transform.position, Quaternion.identity).transform.SetParent(contents.transform);
        //}
        folderPath = Application.dataPath + "/Save/";
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("폴더를 찾을 수 없습니다.");
            return;
        }

        string[] fileNames = GetFileNamesInFolder(folderPath);
        foreach (string fileName in fileNames)
        {
            text.transform.Find("Text").GetComponent<Text>().text = fileName;
            text.GetComponent<Toggle>().group = group;
            text.GetComponent<MapTitle>().title = fileName;
            text.GetComponent<MapTitle>().manager = manager;
            Instantiate(text, contents.transform.position, Quaternion.identity).transform.SetParent(contents.transform);
        }


    }



    string[] GetFileNamesInFolder(string path)
    {
        if (Directory.Exists(path))
        {
            string[] filePaths = Directory.GetFiles(path);
            
            string[] fileNames = filePaths
                .Where(filePath => !filePath.EndsWith(".meta", StringComparison.OrdinalIgnoreCase) &&
                                   !filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                .Select(filePath => Path.GetFileName(filePath))
                .ToArray();


            return fileNames;
        }

        return new string[0];
    }

    //private void Update()
    //{
    //    if (toggleButtonController.isSelectBtnOn)
    //    {
    //        maptitle = toggleButtonController.title;
    //        //mapImage.sprite = LoadImage();
    //    }
    //}

    public void OnclickedTitle(string title)
    {
        maptitle = System.IO.Path.GetFileNameWithoutExtension(title);
        Debug.Log(maptitle);
        mapImage.sprite = LoadImage();
    }

    public Sprite LoadImage()
    {
        byte[] imageData = File.ReadAllBytes(folderPath+"/"+ maptitle+".png");
        Texture2D imageTexture = new Texture2D(2, 2);
        imageTexture.LoadImage(imageData);
        Sprite sprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), Vector2.one * 0.5f);

        return sprite;
    }
}
