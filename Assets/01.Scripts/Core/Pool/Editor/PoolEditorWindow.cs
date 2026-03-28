using System;
using System.Collections.Generic;
using System.Linq;
using Code.Core.Pool;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GGMPool.Editor
{
    public class PoolEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

        [SerializeField] private VisualTreeAsset itemUXMLAsset = default;
        [SerializeField] private PoolManagerSO poolManagerSO = default;
        [SerializeField] private ToolInfoSO toolInfo = default;
        
        private Button _createBtn;
        private ScrollView _itemView;

        private readonly List<PoolItem> _itemList = new();
        private PoolItem _currentItem;

        private ItemInspector _itemInspector;
        private UnityEditor.Editor _typeEditor, _itemEditor;
        
        [MenuItem("Tools/PoolEditorWindow")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<PoolEditorWindow>();
            wnd.titleContent = new GUIContent("Pool manager");
            wnd.minSize = new Vector2(700, 600);
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;

            VisualElement content = m_VisualTreeAsset.Instantiate();
            content.style.flexGrow = 1;
            root.Add(content);

            InitializeItems(content);
            GeneratePoolingItemUI();
        }

        private void GeneratePoolingItemUI()
        {
            _itemView.Clear();
            _itemList.Clear();
            _itemInspector.ClearInspector();

            foreach (var item in poolManagerSO.poolingItemList.OrderBy(x => x.poolType.name))
            {
                var itemUIAsset = itemUXMLAsset.Instantiate();
                var poolItem = new PoolItem(itemUIAsset, item);
                _itemView.Add(itemUIAsset);
                _itemList.Add(poolItem);

                poolItem.Name = item.name;
                poolItem.OnSelectEvent += HandleSelectItem;
                poolItem.OnDeleteEvent += HandleDeleteItem;
            }
        }

        private void HandleDeleteItem(PoolItem item)
        {
            if (!EditorUtility.DisplayDialog("Delete", $"Delete {item.Name}?", "Yes", "No"))
                return;
        
            poolManagerSO.poolingItemList.Remove(item.itemSO);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item.itemSO.poolType));
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(item.itemSO));
            EditorUtility.SetDirty(poolManagerSO);

            AssetDatabase.SaveAssets();

            if (_currentItem == item)
                _currentItem = null;
        
            GeneratePoolingItemUI();
        }

        private void HandleSelectItem(PoolItem item)
        {
            _itemList.ForEach(i => i.IsActive = false);
            item.IsActive = true;
            _currentItem = item;
            _itemInspector.UpdateInspector(item.itemSO);
        }

        private void InitializeItems(VisualElement content)
        {
            _createBtn = content.Q<Button>("BtnCreate");
            _itemView = content.Q<ScrollView>("ItemView");

            _itemView.Clear();

            _itemInspector = new ItemInspector(content, this);
            _itemInspector.NameChangeEvent += HandleAssetNameChange;
            _createBtn.clicked += HandleCreateItem;
        }

        private void HandleAssetNameChange(PoolingItemSO target, string newName)
        {
            var typePath = AssetDatabase.GetAssetPath(target.poolType);
            var itemPath = AssetDatabase.GetAssetPath(target);

            var exists = poolManagerSO.poolingItemList.Any(item => item.poolType.name.Equals(newName));
            
            if (exists)
            {
                EditorUtility.DisplayDialog("Duplicated name!!!",
                    $"Given asset name {newName} already exists", "OK");
                
                return;
            }
        
            AssetDatabase.RenameAsset(typePath, $"{newName}");
            AssetDatabase.RenameAsset(itemPath, $"{newName}");
        
            target.poolType.typeName = newName;
            EditorUtility.SetDirty(target.poolType);
            AssetDatabase.SaveAssets();
        
            GeneratePoolingItemUI();
        }

        private void HandleCreateItem()
        {
            var typeGuid = Guid.NewGuid();
            var typeSO = CreateInstance<PoolTypeSO>();
            typeSO.typeName = typeGuid.ToString();

            var typeFileName = $"{typeSO.typeName}.asset";
            var typeFilePath = $"{toolInfo.poolingFolder}/{toolInfo.typeFolder}";
            CreateFolderIfNotExist(typeFilePath);

            AssetDatabase.CreateAsset(typeSO, $"{typeFilePath}/{typeFileName}");

            var itemSO = CreateInstance<PoolingItemSO>();
            itemSO.poolType = typeSO;

            var itemFileName = $"{typeSO.name}.asset";
            var itemFilePath = $"{toolInfo.poolingFolder}/{toolInfo.itemFolder}";
            CreateFolderIfNotExist(itemFilePath);

            AssetDatabase.CreateAsset(itemSO, $"{itemFilePath}/{itemFileName}");
            poolManagerSO.poolingItemList.Add(itemSO);

            EditorUtility.SetDirty(poolManagerSO);
            AssetDatabase.SaveAssets();

            GeneratePoolingItemUI();
        }

        private void CreateFolderIfNotExist(string path)
        {
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
        }

        private void OnDestroy()
        {
            _itemInspector.ClearInspector();
        }
    }
}