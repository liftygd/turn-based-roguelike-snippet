using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

[RequireComponent(typeof(Attribute_Placeable))]
[RequireComponent(typeof(BoxCollider2D))]
public class GridEntity : MonoBehaviour, IConfigurable
{
    [Header("Entity")] public Vector2Int gridPosition;

    public readonly EntityDataController Data = new();
    private Stats _baseEntityStats;

    protected ComputerGrid _computerGrid;
    public bool isPlaced { get; protected set; }
    public bool isOnGameField { get; protected set; }
    [field: SerializeField] public TimelinePlayer asyncAnimator { get; private set; }
    [field: SerializeField] public List<Vector2Int> occupiedCells { get; private set; } = new() {Vector2Int.zero};

    protected virtual PlacementParameters _customPlacementParameters { get; }
        = new(cell => !cell.IsOccupied());

    private void Awake()
    {
        Configurator = new Configurator(Configure, Dispose);
    }

    private void OnDestroy()
    {
        if (!isPlaced) return;
        Configurator.Dispose();

        var currentCell = _computerGrid.GetCell(gridPosition.x, gridPosition.y);
        if (currentCell == null) return;
        if (currentCell.GetValue() != this) return;

        currentCell.SetValue(null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);

        var center = transform.localPosition;
        foreach (var offset in occupiedCells)
            Gizmos.DrawCube(center + (Vector3Int) offset, 0.2f * Vector3.one);
    }

    public Configurator Configurator { get; private set; }

    [Inject]
    public void Construct(ComputerGrid computerGrid)
    {
        _computerGrid = computerGrid;
    }

    protected virtual void Configure()
    {
        var placeable = GetComponent<Attribute_Placeable>();
        placeable.OnPlaced += () => isPlaced = true;
        placeable.OnPlaced += () => isOnGameField = true;

        placeable.OnPickedUp += () => isPlaced = false;
        placeable.SetPlacementParameters(_customPlacementParameters);
    }

    protected virtual void Dispose()
    {
    }

    public void SnapToEntity(Transform obj)
    {
        var cell = _computerGrid.GetCell(gridPosition.x, gridPosition.y);
        if (cell == null) return;

        cell.SnapToCell(obj);
    }
}