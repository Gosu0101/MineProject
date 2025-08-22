using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public List<GameObject> ui;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && ui[0].activeInHierarchy)
        {
            ui[0].SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !ui[0].activeInHierarchy)
        {

        }
    }
}
