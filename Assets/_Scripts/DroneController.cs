using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Vector2 offset;
    public float xScale;

    private float horizontalAxis;
    private float verticalAxis;

    [SerializeField] private Transform crest;
    [SerializeField] private Transform body;
    
    private Vector3 mousePosition;
    public float moveSpeed = 2f;
    
    void Follow()
    {

        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        if (horizontalAxis < 0) xScale = -1;
        if (horizontalAxis > 0) xScale = 1;
        
        Vector3 newPos = target.position;
        newPos.z = target.position.z;
        newPos.x = target.position.x + offset.x;
        newPos.y = target.position.y + offset.y;

        crest.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,new Vector3(0, 0, horizontalAxis * -30f), 100 * Time.deltaTime));
        
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        Follow();
        LookOnCursor();
    }
    
    void LookOnCursor(){ //заставляет свет следить за курсором мышки
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = body.position.z;
        mousePos.y += body.position.y - Camera.main.transform.position.y; //расстояние между камерой и объектом
        //print(mousePos);
        
        //print(mousePos);

        body.LookAt(mousePos);
    }
}
