using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // UI ���ӽ����̽� �߰�

public class Cell : MonoBehaviour
{
    [SerializeField] private int _xIndex;
    public int xIndex { get { return _xIndex; } }

    [SerializeField] private int _yIndex;
    public int yIndex { get { return _yIndex; } }

    private List<DragBlock> _occupyingItems = new List<DragBlock>(); // �ش� ���� �����ϴ� ������ ����Ʈ

    private SpriteRenderer _cellImg; // Image ������Ʈ �߰�

    private void Start()
    {
        // ���� Image ������Ʈ�� �����ɴϴ�.
        _cellImg = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // ���콺 ������ ��ư Ŭ�� ����
        if (Input.GetMouseButtonDown(1))
        {
            if (!IsOccupied()) return;
            // ���콺 ��ġ���� Raycast�� ���� Ŭ���� ���� ã���ϴ�.
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject) // �ڽ��� Ŭ���Ǿ����� Ȯ��
            {
                ClearItems(); // �������� �����ϴ� �޼��� ȣ��
                Debug.Log("���� �������� ���ŵǾ����ϴ�. ��ġ: (" + _xIndex + ", " + _yIndex + ")");
            }
        }
    }

    public List<ItemData> GetItemDataList() 
    {
        List<ItemData> itemDatas = new List<ItemData>();

        DragBlock data = new DragBlock();
        foreach (var item in _occupyingItems)
        {
            itemDatas.Add(item.ItemData);
            if (item.ItemData.itemType > 0) data = item;
        }

        GridManager.Instance.ClearCollidingCells(data);

        return itemDatas;
    }

    public void Initialize(GridManager manager, int x, int y)
    {
        _xIndex = x;
        _yIndex = y;
    }

    // ���� �������� �߰�
    public void AddOccupyingItem(DragBlock item)
    {
        // �������� �̹� ����Ʈ�� �����ϴ��� Ȯ��
        if (!_occupyingItems.Contains(item))
        {
            _occupyingItems.Add(item); // ����Ʈ�� ������ �߰�
            UpdateCellColor(); // �� ���� ������Ʈ
        }
        else
        {
            Debug.LogWarning("�������� �̹� ���� �����մϴ�."); // �̹� �����ϴ� ��� ��� �޽���
        }
    }

    // ���� �������� �����ߴ��� Ȯ��
    public bool IsOccupied()
    {
        return _occupyingItems.Count > 0; // ����Ʈ�� �������� ������ occupied
    }
    // ���� �����۵��� �ı�
    public void ClearItems(bool isCheck = false)
    {
        foreach (Cell cell in _occupyingItems[0].SelectedCells)
        {
            if (cell.IsOccupied()) // ���� �������� ������ �ִ� ��쿡��
            {
                cell.ClearOccupyingItems(); // ���� �����۵��� �ı��ϴ� �޼��� ȣ��
            }
        }

        ClearOccupyingItems();
    }
    public void ClearOccupyingItems() 
    {
        foreach (var item in _occupyingItems)
        {
            if (item != null)
            {
                Destroy(item.gameObject); // ������ �ı�
            }
        }
        _occupyingItems.Clear(); // ����Ʈ �ʱ�ȭ

        UpdateCellColor();
        Debug.Log("���� �������� ���ŵǾ����ϴ�. ��ġ: (" + _xIndex + ", " + _yIndex + ")");

    }

    public void UpdateCellColor()
    {
/*        if (_occupyingItems.Count > 0)
        {
            _cellImg.color = Color.green; // �������� ���� �� �ʷϻ����� ����
        }
        else
        {
            _cellImg.color = Color.white; // ��� ������ ������� ����
        }*/
    }

    public List<DragBlock> GetOccupyingItems()
    {
        return _occupyingItems;
    }

    // ���� ������ �������� orderIndex�� ��ȯ
    public int GetLastOccupyingItemOrderIndex()
    {
        if (_occupyingItems.Count > 0)
        {
            return _occupyingItems[_occupyingItems.Count - 1].ItemData.orderIndex; // ������ �������� orderIndex ��ȯ
        }
        return -1; // �������� ������ -1 ��ȯ (�Ǵ� �ٸ� �⺻��)
    }

    public void ChangeColor(Color color)
    {
        // ���� ������ ����
        if (_cellImg != null)
        {
            _cellImg.color = color;
        }
    }
}
