using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmaDaFuoco : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject projectileMarkerDebug;
    [SerializeField] GameObject luceMirino;
    [SerializeField] Texture2D textureCursorMirino;
    
    [SerializeField] EventReference gunshotSound;

    void Start()
    {
        Cursor.SetCursor(textureCursorMirino, new Vector2(textureCursorMirino.width/2.0f, textureCursorMirino.height/2.0f), CursorMode.Auto);
        
    }
    
    void Update()
    {
        var ray = playerCamera.ScreenPointToRay(Mouse.current.position.value);
        Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow);
        
        if (Physics.Raycast(ray, out var hit))
        {
            transform.LookAt(hit.point);

            var pistolRay = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(pistolRay, out hit))
            {
                luceMirino.SetActive(true);
                luceMirino.transform.position = hit.point-ray.direction*0.1f;
                luceMirino.transform.LookAt(hit.point);
            }
            /*
            hit.textureCoord;
            hit.textureCoord2;
            hit.point;
            hit.collider;
            hit.barycentricCoordinate;
            hit.triangleIndex;
            hit.lightmapCoord;*/
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log($"Colpito: {hit.transform.name}");
                Instantiate(projectileMarkerDebug,hit.point, Quaternion.identity);

                var soundInst = RuntimeManager.CreateInstance(gunshotSound);
                soundInst.start();
                soundInst.release();
            }
        }
        else
        {
            luceMirino.SetActive(false);
        }

    }
}
