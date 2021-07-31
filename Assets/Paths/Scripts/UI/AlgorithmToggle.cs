using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Paths
{
    [RequireComponent(typeof(Toggle))]
    public class AlgorithmToggle : MonoBehaviour
    {
        [SerializeField] private TMP_Text _onLabel;
        [SerializeField] private TMP_Text _offLabel;

        private Toggle _toggle;

        public bool IsAStar => _toggle.isOn;

        public event Action<bool> StateChanged;

        public void Init(bool isAStar)
        {
            _toggle.isOn = isAStar;
        }

        private void OnEnable()
        {
            _toggle = GetComponent<Toggle>();
            OnValueChanged(_toggle.isOn);
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool state)
        {
            UpdateView(state);
            StateChanged?.Invoke(state);
        }

        private void UpdateView(bool state)
        {
            _onLabel.enabled = state;
            _offLabel.enabled = !state;
        }
    }
}