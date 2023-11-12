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
        ////��ũ�� �������� �ش� ���� �ֱ�
        folderPath = Application.dataPath + "/Save/";
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("������ ã�� �� �����ϴ�.");
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
            Debug.LogError("������ ã�� �� �����ϴ�.");
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

            Debug.Log("�ε� �Ϸ�");
            Debug.Log(loadMapData);
            EditManager.MapDatatoArray(loadMapData);
        }
        else
        {
            Debug.Log("���̺� ������ �����ϴ�.");
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
            Debug.Log("������ ���������� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("������ ������ �������� �ʽ��ϴ�.");
        }
    }


    // ����Ʈ���� Ư�� �ε����� ��� ��ư�� �����ϴ� �Լ�
    void RemoveToggleAtIndex(int index)
    {
        if (index >= 0 && index < toggleList.Count)
        {
            deleteFile();
            GameObject toggleToRemove = toggleList[index];
            Destroy(toggleToRemove.gameObject);  // ��� ��ư ���� ������Ʈ�� ����
            toggleList.RemoveAt(index);  // ����Ʈ���� ����
            maptitle = "";
            mapImage.sprite = original;
        }
    }

    // ����: ��ư�� ������ �� Ư�� �ε����� ��� ��ư ����
    public void RemoveToggleButtonOnClick()
    {
        RemoveToggleAtIndex(index);
    }

    void Update()
    {
        
    }
}
