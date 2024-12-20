using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnDragItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _dropItem;
    [SerializeField] private IngredientData _puzzleItemData; // 아이템 데이터를 드래그 아이템에 전달

    public void Setup(IngredientData itemData) 
    {
        _puzzleItemData = itemData;
    }

    // 마우스를 눌렀을 때 호출되는 함수
    public void OnPointerDown(PointerEventData eventData)
    {
        SpawnDropItem(_puzzleItemData);
    }

    // 드래그 가능한 아이템을 생성하는 함수
    public void SpawnDropItem(IngredientData itemData)
    {
        GameObject cloneDropItem = Instantiate(_dropItem, transform.position , Quaternion.identity);
        cloneDropItem.GetComponent<DragBlock>().Setup(transform.position, itemData);
    }
}
