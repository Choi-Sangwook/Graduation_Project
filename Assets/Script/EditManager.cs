using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    public ToggleButtonController toggleButtonController;
    
    public GameObject TileMap;

    public GameObject tile;

    public  int childCount;

    [Header("Selected Prefeb")]
    public GameObject selectedObj;

    [Header("Placed Prdfeb")]
    public GameObject Pipe;
    public GameObject L_Pipe;
    public GameObject T_Pipe;

    [Header("Prefeb Material")]
    public Material selected_Mat;
    public Material original_Mat;

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

    public void OnServerInitialized(int size)
    {
        if (size % 2 == 0)
            return;
        _yPos = new int[size, size];
        _yPos2 = new int[size * size];
        tileRot = new int[size, size];
        tileRot2 = new int[size * size];
        _size = size;
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
                    Vector3 pos = new Vector3(tileArray[x, z].transform.position.x, -1, tileArray[x, z].transform.position.z);
                    _yPos[x, z] = _ypos;

                    //y좌표로 불러올때
                    //PipeLine
                    if (_ypos == -1)
                    {
                        Instantiate(Pipe, tileArray[x, z].transform.position, Quaternion.identity).transform.SetParent(tileArray[x, z].transform.Find("Quad").gameObject.transform, true);
                        //tileArray[x, z].transform.position = new Vector3(tileArray[x, z].transform.position.x, 0, tileArray[x, z].transform.position.z);
                        tileArray[x, z].transform.localEulerAngles = new Vector3(0, 90 * _yRot, 0);
                        tileRot[x, z] = _yRot;
                        //ChangeObjectPosition((int)tileArray[x, z].transform.position.x, (int)tileArray[x, z].transform.position.z, pos);
                    }
                    //L-Pipe
                    if (_ypos == -2)
                    {
                        Instantiate(L_Pipe, tileArray[x, z].transform.position, Quaternion.identity).transform.SetParent(tileArray[x, z].transform.Find("Quad").gameObject.transform, true);
                        //tileArray[x, z].transform.position = new Vector3(tileArray[x, z].transform.position.x, 0, tileArray[x, z].transform.position.z);
                        tileArray[x, z].transform.localEulerAngles = new Vector3(0, 90 * _yRot, 0);
                        tileRot[x, z] = _yRot;
                        //ChangeObjectPosition((int)tileArray[x, z].transform.position.x, (int)tileArray[x, z].transform.position.z, pos);
                    }
                    //T-Pipe
                    if (_ypos == -3)
                    {
                        Instantiate(T_Pipe, tileArray[x, z].transform.position, Quaternion.identity).transform.SetParent(tileArray[x, z].transform.Find("Quad").gameObject.transform, true);
                        //tileArray[x, z].transform.position = new Vector3(tileArray[x, z].transform.position.x, 0, tileArray[x, z].transform.position.z);
                        tileArray[x, z].transform.localEulerAngles = new Vector3(0, 90 * _yRot, 0);
                        tileRot[x, z] = _yRot;
                        //ChangeObjectPosition((int)tileArray[x, z].transform.position.x, (int)tileArray[x, z].transform.position.z, pos);
                    }
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
        initCube();
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

                    tileArray[x, z] = new GameObject("wall");
                    tileArray[x, z].transform.position = pos;
                    tileArray[x, z].transform.SetParent(maze.transform, false);
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
                    GameObject prefab = Instantiate(toggleButtonController.prefabToSpawn, obj.transform.position, Quaternion.identity);
                    prefab.transform.SetParent(obj.transform, true);
                    //obj.transform.position = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
                    prefab.transform.rotation = Quaternion.Euler(toggleButtonController.prefabToSpawn.transform.rotation.eulerAngles);
                    _yPos[(int)obj.transform.position.x, (int)obj.transform.position.z] = toggleButtonController.profileNum;
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
            if (_yPos[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z] != 1)
            {
                Destroy(selectedObj.transform.GetChild(0).gameObject);
                //selectedObj.transform.position = new Vector3(selectedObj.transform.position.x, 1, selectedObj.transform.position.z);
                tileArray[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z].transform.position = new Vector3(selectedObj.transform.position.x, 1, selectedObj.transform.position.z);
                _yPos[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z] = 1;
                tileRot[(int)selectedObj.transform.position.x, (int)selectedObj.transform.position.z] = 0;
                selectedObj = null;
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


    // Start is called before the first frame update
    void Start()
    {
        OnServerInitialized(_size);

        initCube();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                clicked(hit.collider.gameObject);
                Debug.Log(hit.transform.gameObject.transform.position);
            }
        }
    }
}
