using Reflex.Core;
using UnityEngine;

public class PlayablesWorldFactory : EntityFactory, IWorldFactory<ExecutableData, BaseExecutable>,
    IWorldFactory<PatchData, BasePatch>
{
    public PlayablesWorldFactory(ContainerBuilder containerBuilder) : base(containerBuilder)
    {
    }

    public BaseExecutable Create(ExecutableData data, Vector2Int position, bool shouldPlace = false)
    {
        return CreateEntity<BaseExecutable>(data, position, shouldPlace) as BaseExecutable;
    }

    public BasePatch Create(PatchData data, Vector2Int position, bool shouldPlace = false)
    {
        return CreateEntity<BasePatch>(data, position, shouldPlace) as BasePatch;
    }

    public GridEntity CreateFromType<TData>(TData data, Vector2Int position)
    {
        switch (data)
        {
            case ExecutableData executableData:
                return Create(executableData, position);
            case PatchData patchData:
                return Create(patchData, position);
        }

        return null;
    }

    private GridEntity CreateEntity<T>(EntityData data, Vector2Int position, bool shouldPlace) where T : GridEntity
    {
        var newEntity = CreatePrefab<T>(data);

        if (newEntity == null)
            return null;

        newEntity.Data.SetData(data);
        newEntity.Configurator.Configure();
        newEntity.transform.position = Vector3.one * -100f;

        var movable = newEntity.GetComponent<Attribute_Movable>();
        if (movable && !shouldPlace)
        {
            movable.Configurator.Configure();
            movable.ResetPosition();
            movable.StartMoving().Forget();
        }
        else
        {
            var placeable = newEntity.GetComponent<Attribute_Placeable>();
            placeable.Configurator.Configure();

            if (!placeable.TryPlace(position))
            {
                Object.Destroy(newEntity);
                return null;
            }
        }

        return newEntity;
    }
}