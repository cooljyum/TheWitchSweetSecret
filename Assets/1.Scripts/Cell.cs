using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // UI 네임스페이스 추가

public class Cell : MonoBehaviour
{
    [SerializeField] private int _xIndex;
    public int xIndex => _xIndex;

    [SerializeField] private int _yIndex;
    public int yIndex => _yIndex;

    public int Index { get; private set; }

    private List<DragBlock> _occupyingItems = new List<DragBlock>();
    private SpriteRenderer _cellImg;

    private void Start()
    {
        _cellImg = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!IsOccupied()) return;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ClearItems();
                Debug.Log("셀의 아이템이 제거되었습니다. 위치: (" + _xIndex + ", " + _yIndex + ")");
            }
        }
    }

    public List<IngredientData> GetItemDataList()
    {
        List<IngredientData> itemDatas = new List<IngredientData>();
        DragBlock data = new DragBlock();
        foreach (var item in _occupyingItems)
        {
            itemDatas.Add(item.IngredientData);
            if (item.IngredientData.itemType > 0) data = item;
        }

        GridManager.Instance.ClearCollidingCells(data);

        return itemDatas;
    }

    public void Initialize(GridManager manager, int x, int y, int index)
    {
        _xIndex = x;
        _yIndex = y;
        Index = index;
    }

    public void AddOccupyingItem(DragBlock item)
    {
        if (!_occupyingItems.Contains(item))
        {
            _occupyingItems.Add(item);
            UpdateCellColor();
        }
        else
        {
            Debug.LogWarning("아이템이 이미 셀에 존재합니다.");
        }
    }

    public bool IsOccupied()
    {
        return _occupyingItems.Count > 0;
    }

    public void ClearItems(bool isCheck = false)
    {
        foreach (Cell cell in _occupyingItems[0].SelectedCells)
        {
            if (cell.IsOccupied())
            {
                cell.ClearOccupyingItems();
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
                Destroy(item.gameObject);
            }
        }
        _occupyingItems.Clear();

        UpdateCellColor();
        Debug.Log("셀의 아이템이 제거되었습니다. 위치: (" + _xIndex + ", " + _yIndex + ")");
    }

    public void UpdateCellColor()
    {
        if (_cellImg != null)
        {
            _cellImg.color = _occupyingItems.Count > 0 ? Color.green : Color.white;
        }
    }

    public List<DragBlock> GetOccupyingItems()
    {
        return _occupyingItems;
    }

    public int GetLastOccupyingItemOrderIndex()
    {
        return _occupyingItems.Count > 0 ? _occupyingItems[^1].IngredientData.orderIndex : -1;
    }

    public void ChangeColor(Color color)
    {
        if (_cellImg != null)
        {
            _cellImg.color = color;
        }
    }
}