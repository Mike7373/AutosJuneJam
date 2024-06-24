using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _crosshairSprite;
    private void Update()
    {
        // Vector2 crosshairScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        // Vector2 mousePos = Input.mousePosition;
        // Vector2 screenResult = mousePos - crosshairScreenPos;
        //
        // float angle = Vector2.SignedAngle(Vector2.right, screenResult);
        // Debug.Log(Vector2.SignedAngle(Vector2.right, screenResult));
        // transform.localRotation = Quaternion.AngleAxis(-angle, Vector3.left);

        Vector3 startingVector = new(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z);
        Debug.Log(Camera.main.ScreenToWorldPoint(startingVector));
        
        //Debug.Log($"[{gameObject.name}] Rotation: {transform.rotation}");
        
    }
}
