using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private List<GameObject> wallInFrontOfPlayer;
    //[Header("Bounds")]
    //[SerializeField] private float leftBound;
    //[SerializeField] private float rightBound;
    private Vector3 target;
    private void Start()
    {
        target = transform.position - playerTransform.position;
    }
    private void LateUpdate()
    {
        //Vector3 move = Vector3.Lerp(transform.position, playerTransform.position + target, Time.deltaTime * speed);
        //move.x = Mathf.Clamp(move.x, leftBound, rightBound);
        //transform.position = move;
        checkWall();
    }
    private void checkWall()
    {
        Ray ray = new Ray(transform.position, playerTransform.position - transform.position);
        List<RaycastHit> hit = new List<RaycastHit>(Physics.RaycastAll(ray, 100, wallLayer));

        Debug.DrawLine(transform.position, playerTransform.position);
        if (hit.Count != 0)
        {
            for (int i = 0; i < hit.Count; i++)
            {
                if (!wallInFrontOfPlayer.Contains(hit[i].collider.gameObject))
                {
                    wallInFrontOfPlayer.Add(hit[i].collider.gameObject);
                    hit[i].collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        if (wallInFrontOfPlayer.Count != 0)
        {
            for (int i = 0; i < wallInFrontOfPlayer.Count; i++)
            {
                bool hasItem = false;
                for(int  j = 0; j < hit.Count; j++)
                {
                    if (wallInFrontOfPlayer[i] == hit[j].collider.gameObject)
                    {
                        hasItem = true;
                    }
                }
                if (!hasItem)
                {
                    wallInFrontOfPlayer[i].GetComponent<MeshRenderer>().enabled = true;
                    wallInFrontOfPlayer.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
