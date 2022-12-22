using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{

    public Draggable LastDragged => lastDragged;

    private bool isDragActive = false;

    private Vector2 screenPosition;
    private Vector3 wordPosition;
    private Draggable lastDragged;
    private Collider2D lastValid;

    private void Awake()
    {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if (controllers.Length > 1)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if(isDragActive)
        {
            if (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                Drop();
                return;
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            screenPosition = new Vector2(mousePos.x, mousePos.y);
        }
        else if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
        }
        else
        {
            return;
        }
        wordPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (isDragActive)
        {
            Drag();
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(wordPosition, Vector2.zero);
            if (hit.collider != null)
            {
                Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>();
                if(draggable != null)
                {
                    lastDragged = draggable;
                    InitDrag();
                }
            }
        }
    }

    void InitDrag()
    {
        lastDragged.LastPosition = lastDragged.transform.position;
        UpdateDragStatus(true);
    }

    void Drag()
    {
        lastDragged.transform.position = new Vector2(wordPosition.x, wordPosition.y);
    }

    void Drop()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(wordPosition, Vector2.zero);
        bool checkHit = false;
        foreach (RaycastHit2D hit in hits)
        {

            if (hit.collider.CompareTag("DropValid"))
            {
                lastValid = hit.collider;
                Bounds b = lastDragged.GetComponent<Collider2D>().bounds;
                Vector2 upL = new Vector2(b.min.x, b.max.y);
                Vector2 upR = b.max;
                Vector2 botL = b.min;
                Vector2 botR = new Vector2(b.max.x, b.min.y);
                if(hit.collider.bounds.Contains(upL) && hit.collider.bounds.Contains(upR) && hit.collider.bounds.Contains(botL) && hit.collider.bounds.Contains(botR))
                {
                    checkHit = true;
                }
            }
        }
        if (!checkHit)
            lastDragged.ResetPosition();
        else
            lastDragged.MovePosition();
        UpdateDragStatus(false);
    }

    void UpdateDragStatus(bool isDragging)
    {
        isDragActive = lastDragged.isDragging = isDragging;
        lastDragged.gameObject.layer = isDragging ? Layer.Dragging : Layer.Default;
    }

    public bool checkReposition(Collider2D collider)
    {
        Bounds b = collider.bounds;
        Vector2 upL = new Vector2(b.min.x, b.max.y);
        Vector2 upR = b.max;
        Vector2 botL = b.min;
        Vector2 botR = new Vector2(b.max.x, b.min.y);
        return (lastValid.bounds.Contains(upL) && lastValid.bounds.Contains(upR) && lastValid.bounds.Contains(botL) && lastValid.bounds.Contains(botR));
    }
}
