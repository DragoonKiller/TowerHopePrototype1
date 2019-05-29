using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogData
{
    public string text;
    public float duration;
}

[RequireComponent(typeof(Text))]
public class Dialog : MonoBehaviour
{
    public static Dialog inst;
    
    public float fadePerSec;
    
    Queue<DialogData> data = new Queue<DialogData>();
    Text text => this.GetComponent<Text>();
    
    Dialog() => inst = this;
    
    public void AddDialog(string text, float duration)
    {
        data.Enqueue(new DialogData() { text = text, duration = duration });
    }
    
    void Update()
    {
        if(data.Count == 0) text.color = text.color.A(0f.Max(text.color.a - fadePerSec * Time.deltaTime));
        else text.color = text.color.A(1f);
        
        while(data.Count != 0 && data.Peek().duration <= 0f) data.Dequeue();
        if(data.Count != 0)
        {
            var cur = data.Peek();
            cur.duration -= Time.deltaTime;
            text.text = cur.text;
        }
    }
     
}
