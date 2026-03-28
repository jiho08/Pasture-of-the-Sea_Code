using Code.Core.Defines;
using Code.Core.Pool;
using UnityEngine;

namespace Code.UI.Shop
{
    [CreateAssetMenu(fileName = "ShopItem", menuName = "SO/Shop/Item", order = 0)]
    public class ShopItemSO : ScriptableObject
    {
        public int itemPrice;
        public RenderTexture[] itemTextures;
        public EnumDefine.ShopItemType shopItemType;
        public PoolTypeSO[] fishPoolTypes;
    }
}