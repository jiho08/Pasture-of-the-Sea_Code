using System;
using Code.Core.Pool;
using UnityEngine.UIElements;

public class PoolItem
{
    private readonly Label _nameLabel;
    private readonly VisualElement _rootElem;

    public event Action<PoolItem> OnDeleteEvent;
    public event Action<PoolItem> OnSelectEvent;

    public string Name
    {
        get => _nameLabel.text;
        set => _nameLabel.text = value;
    }

    public readonly PoolingItemSO itemSO;

    public bool IsActive
    {
        get => _rootElem.ClassListContains("active");
        
        set
        {
            if (value)
                _rootElem.AddToClassList("active");
            else
                _rootElem.RemoveFromClassList("active");
        }
    }

    public PoolItem(VisualElement root, PoolingItemSO itemSo)
    {
        itemSO = itemSo;
        _rootElem = root.Q("PoolItem");
        _nameLabel = root.Q<Label>("ItemName");
        var deleteBtn = root.Q<Button>("BtnDelete");
        
        deleteBtn.RegisterCallback<ClickEvent>(evt =>
        {
            OnDeleteEvent?.Invoke(this);
            evt.StopPropagation();
        });

        _rootElem.RegisterCallback<ClickEvent>(evt =>
        {
            OnSelectEvent?.Invoke(this);
            evt.StopPropagation();
        });
    }
}