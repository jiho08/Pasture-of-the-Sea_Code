using System;
using Code.Core.Pool;
using GGMPool.Editor;
using UnityEditor;
using UnityEngine.UIElements;

public class ItemInspector : IDisposable
{
    private readonly TextField _assetNameField;
    private readonly Button _nameChangeBtn;

    private PoolEditorWindow _editorWindow;

    private PoolingItemSO _targetItem;
    private Editor _typeEditor, _itemEditor;

    public delegate void NameChangeDelegate(PoolingItemSO target, string newName);

    public event NameChangeDelegate NameChangeEvent;

    public ItemInspector(VisualElement content, PoolEditorWindow editorWindow)
    {
        _editorWindow = editorWindow;

        _assetNameField = content.Q<TextField>("AssetNameField");
        _nameChangeBtn = content.Q<Button>("BtnChange");

        var typeView = content.Q<IMGUIContainer>("TypeInspectorView");
        var itemView = content.Q<IMGUIContainer>("ItemInspectorView");

        typeView.onGUIHandler += HandleTypeViewGUI;
        itemView.onGUIHandler += HandleItemViewGUI;

        _nameChangeBtn.clicked += HandleNamaeChange;
    }

    private void HandleNamaeChange()
    {
        if (_targetItem == null)
            return;

        if (string.IsNullOrEmpty(_assetNameField.value.Trim()))
            return;

        var newName = _assetNameField.value;
        
        if (EditorUtility.DisplayDialog("Rename", $"Rename this asset to {newName}?", "Yes", "No"))
            NameChangeEvent?.Invoke(_targetItem, newName);
    }

    private void HandleItemViewGUI()
    {
        if (_targetItem == null)
            return;
        
        Editor.CreateCachedEditor(_targetItem, null, ref _itemEditor);
        _itemEditor.OnInspectorGUI();
    }

    private void HandleTypeViewGUI()
    {
        if (_targetItem == null)
            return;
        
        Editor.CreateCachedEditor(_targetItem.poolType, null, ref _typeEditor);
        _typeEditor.OnInspectorGUI();
    }

    public void UpdateInspector(PoolingItemSO item)
    {
        _assetNameField.SetValueWithoutNotify(item.poolType.name);
        _targetItem = item;
    }

    public void ClearInspector()
    {
        _assetNameField.SetValueWithoutNotify(string.Empty);
        _targetItem = null;
    }

    public void Dispose()
    {
        UnityEngine.Object.DestroyImmediate(_itemEditor);
        UnityEngine.Object.DestroyImmediate(_typeEditor);
    }
}