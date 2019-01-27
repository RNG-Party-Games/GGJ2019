using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHeat(float value) {
        GetComponent<RectTransform>().anchorMin = new Vector2(value, GetComponent<RectTransform>().anchorMin.y);
    }
}
