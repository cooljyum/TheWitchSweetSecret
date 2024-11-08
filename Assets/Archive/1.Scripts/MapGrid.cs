using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    private GameObject _homePoint;                   // ���/���� ����
    private int _columns = 13;                       // ���� �� ����
    private int _rows = 7;                           // ���� �� ����
    private float _cellSize = 1.0f;                  // �� ����
    private List<GameObject> _mapCells = new List<GameObject>();
    public List<GameObject> MapCells => _mapCells;

    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        // ���� �Ʒ� �������� ���� �����ǵ��� ���� ��ġ�� ���
        Vector3 startPosition = transform.position - new Vector3(_columns * _cellSize / 1.6f, _rows * _cellSize / 2.25f, 0);

        for (int x = 0; x < _columns; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                // �� ���� ��ġ�� ���� �Ʒ� �������� ����
                Vector3 position = startPosition + new Vector3(x * _cellSize, y * _cellSize, 0);
                GameObject cell = Instantiate(_cellPrefab, position, Quaternion.identity, transform);

                if (x % 2 == 1 || y % 2 == 1)
                {
                    // ȸ�� PathCell
                    cell.tag = "PathCell";
                    cell.AddComponent<BoxCollider2D>();

                    var spriteRenderer = cell.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                        spriteRenderer.color = Color.gray;
                }
                else
                {
                    // ��� BlockCell
                    cell.tag = "BlockCell";

                    var spriteRenderer = cell.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                        spriteRenderer.color = Color.white;
                }

                _mapCells.Add(cell);
            }
        }
        // �� �Ʒ� ������ ���� Ȩ ����Ʈ�� ����
        GameObject homePoint = transform.GetChild(84).gameObject;
        _homePoint = homePoint;

        var homeSpriteRenderer = homePoint.GetComponent<SpriteRenderer>();
        if (homeSpriteRenderer != null)
            homeSpriteRenderer.color = Color.red;
    }

    public int GetCellIndex(GameObject cell)
    {
        if (_mapCells.Contains(cell))
        {
            return _mapCells.IndexOf(cell);
        }
        else
        {
            Debug.LogWarning("�ش� ���� �ʿ� �������� �ʽ��ϴ�.");
            return -1;
        }
    }

    public GameObject GetHomePoint()
    {
        return _homePoint;
    }
}