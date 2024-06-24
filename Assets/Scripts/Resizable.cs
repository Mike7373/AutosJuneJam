using System;
using UnityEngine;

public class Resizable : MonoBehaviour
{
    [Header("Size params")] [SerializeField]
    private float _defaultScale = 1f;
    [SerializeField] private float _shrinkScale = .2f;
    [SerializeField] private float _enlargeScale = 3f;
    [Header("Debug")]
    [SerializeField] private bool _isDebug;

    #region Unity Default Functions
    private void Start()
    {
        if (_defaultScale == 0f)
        {
            _defaultScale = transform.localScale.x;
        }
        else
        {
            ResetScale();
        }
    }
    private void Update()
    {
        if (_isDebug)
        {
            DebugResize();
        }
    }
    #endregion

    #region Resize Functions
    /// <summary>
    /// Shrink or enlarge the object dimension
    /// </summary>
    /// <param name="shrinking">If true use shrink scaling, else use enlarge scaling</param>
    public void Resize(bool shrinking)
    {
        if (shrinking)
        {
            transform.localScale = new Vector3(_shrinkScale, _shrinkScale, _shrinkScale);
        }
        else
        {
            transform.localScale = new Vector3(_enlargeScale, _enlargeScale, _enlargeScale);
        }
    }

    public void ResetScale()
    {
        transform.localScale = new Vector3(_defaultScale, _defaultScale, _defaultScale);
    }
    #endregion
    
    #region Debug
    private void DebugResize()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Resize(true);
            Debug.Log($"[{gameObject.name}] Shrink applied");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Resize(false);
            Debug.Log($"[{gameObject.name}] Enlarge applied");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScale();
            Debug.Log($"[{gameObject.name}] Scale reset applied");
        }
    }
    #endregion
}
