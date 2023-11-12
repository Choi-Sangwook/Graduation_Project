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

    public EditManager EditManager;
    public MapListManager manager;
    public GameObject contents;
    public GameObject text;
    public ToggleGroup group;
    public Image mapImage;
    public ToggleButtonController toggleButtonController;
    string folderPath;
    string maptitle;
    string SAVE_FILENAME;
    public string loadMapData = "";
    private bool isSelectBtnOn;
    public Sprite original;

    public int index;

    private List<GameObject> toggleList = new List<GameObject>();

    void Start()
    {
        ////스크롤 컨텐츠에 해당 제목 넣기
        folderPath = Application.dataPath + "/Save/";
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("폴더를 찾을 수 없습니다.");
            return;
        }
        int n = 0;
        string[] fileNames = GetFileNamesInFolder(folderPath);
        foreach (string fileName in fileNames)
        {
            text.transform.Find("Text").GetComponent<Text>().text = fileName;
            text.GetComponent<Toggle>().group = group;
            text.GetComponent<MapTitle>().title = fileName;
            text.GetComponent<MapTitle>().manager = manager;
            text.GetComponent<MapTitle>().index = n++;
            GameObject newToggle = Instantiate(text, contents.transform.position, Quaternion.identity);
            newToggle.transform.SetParent(contents.transform);
            toggleList.Add(newToggle);
        }
    }

    public void reLoad()
    {
        toggleList.Clear();
        foreach (Transform child in contents.transform)
        {
            Destroy(child.gameObject);
        }
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("폴더를 찾을 수 없습니다.");
            return;
        }
        int n = 0;
        string[] fileNames = GetFileNamesInFolder(folderPath);
        foreach (string fileName in fileNames)
        {
            text.transform.Find("Text").GetComponent<Text>().text = fileName;
            text.GetComponent<Toggle>().group = group;
            text.GetComponent<MapTitle>().title = fileName;
            text.GetComponent<MapTitle>().manager = manager;
            text.GetComponent<MapTitle>().index = n++;
            GameObject newToggle = Instantiate(text, contents.transform.position, Quaternion.identity);
            newToggle.transform.SetParent(contents.transform);
            toggleList.Add(newToggle);
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


    public void LoadMap()
    {
        SAVE_FILENAME = "/" + maptitle + ".txt";
        if (File.Exists(folderPath + SAVE_FILENAME))
        {
            EditManager.OnServerInitialized(15);
            StreamReader reader = new StreamReader(folderPath + SAVE_FILENAME);
            loadMapData = reader.ReadToEnd();
            reader.Close();

            Debug.Log("로드 완료");
            Debug.Log(loadMapData);
            EditManager.MapDatatoArray(loadMapData);
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
    }

    public void deleteFile()
    {
        SAVE_FILENAME = "/" + maptitle;
        if (File.Exists(folderPath + SAVE_FILENAME + ".txt"))
        {
            File.Delete(folderPath + SAVE_FILENAME + ".txt");
            File.Delete(folderPath + SAVE_FILENAME + ".txt.meta");
            File.Delete(folderPath + SAVE_FILENAME + ".png");
            File.Delete(folderPath + SAVE_FILENAME + ".png.meta");
            Debug.Log("파일이 성공적으로 삭제되었습니다.");
        }
        else
        {
            Debug.Log("삭제할 파일이 존재하지 않습니다.");
        }
    }


    // 리스트에서 특정 인덱스의 토글 버튼을 제거하는 함수
    void RemoveToggleAtIndex(int index)
    {
        if (index >= 0 && index < toggleList.Count)
        {
            deleteFile();
            GameObject toggleToRemove = toggleList[index];
            Destroy(toggleToRemove.gameObject);  // 토글 버튼 게임 오브젝트를 제거
            toggleList.RemoveAt(index);  // 리스트에서 제거
            maptitle = "";
            mapImage.sprite = original;
        }
    }

    // 예시: 버튼을 눌렀을 때 특정 인덱스의 토글 버튼 제거
    public void RemoveToggleButtonOnClick()
    {
        RemoveToggleAtIndex(index);
    }

    void Update()
    {
        
    }
}
