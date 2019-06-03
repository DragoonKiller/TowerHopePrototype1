using System;
using UnityEngine;
using UnityEngine.UI;

public class DisasterIndicator : MonoBehaviour
{
    public DisasterManager dm;
    public Text text;
    
    void Update()
    {
        text.text = dm.display;
    }
}
