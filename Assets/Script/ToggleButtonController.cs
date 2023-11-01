using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonController : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public bool isSelectBtnOn;
    public int profileNum;
    public string title;

    public EditManager editManager;


    public void OnclickedTitle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            title = toggle.GetComponent<ToggleButtonController>().title;
            Debug.Log(title);
            isSelectBtnOn = true;
        }
        else
        {
            isSelectBtnOn = false;
        }
    }


    public void OnToggleButtonClicked(Toggle toggle)
    {
        if (toggle.isOn)
        {
            Debug.Log(toggle.transform.position);
            prefabToSpawn = toggle.GetComponent<ToggleButtonController>().prefabToSpawn;
            profileNum = toggle.GetComponent<PiprNum>().num;
            Debug.Log(prefabToSpawn.name);
            if (editManager.selectedObj != null)        
            {
                editManager.childCount = editManager.selectedObj.transform.GetChild(0).transform.childCount;
                for (int i = 0; i < editManager.childCount; i++)
                {
                    editManager.selectedObj.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = editManager.original_Mat;
                }
                editManager.selectedObj = null;
            }
        }
        else
        {
            prefabToSpawn = null;
        }
    }

    public void OnSelectBtnClicked(Toggle toggle)
    {
        if (toggle.isOn)
        {
            isSelectBtnOn = true;
        }
        else
        {
            isSelectBtnOn = false;
            if (editManager.selectedObj != null)
            {
                editManager.childCount = editManager.selectedObj.transform.GetChild(0).transform.childCount;
                for (int i = 0; i < editManager.childCount; i++)
                {
                    editManager.selectedObj.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<Renderer>().material = editManager.original_Mat;
                }
                //editManager.selectedObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = editManager.original_Mat;
                editManager.selectedObj = null;
            }

        }
    }
}
