using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragBlock : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curveMovement; // �̵� ���� �׷���
    [SerializeField] private AnimationCurve _curveScale;    // ũ�� ���� �׷���

    private bool _isOccupied = false;

    private float _returnTime = 0.1f;  // ��� ���� ��ġ ���ư� �� �ҿ� �ð�

    [SerializeField] private ItemData _itemData; //������ ������
    public ItemData ItemData // public ������Ƽ �߰�
    {
        get { return _itemData; }
    }

    private Vector2 _parentPosition;

    private List<Cell> _selectedCells = new List<Cell>(); // DropBlock ���� ���õ� �� ����Ʈ
    public List<Cell> SelectedCells
    {
        get { return _selectedCells; }
    }

    [field: SerializeField] public Vector2Int BlockCount { private set; get; } // ��ī��Ʈ
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer; // ��������Ʈ ������ �߰�

    private bool _isMouseUp = false;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ �ʱ�ȭ
    }

    private void Start()
    {
        MouseDown();
    }

    private void Update()
    {
        if (_isOccupied) return;
        if (!_isMouseUp) MouseDrag();

        if (Input.GetMouseButtonUp(0))
        {
            _isMouseUp= true;
            MouseUp();
        }
    }

    public void Setup(Vector3 parentPosition, ItemData itemData)
    {
        _parentPosition = parentPosition;
        _itemData = itemData;
      
        _collider = GetComponent<BoxCollider2D>();

        // �������� ��������Ʈ�� ����
        if (_spriteRenderer != null && _itemData != null)
        {
            _spriteRenderer.sprite = _itemData.itemImage; // ������ �̹����� ����
            _spriteRenderer.sortingOrder = _itemData.orderIndex + 3; // �ε����� ������� ���� ���� ����
        }
    }

    private void MouseDown()
    {
        UIManager.Instance.SetScrollEnabled(false);

        // �ڷ�ƾ ȣ�� �� ���ڿ��� �ƴ� �޼��带 ���� ȣ��
        StopCoroutine(OnScaleTo(Vector3.one));
        StartCoroutine(OnScaleTo(Vector3.one * 1.3f));
    }

    private void MouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);

        bool isPackagingItem = _itemData.itemType == 0;

        _selectedCells = GridManager.Instance.CheckCellOverlap(_collider, _itemData.itemSize.x, _itemData.itemSize.y, _itemData.orderIndex); // ������Ʈ�� ����Ʈ�� ��ȯ����
    }

    private void MouseUp()
    {
        UIManager.Instance.SetScrollEnabled(true);

        // �������� ������ �� �� ���
        int requiredCells = _itemData.itemSize.x * _itemData.itemSize.y;

        // ���õ� ���� �ʿ��� �� ������ ������ ���� ��ġ�� ����
        if (_selectedCells.Count < requiredCells)
        {
            StartCoroutine(OnMoveTo(_parentPosition, _returnTime)); // �θ� ��ġ�� �̵�
        }
        else
        {
            // ����� ���� ���õ� ���, ���� ����� �׸��� ��ġ�� ����
            Vector2 closestGridPosition = GridManager.Instance.GetCellsCenterPosition(_selectedCells);
            transform.position = new Vector3(closestGridPosition.x, closestGridPosition.y, transform.position.z);

            // �� ���õ� ���� �� �������� ��ġ�ߴٰ� ǥ��
            foreach (var cell in _selectedCells)
            {
                cell.AddOccupyingItem(this);
            }

            _isOccupied = true;
        }

        // ũ�� ���� �ִϸ��̼� �ʱ�ȭ
        StopCoroutine(OnScaleTo(Vector3.one * 1.3f));
        StartCoroutine(OnScaleTo(Vector3.one));
    }

    /// <summary>
    /// ���� ��ġ���� end ��ġ���� time �ð����� �̵�
    /// </summary>
    private IEnumerator OnMoveTo(Vector3 end, float time)
    {
        Vector3 start = transform.position;
        float current = 0f;
        float percent = 0f;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / time;

            transform.position = Vector3.Lerp(start, end, _curveMovement.Evaluate(percent));

            yield return null;
        }

        // ��ǥ ��ġ�� �����ϸ� ������Ʈ ����
        transform.position = end; // ���� ��ġ�� ����
        Destroy(gameObject);
    }

    private IEnumerator OnScaleTo(Vector3 end)
    {
        Vector3 start = transform.localScale;
        float current = 0f;
        float percent = 0f;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / _returnTime;

            transform.localScale = Vector3.Lerp(start, end, _curveScale.Evaluate(percent));
            yield return null;
        }
    }
}
