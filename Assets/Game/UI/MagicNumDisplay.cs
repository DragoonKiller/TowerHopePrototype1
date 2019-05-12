using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MagicNumDisplay : MonoBehaviour
{
    public PlayerState target;
    Text text => this.GetComponent<Text>();
    void Update()
    {
        text.text = "Magic: " + target.magic.ToString("0.0");
    }
}
