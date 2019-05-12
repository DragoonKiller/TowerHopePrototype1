using System;
using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public abstract class Skill : MonoBehaviour
{
    protected PlayerState player => this.GetComponent<PlayerState>();
    protected Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
}
