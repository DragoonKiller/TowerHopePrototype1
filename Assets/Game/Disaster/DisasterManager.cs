using System;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public Protagonist protagonist;
    public Disaster disaster;
    
    public string display;
    
    void Update()
    {
        switch(disaster)
        {
            case DisasterLavaflow c: display = (protagonist.transform.position.y - disaster.transform.position.y).ToString("0."); break;
            
            case null: break;
            default: throw new Exception("Unhandled disaster type for disaster manager.");
        }
    }
}
