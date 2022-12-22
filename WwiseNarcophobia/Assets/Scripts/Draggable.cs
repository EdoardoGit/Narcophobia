using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool isDragging;

    public Vector3 LastPosition;

    private Collider2D collider;

    private DragController dragController;

    private float movementTime = 15f;
    private System.Nullable<Vector3> movementDestination;

    private float resetTime = 15f;
    private System.Nullable<Vector3> resetDestination;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        dragController = FindObjectOfType<DragController>();
    }

    private void FixedUpdate()
    {
        if (movementDestination.HasValue)
        {
            if (isDragging)
            {
                movementDestination = null;
                return;
            }

            if(transform.position == movementDestination)
            {
                gameObject.layer = Layer.Default;
                movementDestination = null;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, movementDestination.Value, movementTime * Time.fixedDeltaTime);
            }
        }

        if (resetDestination.HasValue)
        {
            if (isDragging)
            {
                resetDestination = null;
                return;
            }

            if (transform.position == resetDestination)
            {
                gameObject.layer = Layer.Default;
                resetDestination = null;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, resetDestination.Value, resetTime * Time.fixedDeltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DropValid"))
        {
            return;/* movementDestination = other.transform.position;*/
        }

        Draggable collidedDraggable = other.GetComponent<Draggable>();
        if(collidedDraggable != null && dragController.LastDragged.gameObject == gameObject && !other.CompareTag("DropValid"))
        {
            ColliderDistance2D colliderDistance2D = other.Distance(collider);
            Vector3 diff = new Vector3(colliderDistance2D.normal.x, colliderDistance2D.normal.y) * colliderDistance2D.distance;
            transform.position -= diff;
        }
    }

    public void ResetPosition()
    {
        resetDestination = LastPosition;
    }

    public void MovePosition()
    {
        movementDestination = transform.position;
    }
}
