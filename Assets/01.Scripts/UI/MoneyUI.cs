using Code.Player;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;

        private PlayerResourceManager _cashingResourceManager;
    
        private void Awake()
        {
            _cashingResourceManager = PlayerResourceManager.Instance;
            _cashingResourceManager.Money.Subscribe(SetMoneyText);
        }
    
        private void OnDestroy()
        {
            _cashingResourceManager.Money.Unsubscribe(SetMoneyText);
        }

        private void SetMoneyText(int money) => moneyText.text = money.ToString("#,##0");
    }
}