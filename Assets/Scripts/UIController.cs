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
        if (Input.GetKeyDown(KeyCode.E)) UISetActive(0); // E키 누를 시, Inventory 활성화 or 비활성화
        if (Input.GetKeyDown(KeyCode.Q)) UISetActive(1); // Q키 누를 시, Quest 활성화 or 비활성화
        if (Input.GetKeyDown(KeyCode.R)) UISetActive(2); // R키 누를 시, Item 활성화 or 비활성화

        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectTools(0); // 상단 1번 키 누를 시, 1번 도구 선택
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectTools(1); // 상단 2번 키 누를 시, 2번 도구 선택
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectTools(2); // 상단 3번 키 누를 시, 3번 도구 선택
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectTools(3); // 상단 4번 키 누를 시, 4번 도구 선택
    }

    // List<GameObject>내 uiGameObject에 저장된 UI를 활성화 or 비활성화
    void UISetActive(int index)
    {
        if (uiGameObject[index].activeInHierarchy)
        {
            uiGameObject[index].SetActive(false);

            uiText[index].text = "OFF";
            uiText[index].color = Color.red;
        }
        else
        {
            uiGameObject[index].SetActive(true);

            uiText[index].text = "ON";
            uiText[index].color = Color.green;
        }
    }

    // List<GameObject>내 selectTools에 저장된 위치로 Select 오브젝트가 이동
    void SelectTools(int index)
    {
        uiGameObject[3].transform.position = selectTools[index].transform.position;

        // 선택한 도구에 따라 작용하는 상호작용 소스코드 필요
    }
}
