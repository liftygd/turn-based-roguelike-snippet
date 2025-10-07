using UnityEngine;

public abstract class BaseAttribute : MonoBehaviour, IConfigurable
{
    protected GridEntity _ownerEntity;

    private void Awake()
    {
        _ownerEntity = GetComponent<GridEntity>();
        Configurator = new Configurator(Configure, Dispose);
    }

    private void Start()
    {
        if (!_ownerEntity)
            _ownerEntity = GetComponent<GridEntity>();

        Configurator.Configure();
    }

    public Configurator Configurator { get; private set; }

    protected virtual void Configure()
    {
    }

    protected virtual void Dispose()
    {
    }
}