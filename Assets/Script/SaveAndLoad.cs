using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;



public class SaveAndLoad : MonoBehaviour
{
    
    public string loadMapData = "";
    public string MapData;
    public Button sendButton;
    public TMP_Text chatLog;
    public InputField InputField;
    public string _title;

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";


    public Camera captureCamera; // ���� ī�޶�
    public Rect captureArea; // ĸ���� ����


    public EditManager manager;
    //public MapCreation MapCreation;

    //������ �� ������ ���۽� ���� ����
    //private SocketIoClient socketIoClient;


    // Start is called before the first frame update
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";
        _title = null;
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);





        //������ ����
        //socketIoClient = FindObjectOfType<SocketIoClient>();
    }

    public void Savedata()
    {
        
        _title = null;
        _title = InputField.text;
        SAVE_FILENAME = "/"+_title+".txt";
        if (_title != null && _title != "")
        {
            if (false == File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
            {
                var file = File.CreateText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
                file.Close();
            }


            StreamWriter sw = new StreamWriter(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            manager.ArrayTotile();
            MapData = "";

            //y��ǥ�� ���� ���� ���� ��
            for (int i = 0; i < manager._size * manager._size; i++)
            {
                if (i == (manager._size * manager._size) - 1)
                {
                    MapData = MapData + manager._yPos2[i] + manager.tileRot2[i];
                }
                else
                    MapData = MapData + manager._yPos2[i] + manager.tileRot2[i] + " ";
            }
            CaptureScreen();
            //������ �� ������ ����
            //MapCreation.makeCreation(_title, MapData);
            sw.WriteLine(MapData);
            sw.Flush();
            sw.Close();
            Debug.Log("���� �Ϸ�");
            Debug.Log(sw.ToString());
        }
        else
            Debug.Log("������ �Է��ϼ���");
    }




    public void LoadData()
    {
        _title = null;
        _title = InputField.text;
        SAVE_FILENAME = "/" + _title + ".txt";
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            manager.clear();
            StreamReader reader = new StreamReader(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            loadMapData = reader.ReadToEnd();
            reader.Close();

            Debug.Log("�ε� �Ϸ�");
            Debug.Log(loadMapData);
            manager.MapDatatoArray(loadMapData);
        }
        else
        {
            Debug.Log("���̺� ������ �����ϴ�.");
        }
    }

    void CaptureScreen()
    {
        // ī�޶��� ȭ���� Texture2D�� �о�ɴϴ�.
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        captureCamera.targetTexture = renderTexture;
        Texture2D imageTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = renderTexture;
        imageTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        imageTexture.Apply();
        captureCamera.targetTexture = null;
        RenderTexture.active = null;

        // Texture2D�� PNG �̹����� �����մϴ�.
        byte[] bytes = imageTexture.EncodeToPNG();
        string filePath = SAVE_DATA_DIRECTORY + _title + ".png"; // ������ ���� ���
        System.IO.File.WriteAllBytes(filePath, bytes);

        // �޸𸮿��� ������ Texture2D�� �����մϴ�.
        Destroy(imageTexture);
    }
    //public void CaptureScreen()
    //{
    //    // ī�޶��� ȭ���� Texture2D�� �о�ɴϴ�.
    //    RenderTexture renderTexture = new RenderTexture(Screen.height, Screen.height, 24);
    //    captureCamera.targetTexture = renderTexture;
    //    Texture2D imageTexture = new Texture2D(Screen.height, Screen.height, TextureFormat.RGB24, false);
    //    captureCamera.Render();
    //    RenderTexture.active = renderTexture;
    //    imageTexture.ReadPixels(new Rect(0, 0, Screen.height, Screen.height), 0, 0);
    //    imageTexture.Apply();
    //    captureCamera.targetTexture = null;
    //    RenderTexture.active = null;

    //    // Texture2D�� PNG �̹����� �����մϴ�.
    //    byte[] bytes = imageTexture.EncodeToPNG();
    //    string filePath = Application.dataPath + "/Save/" + _title + ".png"; // ������ ���� ���
    //    System.IO.File.WriteAllBytes(Application.dataPath + "/capturedImage.png", bytes);

    //    // �޸𸮿��� ������ Texture2D�� �����մϴ�.
    //    Destroy(imageTexture);
    //}

    //void CaptureScreen()
    //{
    //    // RenderTexture�� �����Ͽ� Ư�� ������ ������
    //    RenderTexture renderTexture = new RenderTexture((int)captureRect.width, (int)captureRect.height, 24);
    //    Camera.main.targetTexture = renderTexture;
    //    Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
    //    Camera.main.Render();
    //    RenderTexture.active = renderTexture;

    //    // ĸ���� ������ �о�ɴϴ�.
    //    screenshot.ReadPixels(captureRect, 0, 0);
    //    screenshot.Apply();

    //    Camera.main.targetTexture = null;
    //    RenderTexture.active = null;
    //    Destroy(renderTexture);

    //    // ĸ���� �̹����� PNG ���Ϸ� ����
    //    byte[] bytes = screenshot.EncodeToPNG();
    //    string filePath = SAVE_DATA_DIRECTORY + _title + ".png"; // ������ ���� ���
    //    System.IO.File.WriteAllBytes(filePath, bytes);

    //    // �޸𸮿��� ������ Texture2D�� ����
    //    Destroy(screenshot);

    //    Debug.Log("ĸ�� �Ϸ�!");
    //}
}
