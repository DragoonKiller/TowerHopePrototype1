using System;
using UnityEngine;
using UnityEngine.UI;

public class DisasterTimer : MonoBehaviour
{
    public DisasterManager dm;
    public Text text;
    public string minFormat;
    public string secFormat;
    
    void Update()
    {
        text.text = ((int)dm.countdown / 60).ToString(minFormat) + ":" + ((int)dm.countdown % 60).ToString(secFormat);
    }
}
