using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnDragItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _dropItem;
    [SerializeField] private ItemData _itemData; // ������ �����͸� �巡�� �����ۿ� ����

    public void Setup(ItemData itemData) 
    {
        _itemData = itemData;
    }

    // ���콺�� ������ �� ȣ��Ǵ� �Լ�
    public void OnPointerDown(PointerEventData eventData)
    {
        SpawnDropItem(_itemData);
    }

    // �巡�� ������ �������� �����ϴ� �Լ�
    public void SpawnDropItem(ItemData itemData)
    {
        GameObject cloneDropItem = Instantiate(_dropItem, transform.position , Quaternion.identity);
        cloneDropItem.GetComponent<DragBlock>().Setup(transform.position, itemData);
    }
}
