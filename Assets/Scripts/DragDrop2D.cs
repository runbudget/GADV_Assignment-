using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop2D : MonoBehaviour

{
    Vector3 offset;
    Collider2D collider2D;
    public string destinationTag = "DropArea";

     void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }


    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }
    void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
    }

     void OnMouseUp()
    {
        collider2D.enabled = false;
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition();
        RaycastHit2D hitInfo;
        if(hitInfo = Physics2D.Raycast(rayOrigin, rayDirection))
        {
            if(hitInfo.transform.tag == destinationTag)
            {
                transform.position = hitInfo.transform.position + new Vector3(0, 0, -0.01f);
            }
        }
        collider2D.enabled = true;

    }
    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
        
    }
    
}
