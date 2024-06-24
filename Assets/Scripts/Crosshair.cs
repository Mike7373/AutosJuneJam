using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private float _clampArc = 120;
    [SerializeField] private SpriteRenderer _crosshairSprite;
    public static Crosshair Instance;

    #region Unity Default Functions

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    private void Start()
    {
        ToggleCrosshair(false);
    }

    private void Update()
    {
        TakeAim();
        if (Input.GetMouseButtonDown(0))
        {
            ToggleCrosshair(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ToggleCrosshair(false);
        }
    }

    #endregion

    #region Crosshair Functions

    public static void ToggleCrosshair(bool toggle)
    {
        Instance._crosshairSprite.enabled = toggle;
    }

    public void TakeAim()
    {
        Vector2 crosshairScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenResult = mousePos - crosshairScreenPos;

        float parentRotation = transform.parent.localRotation.y;
        float angle = Vector2.SignedAngle(Vector2.right, screenResult);
        
        ClampAimAngle(angle, parentRotation, out float newAngle);
        angle = newAngle;
        
        Debug.Log(Vector2.SignedAngle(Vector2.right, screenResult));
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.left);
        Debug.Log(angle);
    }

    public void ClampAimAngle(float angle, float parentRotation, out float newAngle)
    {
        if (parentRotation < 0)
        {
            if (angle < 180-_clampArc/2 && angle > 0)
            {
                angle = 180-_clampArc/2;
            }

            if (angle > -(180-_clampArc/2) && angle < 0)
            {
                angle = -(180-_clampArc/2);
            }
            angle = -angle + 180;
        }
        else
        {
            if (angle > _clampArc/2)
            {
                angle = _clampArc/2;
            }

            if (angle < -_clampArc/2)
            {
                angle = -_clampArc/2;
            }
        }
        newAngle = angle;
    }
    
    #endregion
}
