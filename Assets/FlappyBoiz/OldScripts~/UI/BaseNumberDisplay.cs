using LitMotion.Animation;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public abstract class BaseNumberDisplay : MonoBehaviour
    {
        protected TMP_Text m_numberText;

        [SerializeField]
        protected IntVariable numberVariable;

        protected virtual void Awake()
        {
            m_numberText = GetComponent<TMP_Text>();
        }
        
        protected void OnEnable()
        {
            numberVariable.ChangedWithHistory?.Register(OnValueChanged);
        }

        protected void OnDisable()
        {
            numberVariable.ChangedWithHistory?.Unregister(OnValueChanged);
        }

        protected virtual void Start()
        {
            m_numberText.text = numberVariable.Value.ToString();
        }


        private void OnValueChanged(IntPair newValue) => OnValueChanged(newValue.Item1, newValue.Item2);

        private void OnValueChanged(int newValue) => OnValueChanged(newValue, -1);

        protected abstract void OnValueChanged(int newValue, int oldValue);
    }
}