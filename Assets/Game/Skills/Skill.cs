using System;
using UnityEngine;

[RequireComponent(typeof(Protagonist))]
public abstract class Skill : MonoBehaviour
{
    protected Protagonist player => this.GetComponent<Protagonist>();
    protected Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
}
