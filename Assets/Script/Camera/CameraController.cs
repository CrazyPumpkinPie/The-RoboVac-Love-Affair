using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using UnityEngine;
using UnityEngine.Windows;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float hiddenStep;
    [SerializeField] private List<Wall> wallInFrontOfPlayer = new List<Wall>();
    private Vector3 target;
    private void Start()
    {
        target = transform.position - playerTransform.position;
    }
    private void LateUpdate()
    {
        CheckWall();
        for(int i = 0; i < wallInFrontOfPlayer.Count; i++)
        {
            ChangeAlphe(wallInFrontOfPlayer[i].wall, wallInFrontOfPlayer[i].isBeetweenPlayerAndCamera, 
                wallInFrontOfPlayer[i].wall.GetComponent<Renderer>().material, i);
        }
    }
    private void CheckWall()
    {
        Ray ray = new Ray(transform.position, playerTransform.position - transform.position);
        List<RaycastHit> hit = new List<RaycastHit>(Physics.RaycastAll(ray, 100, wallLayer));

        Debug.DrawLine(transform.position, playerTransform.position); 
        for (int i = 0; i < hit.Count; i++)
        {
            bool isHas = false;
            for (int j = 0; j < wallInFrontOfPlayer.Count; j++)
            {
                if (wallInFrontOfPlayer[j].wall == hit[i].collider.gameObject)
                {
                    isHas = true;
                    break;
                }
            }
            if (!isHas)
            {
                wallInFrontOfPlayer.Add(new Wall(hit[i].collider.gameObject, true));
            }
        }

        for(int i = 0; i < wallInFrontOfPlayer.Count; i++)
        {
            bool ishas = false;
            for(int j =0; j < hit.Count; j++)
            {
                if (wallInFrontOfPlayer[i].wall == hit[j].collider.gameObject)
                {
                    ishas = true;
                }
            }
            if (!ishas)
            {
                wallInFrontOfPlayer[i].isBeetweenPlayerAndCamera = false;
            }
        }


    }


    private void ChangeAlphe(GameObject _object, bool _isBeetweenPlayerAndCamera, Material _material, int index)
    {
        float step = hiddenStep / 100;
        Color color = _material.color;
        if(!_isBeetweenPlayerAndCamera && color.a == 1f)
        {
            wallInFrontOfPlayer.RemoveAt(index);
            return;
        }
        else if (_isBeetweenPlayerAndCamera && color.a > 0){
            color.a -= step;
            color.a = Mathf.Clamp(color.a, 0f, 1f);
            _material.color = color;
            return;
        }
        else if(!_isBeetweenPlayerAndCamera && color.a < 1f)
        {
            color.a += step;
            color.a = Mathf.Clamp(color.a, 0f, 1f);
            _material.color = color;
            return;
        }
    }
    //IEnumerator Hidden(Material _material)
    //{
    //    for (float i = _material.color.a; i >= 0;)
    //    {
    //        i -= hiddenStep / 100;
    //        i = Mathf.Clamp(i, 0f, 1f);
    //        Color _color = _material.color;
    //        _color.a = i;
    //        _material.color = _color;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}
    //IEnumerator Visible(Material _material)
    //{
    //    for (float i = 0; i <= 100;)
    //    {
    //        i += hiddenStep / 100;
    //        i = Mathf.Clamp(i, 0f, 1f);
    //        Color _color = _material.color;
    //        _color.a = i;
    //        _material.color = _color;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    class Wall
    {
        public GameObject wall;
        public bool isBeetweenPlayerAndCamera;
        public Wall()
        {

        }
        public Wall(GameObject _wall, bool _isBeetweenPlayerAndCamera)
        {
            wall = _wall; 
            isBeetweenPlayerAndCamera = _isBeetweenPlayerAndCamera;
        }
    }
}
