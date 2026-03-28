using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.FSM
{
    [UnityEditor.CustomEditor(typeof(StateSO))]
    public class StateSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorUI = default;
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            editorUI.CloneTree(root);

            var dropdown = root.Q<DropdownField>("ClassDropdownField");
            CreateDropdown(dropdown);
            
            return root;
        }

        private void CreateDropdown(DropdownField dropdown)
        {
            dropdown.choices.Clear();
            
            var assembly = Assembly.GetAssembly(typeof(EntityState)); // Entity 든 Player 든 상관 x
            
            var derivedTypes = assembly.GetTypes()
                .Where(type => type.IsClass // 필요 없음 (IsSubclassOf가 이미 있어서)
                               && !type.IsAbstract
                               && type.IsSubclassOf(typeof(EntityState)))
                .Select(type => type.FullName) // FullName : 네임스페이스까지 포함한 이름, Name : 그냥 class 이름
                .ToList();
            
            dropdown.choices.AddRange(derivedTypes);
        }
    }
}