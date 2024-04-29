using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWindow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void close()
    {
        gameObject.SetActive(false);
    }
}
