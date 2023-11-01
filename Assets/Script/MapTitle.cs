using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTitle : MonoBehaviour
{
    public MapListManager manager;
    public string title;

    public void onClickedTitle()
    {
        manager.OnclickedTitle(title);
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
