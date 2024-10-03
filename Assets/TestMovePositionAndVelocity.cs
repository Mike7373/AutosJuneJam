using UnityEngine;

public class TestMovePositionAndVelocity : MonoBehaviour
{
    Rigidbody rigidBody;
    void Start()
    {
        Debug.Log("Questo test controlla se utilizzando la rigidBody.movePosition viene aggiornata anche la posizione");

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = Vector3.right;
    }

    void FixedUpdate()
    {
        Debug.Log($"Velocità: {rigidBody.velocity}");
        
        rigidBody.MovePosition(rigidBody.position + Vector3.right*Time.deltaTime);
        
        Debug.Log($"Velocità: {rigidBody.velocity}");

        
        //Dovrebbe darmi 1m/s
    }
}
