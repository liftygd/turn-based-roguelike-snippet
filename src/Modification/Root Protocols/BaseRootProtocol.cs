using UnityEngine;

public abstract class BaseRootProtocol : MonoBehaviour, IModifier, IConfigurable, ITag_PlayerEntity
{
    private void Awake()
    {
        Configurator = new Configurator(Configure, Dispose);
    }

    private void OnDestroy()
    {
        Configurator.Dispose();
    }

    public Configurator Configurator { get; private set; }
    public abstract ModifierOrder Order { get; }
    public abstract bool IsStackable { get; }

    protected abstract void Configure();

    protected abstract void Dispose();
}