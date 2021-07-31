using UnityEngine;

namespace Paths
{
    [RequireComponent(typeof(Camera))]
    public class CameraPlacer : MonoBehaviour
    {
        private const float PaddingTop = 0.15f;

        public void Place(int width, int height)
        {
            var camera = GetComponent<Camera>();

            var h = height * 0.5f + PaddingTop * height;
            var w = width * 0.5f / camera.aspect;

            camera.orthographicSize = Mathf.Max(h, w);

            transform.rotation = Quaternion.Euler(90, 0, 0);
            transform.position = new Vector3(
                width * 0.5f - 0.5f, 
                10, 
                height * 0.5f - 0.5f + PaddingTop * height);
        }
    }
}