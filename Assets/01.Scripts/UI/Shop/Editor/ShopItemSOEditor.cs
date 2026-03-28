using Code.Core.Defines;
using UnityEditor;

namespace Code.UI.Shop.Editor
{
    [CustomEditor(typeof(ShopItemSO))]
    public class ShopItemSOEditor : UnityEditor.Editor
    {
        private SerializedProperty _itemPriceProp;
        private SerializedProperty _itemTextureProp;
        private SerializedProperty _shopItemTypeProp;
        private SerializedProperty _fishPoolTypeProp;

        private void OnEnable()
        {
            _itemPriceProp = serializedObject.FindProperty("itemPrice");
            _itemTextureProp = serializedObject.FindProperty("itemTextures");
            _shopItemTypeProp = serializedObject.FindProperty("shopItemType");
            _fishPoolTypeProp = serializedObject.FindProperty("fishPoolTypes");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_itemPriceProp);
            EditorGUILayout.PropertyField(_itemTextureProp);
            EditorGUILayout.PropertyField(_shopItemTypeProp);

            if ((EnumDefine.ShopItemType)_shopItemTypeProp.enumValueIndex == EnumDefine.ShopItemType.Fish)
                EditorGUILayout.PropertyField(_fishPoolTypeProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}