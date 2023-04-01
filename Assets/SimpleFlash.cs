using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleFlash : MonoBehaviour
{
    [SerializeField] Text text;

    [SerializeField] Material newTextMaterial;
    [SerializeField] Material oldTextMaterial;
    [SerializeField] float BlinkFadeInTime = 0.5f;
    [SerializeField] float BlinkFadeStayTime = 0.8f;
    [SerializeField] float BlinkFadeOutTime = 0.7f;

    Color textColor;


    [SerializeField] float _timeCheker = 0;

    public void Start()
    {
        text = GetComponent<Text>();
        textColor = text.color;
        oldTextMaterial = text.material;
        newTextMaterial = GetComponent<Material>();
    }

    public void Update()
    {
        _timeCheker += Time.deltaTime;
        if (_timeCheker < BlinkFadeInTime)
        {
            //text.material = oldTextMaterial;
            text.color = textColor;
        }
        else if (_timeCheker < BlinkFadeInTime + BlinkFadeStayTime)
        {
            //text.material = newTextMaterial;
            text.color = Color.green;
        }
        else if (_timeCheker < BlinkFadeInTime + BlinkFadeStayTime + BlinkFadeOutTime)
        {
            //text.material = oldTextMaterial;
            text.color = textColor;
        }
        else
        {
            _timeCheker = 0;
        }
    }
}
