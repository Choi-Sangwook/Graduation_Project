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
            //CaptureScreen();
            CaptureScreenAndResize();
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

    void CaptureScreenAndResize()
    {
        // 원하는 크기로 RenderTexture를 생성합니다.
        int captureWidth = 750; // 캡쳐할 영역의 가로 크기
        int captureHeight = 750; // 캡쳐할 영역의 세로 크기

        RenderTexture renderTexture = new RenderTexture(captureWidth, captureHeight, 24);

        // 카메라 설정
        captureCamera.targetTexture = renderTexture;

        // 캡쳐할 영역의 크기를 조정하여 렌더링합니다.
        captureCamera.Render();

        // Texture2D를 캡쳐할 영역 크기로 생성합니다.
        Texture2D imageTexture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);

        // 캡쳐할 영역의 크기와 위치를 지정하여 ReadPixels합니다.
        imageTexture.ReadPixels(new Rect(475, 130, captureWidth, captureHeight), 0, 0);
        imageTexture.Apply();

        // 카메라 설정 초기화
        captureCamera.targetTexture = null;

        // 메모리에서 생성한 Texture2D를 해제합니다.
        Destroy(imageTexture);

        // 캡쳐한 이미지를 원하는 크기로 리사이즈합니다.
        Texture2D resizedTexture = ResizeTexture(imageTexture, 300, 300);

        // 리사이즈된 이미지를 PNG로 저장합니다.
        byte[] bytes = resizedTexture.EncodeToPNG();
        string filePath = SAVE_DATA_DIRECTORY + _title + ".png"; // 저장할 파일 경로
        System.IO.File.WriteAllBytes(filePath, bytes);

        // 리사이즈된 Texture2D를 해제합니다.
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
