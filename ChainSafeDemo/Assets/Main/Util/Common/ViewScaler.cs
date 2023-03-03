using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_WEBGL
public class ViewScaler : MonoBehaviour
{
    private float _scaleX = 1;
    private float _scaleY = 1;
    void Update()
    {
        if (transform.parent != null)
        {
            RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
            float scaleX = parentRect.sizeDelta.x / 1920f;
            float scaleY = parentRect.sizeDelta.y / 1080f;

            if (scaleX != _scaleX || scaleY != _scaleY)
            {
                _scaleX = scaleX;
                _scaleY = scaleY;
                RectTransform childRect = transform.GetComponent<RectTransform>();
                childRect.localScale = new Vector3(_scaleX, _scaleY, 1f);
            }
        }
    }
}
#endif
