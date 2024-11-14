using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnDragItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _dropItem;
    [SerializeField] private IngredientData _puzzleItemData; // ������ �����͸� �巡�� �����ۿ� ����

    public void Setup(IngredientData itemData) 
    {
        _puzzleItemData = itemData;
    }

    // ���콺�� ������ �� ȣ��Ǵ� �Լ�
    public void OnPointerDown(PointerEventData eventData)
    {
        SpawnDropItem(_puzzleItemData);
    }

    // �巡�� ������ �������� �����ϴ� �Լ�
    public void SpawnDropItem(IngredientData itemData)
    {
        GameObject cloneDropItem = Instantiate(_dropItem, transform.position , Quaternion.identity);
        cloneDropItem.GetComponent<DragBlock>().Setup(transform.position, itemData);
    }
}
