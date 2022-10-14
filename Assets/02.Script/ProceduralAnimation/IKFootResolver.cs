using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootResolver : MonoBehaviour
{
    [SerializeField] float footSpacing = 2.0f;
    [SerializeField] float stepDistance = 2.0f;
    [SerializeField] Transform body;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float speed = 1;
    [SerializeField] float stepHeight = 2;

    float lerp;
    Vector3 newPosition;
    Vector3 currentPosition;
    Vector3 oldPosition;

    private void Start()
    {
        newPosition = this.transform.position;
        oldPosition = this.transform.position;
        currentPosition = this.transform.position;
    }
    void Update()
    {
        transform.position = currentPosition;

        Ray ray = new Ray(body.position + (body.right * footSpacing)+body.forward*-2, Vector3.down);
        if(Physics.Raycast(ray,out RaycastHit info, 10, groundLayer))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance)
            {
                lerp = 0;
                newPosition = info.point;
            }
        }

        if (lerp < 1)
        {
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }
}
