using UnityEngine;
using UnityEngine.UI;
using System.Collections; // 코루틴을 사용하기 위해 추가

public class ShopUI : MonoBehaviour
{
    [Header("UI 오브젝트 연결")]
    [SerializeField] private Text saleFeedbackText; // "총 110 골드를 획득했습니다!" 텍스트

    // '판매' 버튼을 클릭했을 때 호출될 함수
    public void OnSellButtonClick(int material)
    {
        if (PlayerManager.Instance != null)
        {
            // PlayerManager에게 아이템을 모두 판매하고, 얼마를 벌었는지 돌려받습니다.
            int totalEarnedGold = PlayerManager.Instance.SellAllItems();
                
            saleFeedbackText.gameObject.SetActive(true);

            // 만약 1골드라도 벌었다면 피드백을 표시합니다.
            if (totalEarnedGold > 0)
            {
                StartCoroutine(ShowSaleFeedback(totalEarnedGold));
            }
            else // 판매할 아이템이 없다면
            {
                StartCoroutine(ShowSaleFeedback("You don't have any items to sell."));
            }
        }
    }

    // 판매 결과를 잠시 보여주고 사라지게 하는 코루틴
    private IEnumerator ShowSaleFeedback(int gold)
    {
        saleFeedbackText.text = "You've won " + gold + " gold!";

        yield return new WaitForSecondsRealtime(2f); // 2초 동안 보여줌 (시간이 멈춰있으므로 Realtime 사용)

        saleFeedbackText.gameObject.SetActive(false);
    }

    // 텍스트 피드백을 보여주는 코루틴 (오버로딩)
    private IEnumerator ShowSaleFeedback(string message)
    {
        saleFeedbackText.text = message;

        yield return new WaitForSecondsRealtime(2f);

        saleFeedbackText.gameObject.SetActive(false);
    }
}
