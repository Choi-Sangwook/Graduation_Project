using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTitle : MonoBehaviour
{
    public MapListManager manager;
    public string title;
    public int index;

    public void onClickedTitle()
    {
        manager.OnclickedTitle(title);
        manager.index = index;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
