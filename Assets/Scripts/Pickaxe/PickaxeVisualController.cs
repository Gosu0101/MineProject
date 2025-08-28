using UnityEngine;

public class PickaxeVisualController : MonoBehaviour
{
    private GameObject currentPickaxeModel; // 현재 들고 있는 곡괭이 모델

    // 새로운 곡괭이 모델로 교체하는 함수
    public void ChangePickaxeVisual(PickaxeData newPickaxeData)
    {
        // 1. 기존에 들고 있던 곡괭이가 있다면 파괴합니다.
        if (currentPickaxeModel != null)
        {
            Destroy(currentPickaxeModel);
        }

        // 2. 새로운 곡괭이 데이터에 모델 프리팹이 있다면
        if (newPickaxeData.pickaxePrefab != null)
        {
            // 3. 새로운 모델을 생성하고, 이 오브젝트의 자식으로 만듭니다.
            currentPickaxeModel = Instantiate(newPickaxeData.pickaxePrefab, transform);
        }
    }
}
