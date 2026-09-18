using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EdgeCollider2D))]
public class LinearHitDetection : MonoBehaviour
{
    [SerializeField]
    private LayerMask _hitableLayer;
    [SerializeField]
    private UnityEvent _onEnter;
    [SerializeField]
    private UnityEvent _onExit;

    private EdgeCollider2D edge;
    
    void Awake()
    {
        edge = GetComponent<EdgeCollider2D>();
        edge.enabled = false;
    }

    void Update()
    {
        var hit = Physics2D.Linecast((Vector2)transform.position + edge.points[0], (Vector2)transform.position + edge.points[1], _hitableLayer);

        if(hit)
        {
            _onEnter?.Invoke();
        }
        else
        {
            _onExit?.Invoke();
        }
    }
}
