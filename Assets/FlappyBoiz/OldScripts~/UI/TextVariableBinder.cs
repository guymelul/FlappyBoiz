using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public class TextVariableBinder : MonoBehaviour
    {
        [SerializeField]
        private StringVariable variable;

        private TMP_Text m_text;

        public void Awake()
        {
            m_text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            if (!variable) return;

            variable.Changed?.Register(OnStringVariableChanged);
        }

        private void OnDisable()
        {
            if (!variable) return;

            variable.Changed?.Unregister(OnStringVariableChanged);
        }

        private void OnStringVariableChanged()
        {
            if (!variable) return;

            m_text.text = variable.Value;
        }
    }
}