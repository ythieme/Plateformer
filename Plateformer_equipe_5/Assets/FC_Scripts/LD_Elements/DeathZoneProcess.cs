using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneProcess : MonoBehaviour
{
    public FearScript_FC fear;
    public BoxCollider2D box;
        
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        fear = GameObject.Find("FearObject").GetComponent<FearScript_FC>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fear.fear = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(box.bounds.center.x - box.bounds.extents.x, box.bounds.center.y + box.bounds.extents.y), 
            new Vector2(box.bounds.center.x + box.bounds.extents.x, box.bounds.center.y + box.bounds.extents.y)); //haut
        Gizmos.DrawLine(new Vector2(box.bounds.center.x - box.bounds.extents.x, box.bounds.center.y + box.bounds.extents.y),
            new Vector2(box.bounds.center.x - box.bounds.extents.x, box.bounds.center.y - box.bounds.extents.y));//gauche
        Gizmos.DrawLine(new Vector2(box.bounds.center.x - box.bounds.extents.x, box.bounds.center.y - box.bounds.extents.y),
            new Vector2(box.bounds.center.x + box.bounds.extents.x, box.bounds.center.y - box.bounds.extents.y));//bas
        Gizmos.DrawLine(new Vector2(box.bounds.center.x + box.bounds.extents.x, box.bounds.center.y - box.bounds.extents.y),
            new Vector2(box.bounds.center.x + box.bounds.extents.x, box.bounds.center.y + box.bounds.extents.y));//droite
    }
}
