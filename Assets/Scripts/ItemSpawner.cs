using System.Collections.Generic;
using UnityEngine;
using frare;
using frare.CollectionsExtensions;

public class ItemSpawner : Singleton<ItemSpawner>
{
    protected override bool DontDestroyWhenLoad => false;

    [SerializeField] private List<Item> items;

    private Item nextItem;

    protected override void Awake()
    {
        base.Awake();

        nextItem = items.GetRandom();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public Item SpawnItem(Transform target)
    {
        Item item = nextItem;
        Item instance = Instantiate(item, target.transform.position, item.transform.rotation, target);

        SetupNextItem();

        return instance;
    }

    private void SetupNextItem()
    {
        nextItem = items.GetRandom();
        MainCanvas.Instance.UpdateNextItem(nextItem);
    }
}
