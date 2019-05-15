using System;
using UnityEngine;

[RequireComponent(typeof(Protagonist))]
public abstract class Skill : MonoBehaviour
{
    public SkillSpec spec;
    public SkillConfig config;
    
    protected Protagonist protagonist => this.GetComponent<Protagonist>();
    protected ContactDetector contactDetector => this.GetComponent<ContactDetector>();
    protected Rigidbody2D rd => this.GetComponent<Rigidbody2D>();
}
