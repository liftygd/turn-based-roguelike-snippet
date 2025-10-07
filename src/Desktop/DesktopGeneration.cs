using System;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UnityEngine;

public class DesktopGeneration : MonoBehaviour
{
    [SerializeField] private ComputerGrid computerGrid;
    [SerializeField] private float noiseScale = 1f;
    [SerializeField] private float noiseThreshold = 0.5f;
    [SerializeField] private ValueBank fileGenerationBank;
    private FileWorldFactory _fileWorldFactory;
    private BiomeManager _biomeManager;
    private Vector2Int _gridSize;
    private WallpaperController _wallpaperController;
    private BiomeData _currentBiome;

    [Inject]
    public void Construct(FileWorldFactory fileWorldFactory, WallpaperController wallpaperController, BiomeManager biomeManager)
    {
        _fileWorldFactory = fileWorldFactory;
        _wallpaperController = wallpaperController;
        _biomeManager = biomeManager;
    }

    [ContextMenu("Generate")]
    public async UniTask GenerateWorld()
    {
        _gridSize = computerGrid.GetCellCount();
        fileGenerationBank.ResetBank();
        _currentBiome = _biomeManager.currentBiome;

        await PopulateGrid();
    }

    private async UniTask PopulateGrid()
    {
        _wallpaperController.ChangeWallpaper(_currentBiome.biomeImage);

        for (var x = 0; x <= _gridSize.x; x++)
        for (var y = 0; y <= _gridSize.y; y++)
        {
            var cell = computerGrid.GetCell(x, y);
            if (cell == null || cell.IsOccupied()) continue;

            cell.animator.Reset();

            var perlin = Mathf.PerlinNoise(
                (float) x / _gridSize.x * noiseScale + GlobalRandom.NextRandomFloat() * 100,
                (float) y / _gridSize.y * noiseScale + GlobalRandom.NextRandomFloat() * 100);

            if (perlin >= noiseThreshold)
                await CreateFile(x, y, _currentBiome);
        }
    }

    private UniTask CreateFile(int x, int y, BiomeData biomeData)
    {
        var newFile = biomeData.GetRandomFile();
        if (newFile == null)
            return UniTask.CompletedTask;

        if ((float) newFile.cost / fileGenerationBank.GetBankValue() >= 0.3f)
            return UniTask.CompletedTask;

        if (!fileGenerationBank.RetrieveFromBank(newFile.cost))
            return UniTask.CompletedTask;

        _fileWorldFactory.Create(newFile, new Vector2Int(x, y));
        return UniTask.Delay(TimeSpan.FromSeconds(0.01f));
    }

    public async UniTask ClearWorld()
    {
        _gridSize = computerGrid.GetCellCount();
        _wallpaperController.ReturnDefaultWallpaper();

        await ClearGrid();
    }

    private async UniTask ClearGrid()
    {
        for (var x = 0; x <= _gridSize.x; x++)
        for (var y = 0; y <= _gridSize.y; y++)
        {
            var cell = computerGrid.GetCell(x, y);
            if (cell == null) continue;

            cell.animator.Reset();
            if (cell.GetValue() == null) continue;

            Destroy(cell.GetValue().gameObject);
            await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
        }
    }
}