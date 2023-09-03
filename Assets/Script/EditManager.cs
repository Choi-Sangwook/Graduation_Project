using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    public GameObject TileMap;

    public GameObject tile;

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



    // Start is called before the first frame update
    void Start()
    {
        OnServerInitialized(_size);

        initCube();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
