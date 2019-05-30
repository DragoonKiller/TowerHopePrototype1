using System;
using UnityEngine;

public class ReviveBeacon : MonoBehaviour
{
    public static ReviveBeacon inst;
    ReviveBeacon() => inst = this;
}
