using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Paths
{
    public class BoardPanel : MonoBehaviour
    {
        private const int MinValue = 2;
        private const int MaxValue = 100;

        public delegate void BoardCreateEventHandler(int width, int height);

        [SerializeField] private TMP_InputField _width;
        [SerializeField] private TMP_InputField _height;
        [SerializeField] private Button _createButton;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public event BoardCreateEventHandler CreateClicked;

        public void Init(int width, int height)
        {
            Width = width;
            Height = height;
            _width.text = width.ToString();
            _height.text = height.ToString();
        }

        private void OnEnable()
        {
            _createButton.onClick.AddListener(OnCreateClicked);
        }

        private void OnDisable()
        {
            _createButton.onClick.RemoveListener(OnCreateClicked);
        }

        private void OnCreateClicked()
        {
            Width = Parse(_width);
            Height = Parse(_height);

            CreateClicked?.Invoke(Width, Height);
        }

        private int Parse(TMP_InputField field)
        {
            if (int.TryParse(field.text, out int value))
                return Mathf.Clamp(value, MinValue, MaxValue);

            return MinValue;
        }

    }
}