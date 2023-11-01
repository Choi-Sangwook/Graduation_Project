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


    public Camera captureCamera; // 메인 카메라
    public Rect captureArea; // 캡쳐할 영역


    public EditManager manager;
    //public MapCreation MapCreation;

    //서버로 맵 데이터 전송시 서버 연결
    //private SocketIoClient socketIoClient;


    // Start is called before the first frame update
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";
        _title = null;
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);





        //서버와 연결
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

            //y좌표만 따로 빼서 만든 거
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
            //서버로 맵 데이터 전송
            //MapCreation.makeCreation(_title, MapData);
            sw.WriteLine(MapData);
            sw.Flush();
            sw.Close();
            Debug.Log("저장 완료");
            Debug.Log(sw.ToString());
        }
        else
            Debug.Log("제목을 입력하세요");
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

            Debug.Log("로드 완료");
            Debug.Log(loadMapData);
            manager.MapDatatoArray(loadMapData);
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
    }

    void CaptureScreen()
    {
        // 카메라의 화면을 Texture2D로 읽어옵니다.
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        captureCamera.targetTexture = renderTexture;
        Texture2D imageTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = renderTexture;
        imageTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        imageTexture.Apply();
        captureCamera.targetTexture = null;
        RenderTexture.active = null;

        // Texture2D를 PNG 이미지로 저장합니다.
        byte[] bytes = imageTexture.EncodeToPNG();
        string filePath = SAVE_DATA_DIRECTORY + _title + ".png"; // 저장할 파일 경로
        System.IO.File.WriteAllBytes(filePath, bytes);

        // 메모리에서 생성한 Texture2D를 해제합니다.
        Destroy(imageTexture);
    }
    //public void CaptureScreen()
    //{
    //    // 카메라의 화면을 Texture2D로 읽어옵니다.
    //    RenderTexture renderTexture = new RenderTexture(Screen.height, Screen.height, 24);
    //    captureCamera.targetTexture = renderTexture;
    //    Texture2D imageTexture = new Texture2D(Screen.height, Screen.height, TextureFormat.RGB24, false);
    //    captureCamera.Render();
    //    RenderTexture.active = renderTexture;
    //    imageTexture.ReadPixels(new Rect(0, 0, Screen.height, Screen.height), 0, 0);
    //    imageTexture.Apply();
    //    captureCamera.targetTexture = null;
    //    RenderTexture.active = null;

    //    // Texture2D를 PNG 이미지로 저장합니다.
    //    byte[] bytes = imageTexture.EncodeToPNG();
    //    string filePath = Application.dataPath + "/Save/" + _title + ".png"; // 저장할 파일 경로
    //    System.IO.File.WriteAllBytes(Application.dataPath + "/capturedImage.png", bytes);

    //    // 메모리에서 생성한 Texture2D를 해제합니다.
    //    Destroy(imageTexture);
    //}

    //void CaptureScreen()
    //{
    //    // RenderTexture를 생성하여 특정 영역을 렌더링
    //    RenderTexture renderTexture = new RenderTexture((int)captureRect.width, (int)captureRect.height, 24);
    //    Camera.main.targetTexture = renderTexture;
    //    Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
    //    Camera.main.Render();
    //    RenderTexture.active = renderTexture;

    //    // 캡쳐할 영역을 읽어옵니다.
    //    screenshot.ReadPixels(captureRect, 0, 0);
    //    screenshot.Apply();

    //    Camera.main.targetTexture = null;
    //    RenderTexture.active = null;
    //    Destroy(renderTexture);

    //    // 캡쳐한 이미지를 PNG 파일로 저장
    //    byte[] bytes = screenshot.EncodeToPNG();
    //    string filePath = SAVE_DATA_DIRECTORY + _title + ".png"; // 저장할 파일 경로
    //    System.IO.File.WriteAllBytes(filePath, bytes);

    //    // 메모리에서 생성한 Texture2D를 해제
    //    Destroy(screenshot);

    //    Debug.Log("캡쳐 완료!");
    //}
}
