using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Register : MonoBehaviour
{
    string state;
    public GameObject registerWindow;

    public void init()
    {
        string state = registerWindow.transform.Find("Content/State").gameObject.GetComponent<TMP_InputField>().text;
        setState(state);
    }

    public string getState()
    {
        return state;
    }

    public void setState(string state)
    {
        this.state = state;
    }

    public void updateState(string state)
    {
        setState(state);
        registerWindow.transform.Find("Content/State").gameObject.GetComponent<TMP_InputField>().text = state;
    }
}
