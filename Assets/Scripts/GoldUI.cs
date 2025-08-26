using UnityEngine;
using UnityEngine.UI; // TextMeshPro 대신 기본 UI를 사용하기 위해 변경

public class GoldUI : MonoBehaviour
{
    // Inspector 창에서 연결할 기본 Text UI
    public Text goldText; // 변수 타입을 Text로 변경

    // Gold 값을 받아서 텍스트를 업데이트하는 함수
    public void UpdateGoldText(int amount)
    {
        if (goldText != null)
        {
            // 숫자를 텍스트로 변환하여 UI에 표시
            goldText.text = amount.ToString();
        }
    }
}
