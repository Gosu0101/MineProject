using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public List<GameObject> uiGameObject;
    public List<Text> uiText;
    public List<GameObject> selectTools;

    void Start()
    {

    }

    void Update()
    {
        // Inventory ON / OFF
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (uiGameObject[0].activeInHierarchy)
            {
                uiGameObject[0].SetActive(false);

                uiText[0].text = "OFF";
                uiText[0].color = Color.red;
            }
            else
            {
                uiGameObject[0].SetActive(true);

                uiText[0].text = "ON";
                uiText[0].color = Color.green;
            }
        }

        // Quest ON / OFF
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (uiGameObject[1].activeInHierarchy)
            {
                uiGameObject[1].SetActive(false);

                uiText[1].text = "OFF";
                uiText[1].color = Color.red;
            }
            else
            {
                uiGameObject[1].SetActive(true);

                uiText[1].text = "ON";
                uiText[1].color = Color.green;
            }
        }
        
        // Item ON / OFF
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (uiGameObject[2].activeInHierarchy)
            {
                uiGameObject[2].SetActive(false);

                uiText[2].text = "OFF";
                uiText[2].color = Color.red;
            }
            else
            {
                uiGameObject[2].SetActive(true);

                uiText[2].text = "ON";
                uiText[2].color = Color.green;
            }
        }

        // Select Tools
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            uiGameObject[3].transform.position = selectTools[0].transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            uiGameObject[3].transform.position = selectTools[1].transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            uiGameObject[3].transform.position = selectTools[2].transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            uiGameObject[3].transform.position = selectTools[3].transform.position;
        }
    }
}
