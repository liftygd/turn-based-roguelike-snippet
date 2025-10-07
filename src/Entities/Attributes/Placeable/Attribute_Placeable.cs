using System;
using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;

public class Attribute_Placeable : BaseAttribute
{
    private ComputerGrid _computerGrid;

    private PlacementParameters _placementParameters = new();
    public Action OnPickedUp;
    public Action OnPlaced;

    [Inject]
    public void Construct(ComputerGrid computerGrid)
    {
        _computerGrid = computerGrid;
    }

    public void SetPlacementParameters(PlacementParameters placementParameters)
    {
        _placementParameters = placementParameters;
    }

    private List<Vector2Int> GetAllCellPositions(Vector2Int center)
    {
        var positions = new List<Vector2Int>();
        positions.Add(center);

        foreach (var offset in _ownerEntity.occupiedCells)
            positions.Add(center + offset);

        return positions;
    }

    public bool TryPickUp()
    {
        if (!_ownerEntity.isPlaced) return true;

        if (_ownerEntity.Data.TryGetData(out EntityData data) &&
            data.characteristics.Contains(EntityCharacteristics.Immovable))
            return false;

        var positions = GetAllCellPositions(_ownerEntity.gridPosition);
        if (!TryPickUpAllCells(positions, out var cells)) return false;

        foreach (var cell in cells)
            cell.SetValue(null);

        OnPickedUp?.Invoke();
        return true;
    }

    private bool TryPickUpAllCells(List<Vector2Int> gridPositions, out List<GridCell> cells)
    {
        cells = new List<GridCell>();

        foreach (var position in gridPositions)
        {
            var cell = _computerGrid.GetCell(position.x, position.y);
            if (cell == null) return false;
            if (cell.GetValue() != _ownerEntity) return false;

            cells.Add(cell);
        }

        return true;
    }

    public bool TryPlace(Vector2Int gridPosition)
    {
        var positions = GetAllCellPositions(gridPosition);
        if (!TryPlaceAllCells(positions, out var cells)) return false;

        foreach (var cell in cells)
        {
            if (cell.IsOccupied()) continue;
            cell.SetValue(_ownerEntity);
        }

        _ownerEntity.gridPosition = gridPosition;
        _ownerEntity.SnapToEntity(_ownerEntity.transform);
        OnPlaced?.Invoke();

        return true;
    }

    private bool TryPlaceAllCells(List<Vector2Int> gridPositions, out List<GridCell> cells)
    {
        cells = new List<GridCell>();

        foreach (var position in gridPositions)
        {
            var cell = _computerGrid.GetCell(position.x, position.y);
            if (cell == null) return false;
            if (!_placementParameters.CanBePlaced(cell)) return false;

            cells.Add(cell);
        }

        return true;
    }

    public bool CanBePlaced(Vector2Int gridPosition)
    {
        var positions = GetAllCellPositions(gridPosition);
        if (!TryPlaceAllCells(positions, out var cells)) return false;

        return true;
    }
}