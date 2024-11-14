using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public GameObject _cellPrefab;                   // �� ������
    public Transform _gridParent;                    // �׸��� �θ� ������Ʈ
    public int _gridSize = 5;                        // �׸��� ũ�� (��: 5x5)
    //public List<int> _activeCellIndices;             // Ȱ��ȭ�� �� �ε���

    private List<Cell> _cells = new List<Cell>();
    private List<PuzzleItemData>[] _itemDataArray;  // ������ ������ ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���� ��ü �ı�
        }
    }

    private void Start()
    {
        InitializeGrid();
        InitializeItemDataArray();
    }

    // �ʱ�ȭ �� �׸��� ���� �� Ȱ��ȭ
    public void InitializeGrid()
    {
        ClearGrid();

        for (int i = 0; i < _gridSize * _gridSize; i++)
        {
            GameObject newCellObj = Instantiate(_cellPrefab, _gridParent);
            Cell newCell = newCellObj.GetComponent<Cell>();
            newCell.Initialize(this, i % _gridSize, i / _gridSize, i);

            // _activeCellIndices�� ���Ե� �ε����� Ȱ��ȭ
            if (_activeCellIndices.Contains(i))
            {
                newCellObj.SetActive(true);
                newCell.ChangeColor(Color.green); // ��: Ȱ��ȭ�� ���� �ʷϻ����� ǥ��
            }
            else
            {
                newCellObj.SetActive(false);
            }

            _cells.Add(newCell);
        }

        ArrangeGrid();
    }

    // �׸��� ����
    private void ClearGrid()
    {
        foreach (var cell in _cells)
        {
            Destroy(cell.gameObject);
        }
        _cells.Clear();
    }

    // �׸��� �迭 ����
    private void ArrangeGrid()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            int x = i % _gridSize;
            int y = i / _gridSize;
            _cells[i].transform.localPosition = new Vector3(x, y, 0);
        }
    }

    // ���� �����۵��� ����
    public void ClearCollidingCells(DragBlock data)
    {
        foreach (Cell cell in data.SelectedCells)
        {
            if (cell.IsOccupied())
            {
                cell.ClearOccupyingItems();
            }
        }
    }

    // ���� ����Ʈ �迭 �ʱ�ȭ
    private void InitializeItemDataArray()
    {
        _itemDataArray = new List<PuzzleItemData>[_gridSize * _gridSize]; // �� ������ŭ ����Ʈ �迭 �ʱ�ȭ

        for (int i = 0; i < _itemDataArray.Length; i++)
        {
            _itemDataArray[i] = new List<PuzzleItemData>();
        }
    }

    public void OnClearButtonClicked()
    {
        ClearAllItems(); // ���� �������� �����ϴ� �޼��带 ȣ��
    }

    // ��� ���� �������� �����ϴ� �޼���
    public void ClearAllItems()
    {
        foreach (Cell cell in _cells)
        {
            if (cell.IsOccupied())
            {
                cell.ClearItems(); // ������ ����
            }
        }
        Debug.Log("��� ���� �������� ���ŵǾ����ϴ�.");
    }

    // ���õ� ������ �߽� ��ġ�� ����ϴ� �޼���
    public Vector2 GetCellsCenterPosition(List<Cell> selectedCells)
    {
        if (selectedCells.Count == 0) return Vector2.zero;

        Vector2 centerPosition = Vector2.zero;
        foreach (Cell cell in selectedCells)
        {
            centerPosition += (Vector2)cell.transform.position;
        }

        centerPosition /= selectedCells.Count;
        return centerPosition;
    }

    // �����۰� ���� �浹�� üũ�ϴ� �޼���
    public List<Cell> CheckCellOverlap(BoxCollider2D itemCollider, int width, int height, int itemOrderIndex)
    {
        List<Cell> selectedCells = new List<Cell>();
        Cell selectedCell = null;
        float maxArea = 0.0f;

        foreach (Cell cell in _cells)
        {
            BoxCollider2D cellCollider = cell.GetComponent<BoxCollider2D>();
            float overlapArea = GetOverlapArea(itemCollider.bounds, cellCollider.bounds, out float overlapWidth, out float overlapHeight);

            if (overlapArea > maxArea)
            {
                maxArea = overlapArea;
                selectedCell = cell;
            }
        }

        if (selectedCell != null)
        {
            AddCellsToSelection(selectedCells, selectedCell, width, height, itemOrderIndex);
        }

        return selectedCells;
    }

    // �� ���� �ٿ�� ���� ��ġ�� ������ ����ϴ� �޼���
    private float GetOverlapArea(Bounds boundsA, Bounds boundsB, out float overlapWidth, out float overlapHeight)
    {
        float xMin = Mathf.Max(boundsA.min.x, boundsB.min.x);
        float xMax = Mathf.Min(boundsA.max.x, boundsB.max.x);
        float yMin = Mathf.Max(boundsA.min.y, boundsB.min.y);
        float yMax = Mathf.Min(boundsA.max.y, boundsB.max.y);

        overlapWidth = xMax - xMin;
        overlapHeight = yMax - yMin;

        return overlapWidth > 0 && overlapHeight > 0 ? overlapWidth * overlapHeight : 0;
    }

    // ���õ� ���� �������� �߰��ϴ� �޼���
    private void AddCellsToSelection(List<Cell> selectedCells, Cell selectedCell, int width, int height, int itemOrderIndex)
    {
        int x = selectedCell.xIndex;
        int y = selectedCell.yIndex;
        

        int startX = 0;
        int startY = 0;

        // ������ ũ�⿡ ���� ���� ��ǥ ���
        //  x : + -> ���� /- -> �� //y : + -> �Ʒ� /- -> �� 
        switch (width)
        {
            case 1: // 1x1, 1x2, 1x3
                startX = x;
                startY = y + (height > 1 ? 1 : 0); // ���̿� �°� �Ʒ������� �̵�;

                if (y == 0 && height > 2)
                {
                    startY += 1;
                }

                if (y == 4 && height >= 2)
                {
                    startY -= 1;
                }

                break;

            case 2: // 2x1, 2x2
                startX = x - 1; // 2ĭ�� �� �������� 1ĭ �̵�
                startY = y + (height > 1 ? 1 : 0); // ���̿� �°�  �������� �̵�

                if (x == 0)
                {
                    startX += 1;
                }

                if (y == 4 && height > 1)
                {
                    startY -= 1;
                }

                break;

            case 3: // 3x1, 3x3
                startX = x - 1; // 3ĭ�� �� �������� 1ĭ �̵�
                startY = y + (height == 1 ? 0 : (height == 2 ? 1 : 2)); // ���̿� �°� �������� �̵�
                
                if (x == 0)
                {
                    startX += 1;
                }

                if (x == 4)
                {
                    startX -= 1;
                }

                if (y >= 3 && height > 1)
                {
                    startY -= (y-2);
                }
                break;

            case 4: // 4x4
                startX = x - 2; // 4ĭ�� �� �������� 2ĭ �̵�
                startY = y + 2; // ���̿� �°� �������� �̵�

                if (x == 0)
                {
                    startX += 2;
                }
                if (x == 1)
                {
                    startX += 1;
                }

                if (x == 4)
                {
                    startX -= 1;
                }

                if (y == 0)
                {
                    startY += 1;
                }

                if (y >= 3 && height > 1)
                {
                    startY -= (y - 2);
                }

                break;

            default:
                startX = x - (width - 1) / 2; // �⺻���� ��� (��� ����)
                startY = y - (height - 1) / 2; // �⺻���� ��� (��� ����)
                break;
        }

        // ������ �� ��ǥ
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int cellX = startX + j; // ���� X ��ǥ�� j�� ����
                int cellY = startY - i; // ���� Y ��ǥ���� i�� �� (Y�� ����)

                if (cellX >= 0 && cellX < _cells.GetLength(0) && cellY >= 0 && cellY < _cells.GetLength(1))
                {
                    Cell cell = _cells[cellX, cellY];

                    if (itemOrderIndex > 0)
                    {
                        if (!selectedCells.Contains(cell) && cell.IsOccupied() && itemOrderIndex > cell.GetLastOccupyingItemOrderIndex())
                        {
                            selectedCells.Add(cell);
                        }
                    }
                    else
                    {
                        // ���� �̹� ���� �������� �ִ��� Ȯ��
                        if (!selectedCells.Contains(cell) && !cell.IsOccupied())
                        {
                            selectedCells.Add(cell);
                        }
                    }
                }
            }
        }
    }

    public List<PuzzleItemData>[] GetCellItemData()
    {
        InitializeItemDataArray();
        int itemCount = 0;

        foreach (Cell cell in _cells)
        {
            if (cell.IsOccupied())
            {
                List<DragBlock> dragBlockList = cell.GetOccupyingItems();
                var dragBlockListCopy = new List<DragBlock>(dragBlockList); // ���纻 ����

                foreach (var item in dragBlockListCopy) // ���纻�� �ݺ�
                {
                    if (itemCount < _itemDataArray.Length)
                    {
                        _itemDataArray[itemCount].Add(item.IngredientData);
                        itemCount++; 
                        ClearCollidingCells(item);
                    }
                }
            }
        }

        return _itemDataArray;
    }
}