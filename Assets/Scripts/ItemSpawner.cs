using System.Collections.Generic;
using UnityEngine;
using frare;
using frare.CollectionsExtensions;

public class ItemSpawner : Singleton<ItemSpawner>
{
    protected override bool DontDestroyWhenLoad => false;

    [SerializeField] private List<Item> items;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public Item SpawnItem(Transform target)
    {
        Item item = items.GetRandom();
        Item instance = Instantiate(items.GetRandom(), target.transform.position, item.transform.rotation, target);
        return instance;
    }
}
