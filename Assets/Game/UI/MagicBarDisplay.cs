using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MagicBarDisplay : MonoBehaviour
{
    public Inventory target;
    Slider slider => this.GetComponent<Slider>();
    void Update()
    {
        slider.value = target.carryingWand.curSlot.magic / target.carryingWand.curSlot.maxMagic;
    }
}
