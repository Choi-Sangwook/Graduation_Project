using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    public ToggleButtonController toggleButtonController;
    
    public GameObject TileMap;

    public GameObject tile;
    public GameObject Wall;

    public  int childCount;

    public GameObject MainUI;
    public GameObject EditUI;

    [Header("Selected Prefeb")]
    public GameObject selectedObj;

    [Header("Placed Prdfeb")]
    public GameObject Pipe;
    public GameObject L_Pipe;
    public GameObject T_Pipe;

    [Header("Prefeb Material")]
    public Material selected_Mat;
    public Material original_Mat;
    public Material build_Mat;
    public Material cantBuild_Mat;

    [Header("Title InputField")]
    public InputField inputField;

    public GameObject[,] tileArray;
    public int _size = 15;
    public float interval = 3.0f;
    public int bottomNum = 0;
    public int wallNum = 0;

    //yPos
    public int[,] _yPos;
    public int[] _yPos2;

    //RotValues
    public int[,] tileRot;
    public int[] tileRot2;

    public Text PCount;
    public Text L_PCount;
    public Text T_PCount;

    public int PipeCount = 0;
    public int L_PipeCount = 0;
    public int T_PipeCount = 0;

    public MapListManager maplistmanager;

    public Camera mainCamera;
    public Camera subCamera;
    public Toggle isSubCamera;

    public LayerMask UI;

    public Rect limitedArea = new(450, 150, 800, 800); // 특정 화면 영역을 정의하는 Rect

    public GameObject spawnPref;

    public void OnServerInitialized(int size)
    {
        MainUI.SetActive(false);
        EditUI.SetActive(true);
        ToggleCamera();
        if (size % 2 == 0)
            return;
        _yPos = new int[size, size];
        _yPos2 = new int[size * size];
        tileRot = new int[size, size];
        tileRot2 = new int[size * size];
        _size = size;
        spawnPref = null;
        GenerateCube();
    }

    void GenerateCube()
    {
        for (int x = 0; x < _size; x++)
        {
            for (int z = 0; z < _size; z++)
            {
                _yPos[x, z] = 1;
                tileRot[x, z] = 0;
            }
        }
        PipeCount = 0;
        L_PipeCount = 0;
        T_PipeCount = 0;
        chageCount();
        initCube();
    }

    public void initCube()
    {
        wallNum = (_size * _size) - bottomNum;
        make2ArrayCube(TileMap);
    }


    public void MapDatatoArray(string MapData)
    {
        string[] _ystring = MapData.Split(" ");
        Debug.Log("전" + MapData);

        int n = 0;
        for (int x = 0; x < _size; x++)
        {
            for (int z = 0; z < _size; z++)
            {
                if (int.Parse(_ystring[n]) < 0)
                {
                    int _ypos = int.Parse(_ystring[n]) / 10;
                    int _yRot = Mathf.Abs(int.Parse(_ystring[n]) % 10);
                    Vector3 pos = new Vector3(tileArray[x, z].transform.position.x, 1.25f, tileArray[x, z].transform.position.z);
                    _yPos[x, z] = _ypos;

                    //y좌표로 불러올때
                    //PipeLine
                    if (_ypos == -1)
                    {
                        GameObject prefab = Instantiate(Pipe, pos, Quaternion.identity);
                        prefab.transform.SetParent(tileArray[x, z].transform.Find("Quad").gameObject.transform, true);
                        prefab.transform.rotation = Quaternion.Euler(Pipe.transform.rotation.eulerAngles);
                        //tileArray[x, z].transform.position = new Vector3(tileArray[x, z].transform.position.x, 0, tileArray[x, z].transform.position.z);
                        tileArray[x, z].transform.localEulerAngles = new Vector3(0, 90 * _yRot, 0);
                        tileRot[x, z] = _yRot;
                        //ChangeObjectPosition((int)tileArray[x, z].transform.position.x, (int)tileArray[x, z].transform.position.z, pos);
                    }
                    //L-Pipe
                    if (_ypos == -2)
                    {
                        GameObject prefab = Instantiate(L_Pipe, pos, Quaternion.identity);
                        prefab.transform.SetParent(tileArray[x, z].transform.Find("Quad").gameObject.transform, true);
                        prefab.transform.rotation = Quaternion.Euler(L_Pipe.transform.rotation.eulerAngles);
                        //tileArray[x, z].transform.position = new Vector3(tileArray[x, z].transform.position.x, 0, tileArray[x, z].transform.position.z);
                        tileArray[x, z].transform.localEulerAngles = new Vector3(0, 90 * _yRot, 0);
                        tileRot[x, z] = _yRot;
                        //ChangeObjectPosition((int)tileArray[x, z].transform.position.x, (int)tileArray[x, z].transform.position.z, pos);
                    }
                    //T-Pipe
                    if (_ypos == -3)
                    {
                        GameObject prefab = Instantiate(T_Pipe, pos, Quaternion.identity);
                        prefab.transform.SetParent(tileArray[x, z].transform.Find("Quad").gameObject.transform, true);
                        prefab.transform.rotation = Quaternion.Euler(T_Pipe.transform.rotation.eulerAngles);
                        //tileArray[x, z].transform.position = new Vector3(tileArray[x, z].transform.position.x, 0, tileArray[x, z].transform.position.z);
                        tileArray[x, z].transform.localEulerAngles = new Vector3(0, 90 * _yRot, 0);
                        tileRot[x, z] = _yRot;
                        //ChangeObjectPosition((int)tileArray[x, z].transform.position.x, (int)tileArray[x, z].transform.position.z, pos);
                    }
                    updateCount(_ypos,_ypos);
                }
                n++;
            }
        }
    }

    //Reset Editer
    public void clear()
    {
        for (int x = 0; x < _size; x++)
        {
            for (int z = 0; z < _size; z++)
            {
                Destroy(tileArray[x, z]);
            }
        }
        GenerateCube();
    }

    public void home()
    {
        for (int x = 0; x < _size; x++)
        {
            for (int z = 0; z < _size; z++)
            {
                Destroy(tileArray[x, z]);
            }
        }
        PipeCount = 0;
        L_PipeCount = 0;
        T_PipeCount = 0;
        MainUI.SetActive(true);
        EditUI.SetActive(false);
        maplistmanager.reLoad();
    }

    public void make2ArrayCube(GameObject maze)
    {
        tileArray = new GameObject[_size, _size];
        for (int x = 0; x < _size; x++)
        {
            for (int z = 0; z < _size; z++)
            {
                Vector3 pos = new Vector3(x, 1, z);
                if (x == 0 || x == _size - 1 || z == 0 || z == _size - 1)
                {
                    if (x == z || (x==0&&z==_size-1) || (x==_size-1&&z==0))
                    {
                        tileArray[x, z] = new GameObject("wall");
                        tileArray[x, z].transform.position = pos;
                        tileArray[x, z].transform.SetParent(maze.transform, false);
                    }
                    else if (z == 0 && x > 0)
                    {
                        tileArray[x, z] = Instantiate(Wall, pos, Quaternion.identity);
                        tileArray[x, z].transform.localEulerAngles = new Vector3(0, 270, 0);
                        tileArray[x, z].transform.SetParent(maze.transform, false);
                    }
                    else if (x > 0 && z == _size - 1)
                    {
                        tileArray[x, z] = Instantiate(Wall, pos, Quaternion.identity);
                        tileArray[x, z].transform.localEulerAngles = new Vector3(0, 90, 0);
                        tileArray[x, z].transform.SetParent(maze.transform, false);
                    }
                    else if (x == 14 && z > 0)
                    {
                        tileArray[x, z] = Instantiate(Wall, pos, Quaternion.identity);
                        tileArray[x, z].transform.localEulerAngles = new Vector3(0, 180, 0);
                        tileArray[x, z].transform.SetParent(maze.transform, false);
                    }
                    else
                    {
                        tileArray[x, z] = Instantiate(Wall, pos, Quaternion.identity);
                        tileArray[x, z].transform.SetParent(maze.transform, false);
                    }
                }
                else
                {
                    tileArray[x, z] = Instantiate(tile, pos, Quaternion.identity);
                    tileArray[x, z].transform.SetParent(maze.transform, false);
                }
            }
        }
    }



    public void clicked(GameObject obj)
    {
        if (!toggleButtonController.isSelectBtnOn)
        {
            if (toggleButtonController.prefabToSpawn != null)
            {
                if (_yPos[(int)obj.transform.position.x, (int)obj.transform.position.z] > 0)
                {
                    Debug.Log(toggleButtonController.prefabToSpawn.name);
                    GameObject prefab = Instantiate(toggleButtonController.prefabToSpawn, new Vector3(obj.transform.position.x,1.25f,obj.transform.position.z), Quaternion.identity);
                    prefab.transform.SetParent(obj.transform, true);
                    //obj.transform.position = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
                    prefab.transform.rotation = Quaternion.Euler(toggleButtonController.prefabToSpawn.transform.rotation.eulerAngles);
                    _yPos[(int)obj.transform.position.x, (int)obj.transform.position.z] = toggleButtonController.profileNum;
                    updateCount(toggleButtonController.profileNum,-1);
                    ChangeObjectPosition((int)obj.transform.position.x, (int)obj.transform.position.z, obj.transform.position);
                    Debug.Log(obj.transform.position);
                }
            }
        }
        else
        {
            if (_yPos[(int)obj.transform.position.x, (int)obj.transform.position.z] != 1)
            {
                if (selectedObj == null || (selectedObj.transform.position != obj.transform.position && selectedObj != null))
                {
                    if (selectedObj != null)
                    {
                        childCount = selectedObj.transform.GetChild(0).transform.childCount;
                        for(int i = 0; i < childCount; i++)
                        {
                            selectedObj.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = original_Mat;
                        }
                        
                    }
                    selectedObj = obj;
                    childCount = selectedObj.transform.GetChild(0).transform.childCount;
                    for (int i = 0; i < childCount; i++)
                    {
                        selectedObj.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = selected_Mat;
                    }
                    
                }
                else
                {
                    childCount = selectedObj.transform.GetChild(0).transform.childCount;
                    for (int i = 0; i < childCount; i++)
                    {
                        selectedObj.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = original_Mat;
                        
                    }
                    selectedObj = null;
                }
            }
        }
        chageCount();
    }

    public void objRot()
    {
        if (toggleButtonController.isSelectBtnOn)
        {
            Vector3 currentRotation = selectedObj.transform.rotation.eulerAngles;

            
            currentRotation.y += 90f;
            tileRot[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z] += 1;

            
            selectedObj.transform.rotation = Quaternion.Euler(currentRotation);
        }
    }

    public void objDelete()
    {
        if (toggleButtonController.isSelectBtnOn)
        {
            if (selectedObj != null)
            {
                if (_yPos[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z] != 1)
                {
                    updateCount(_yPos[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z], 1);
                    Destroy(selectedObj.transform.GetChild(0).gameObject);
                    //selectedObj.transform.position = new Vector3(selectedObj.transform.position.x, 1, selectedObj.transform.position.z);
                    tileArray[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z].transform.position = new Vector3(selectedObj.transform.position.x, 1, selectedObj.transform.position.z);
                    _yPos[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z] = 1;
                    tileRot[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z] = 0;
                    selectedObj = null;

                }
            }


        }
    }

    public void ArrayTotile()
    {
        int n = 0;
        for (int x = 0; x < _size; x++)
        {
            for (int z = 0; z < _size; z++)
            {
                _yPos2[n] = _yPos[x, z];
                tileRot2[n] = tileRot[x, z];
                n++;
            }
        }
    }

    void ChangeObjectPosition(int x, int y, Vector3 newPosition)
    {
        if (x >= 0 && x < tileArray.GetLength(0) && y >= 0 && y < tileArray.GetLength(1))
        {
            tileArray[x, y].transform.position = newPosition;
            Debug.Log("Maze" + x + "," + y + ":" + tileArray[x, y].transform.position);
        }
    }

    public int[] FindObjectIndexAtPosition(Vector3 position)
    {
        int[] xy = new int[2];
        xy[0] = -1;
        xy[1] = -1;

        for (int i = 0; i < tileArray.GetLength(0); i++)
        {
            for (int j = 0; j < tileArray.GetLength(1); j++)
            {
                if (tileArray[i, j] != null && tileArray[i, j].transform.position == position)
                {
                    xy[0] = i;
                    xy[1] = j;
                    return xy;
                }
            }
        }

        return xy; 
    }

    public void chageCount()
    {
        PCount.text = (PipeCount*0.25).ToString();
        L_PCount.text = L_PipeCount.ToString();
        T_PCount.text = T_PipeCount.ToString();
    }

    public void updateCount(int n,int i)
    {
        if (i < 0)
        {
            switch (n)
            {
                case -1:
                    PipeCount++;
                    break;
                case -2:
                    L_PipeCount++;
                    break;
                case -3:
                    T_PipeCount++;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (n)
            {
                case -1:
                    PipeCount--;
                    break;
                case -2:
                    L_PipeCount--;
                    break;
                case -3:
                    T_PipeCount--;
                    break;
                default:
                    break;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        MainUI.SetActive(true);
        EditUI.SetActive(false);
        ToggleCamera();
    }

    public void ToggleCamera()
    {
        // 각 카메라의 활성화 여부 설정
        mainCamera.enabled = !isSubCamera.isOn;
        subCamera.enabled = isSubCamera.isOn;
    }


    void Update()
    {
        // 마우스 왼쪽 버튼을 클릭할 때
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치를 가져와서 화면 좌표로 변환
            Vector3 mousePosition = Input.mousePosition;

            // 특정 화면 영역 내에서만 레이를 쏘기
            if (IsMousePositionInLimitedArea(mousePosition))
            {
                // 마우스 위치에서 레이를 쏘기
                Ray ray = (isSubCamera.isOn ? subCamera.ScreenPointToRay(Input.mousePosition) : mainCamera.ScreenPointToRay(Input.mousePosition));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    clicked(hit.collider.gameObject);
                    Debug.Log(hit.transform.gameObject.transform.position);
                    Debug.Log("레이가 오브젝트에 맞았습니다.");
                }
            }
        }
        //선택 버튼이 클릭되지 않고 생성할 프리펩이 지정되있을때
        if (spawnPref!=null)
        {
            // 마우스 위치를 가져와서 화면 좌표로 변환
            Vector3 mousePosition = Input.mousePosition;

            // 특정 화면 영역 내에서만 레이를 쏘기
            if (IsMousePositionInLimitedArea(mousePosition))
            {
                // 마우스 위치에서 레이를 쏘기
                Ray ray = (isSubCamera.isOn ? subCamera.ScreenPointToRay(Input.mousePosition) : mainCamera.ScreenPointToRay(Input.mousePosition));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    Vector3 _location = hit.collider.gameObject.transform.position;
                    childCount = spawnPref.transform.childCount;
                    Debug.Log(childCount);
                    if (_yPos[(int)_location.x, (int)_location.z] > 0)
                    {
                        for (int i = 0; i < childCount; i++)
                        {
                            spawnPref.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = build_Mat;
                            Debug.Log("초록");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < childCount; i++)
                        {
                            spawnPref.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = cantBuild_Mat;
                            Debug.Log("빨강");
                        }
                    }
                    _location.Set(_location.x, 1.25f, _location.z);
                    spawnPref.transform.position = _location;
                    Debug.Log(hit.transform.gameObject.transform.position);
                    Debug.Log("레이가 오브젝트에 맞았습니다.");
                }
            }
        }
        chageCount();
    }

    bool IsMousePositionInLimitedArea(Vector3 mousePosition)
    {
        // 특정 화면 영역에 해당하는지 여부를 판단
        return limitedArea.Contains(mousePosition);
    }
}
