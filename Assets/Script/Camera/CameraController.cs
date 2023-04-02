using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float hiddenStep;
    private List<GameObject> wallInFrontOfPlayer = new List<GameObject>();
    private Vector3 target;
    private void Start()
    {
        target = transform.position - playerTransform.position;
    }
    private void LateUpdate()
    {
        CheckWall();
    }
    private void CheckWall()
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
                    StartCoroutine(Hidden(hit[i].collider.gameObject.GetComponent<Renderer>().material));
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
                    StartCoroutine(Visible(wallInFrontOfPlayer[i].GetComponent<Renderer>().material));
                    wallInFrontOfPlayer.RemoveAt(i);
                    i--;
                }
            }
        }

        
    }
    IEnumerator Hidden(Material _material)
    {
        for (float i = _material.color.a; i >= 0;)
        {
            i -= hiddenStep / 100;
            i = Mathf.Clamp(i, 0f, 1f);
            Color _color = _material.color;
            _color.a = i;
            _material.color = _color;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator Visible(Material _material)
    {
        for (float i = 0; i <= 100;)
        {
            i += hiddenStep / 100;
            i = Mathf.Clamp(i, 0f, 1f);
            Color _color = _material.color;
            _color.a = i;
            _material.color = _color;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
