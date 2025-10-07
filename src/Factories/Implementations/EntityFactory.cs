using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

public class EntityFactory
{
    private Container _container;

    public EntityFactory(ContainerBuilder containerBuilder)
    {
        containerBuilder.OnContainerBuilt += container => _container = container;
    }

    protected T CreatePrefab<T>(EntityData data) where T : MonoBehaviour
    {
        var newEntity = Object.Instantiate(data.entityPrefab).GetComponent<T>();
        GameObjectInjector.InjectRecursive(newEntity.gameObject, _container);

        return newEntity;
    }
}