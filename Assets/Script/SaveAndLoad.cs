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
            //CaptureScreen();
            CaptureScreenAndResize();
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

    void CaptureScreenAndResize()
    {
        // ���ϴ� ũ��� RenderTexture�� �����մϴ�.
        int captureWidth = 750; // ĸ���� ������ ���� ũ��
        int captureHeight = 750; // ĸ���� ������ ���� ũ��

        RenderTexture renderTexture = new RenderTexture(captureWidth, captureHeight, 24);

        // ī�޶� ����
        captureCamera.targetTexture = renderTexture;

        // ĸ���� ������ ũ�⸦ �����Ͽ� �������մϴ�.
        captureCamera.Render();

        // Texture2D�� ĸ���� ���� ũ��� �����մϴ�.
        Texture2D imageTexture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);

        // ĸ���� ������ ũ��� ��ġ�� �����Ͽ� ReadPixels�մϴ�.
        imageTexture.ReadPixels(new Rect(475, 130, captureWidth, captureHeight), 0, 0);
        imageTexture.Apply();

        // ī�޶� ���� �ʱ�ȭ
        captureCamera.targetTexture = null;

        // �޸𸮿��� ������ Texture2D�� �����մϴ�.
        Destroy(imageTexture);

        // ĸ���� �̹����� ���ϴ� ũ��� ���������մϴ�.
        Texture2D resizedTexture = ResizeTexture(imageTexture, 300, 300);

        // ��������� �̹����� PNG�� �����մϴ�.
        byte[] bytes = resizedTexture.EncodeToPNG();
        string filePath = SAVE_DATA_DIRECTORY + _title + ".png"; // ������ ���� ���
        System.IO.File.WriteAllBytes(filePath, bytes);

        // ��������� Texture2D�� �����մϴ�.
        Destroy(resizedTexture);
    }



    Texture2D ResizeTexture(Texture2D sourceTexture, int targetWidth, int targetHeight)
    {
        RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 24);
        Graphics.Blit(sourceTexture, rt);

        Texture2D result = new Texture2D(targetWidth, targetHeight);
        RenderTexture.active = rt;
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();
        RenderTexture.active = null;

        return result;
    }

}
