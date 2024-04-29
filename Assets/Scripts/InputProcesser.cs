using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputProcesser : MonoBehaviour
{
    public GameObject canvas;
    public GameObject programInputResizeButton;
    public GameObject programInputHelpButton;
    public GameObject programInputBox;
    public GameObject turingMachine;
    public GameObject defaultModeButton;
    public GameObject freeModeButton;


    bool programInputActiveStatus = true;
    public GameObject programInputHelpWindow;
    public GameObject registerWindow;
    public GameObject tableWindow;
    public GameObject tapeWindow;

    public void Start()
    {
        defaultMode();
    }

    //todo 弹出窗口
    void initWindow(ref GameObject window, GameObject windowPrefab)
    {
        window = Instantiate(windowPrefab, Vector3.zero, Quaternion.identity);
        window.SetActive(false);
        window.transform.SetParent(canvas.transform, false);
    }

    public void programInputResize()
    {
        Sprite max = Resources.Load<Sprite>("maximize");
        Sprite min = Resources.Load<Sprite>("minimize");
        if (programInputActiveStatus)
        {//隐藏输入框
            programInputBox.SetActive(false);
            programInputActiveStatus = false;
            programInputResizeButton.GetComponent<Image>().sprite = max;
        }
        else
        {//显示输入框
            programInputBox.SetActive(true);
            programInputActiveStatus=true;
            programInputResizeButton.GetComponent <Image>().sprite = min;
        }
    }
    public void programInputHelp()
    {
        programInputHelpWindow.SetActive(true);
    }

    public void programSubmit()
    {
        clearTapeWindow();
        string program = GameObject.Find("ProgramInputField").GetComponent<TMP_InputField>().text;
        GameObject cellTemp = tapeWindow.transform.Find("Content/Viewport/Content/CellTemp").gameObject;
        foreach (var ch in program)
        {
            if (ch != '\n')
            {
                GameObject cell = Instantiate(cellTemp, Vector3.zero, Quaternion.identity);
                cell.SetActive(true);
                cell.transform.SetParent (cellTemp.transform.parent, false);
                cell.GetComponent<TMP_InputField>().text = ch.ToString();
            }
        }
    }
    void clearTapeWindow()
    {
        GameObject content = tapeWindow.transform.Find("Content/Viewport/Content").gameObject;
        Transform transform;
        for (int i = 1; i < content.transform.childCount; i++)
        {
            transform = content.transform.GetChild(i);
            GameObject.Destroy(transform.gameObject);

        }

    }

    public void registerStateWindow()
    {
        registerWindow.SetActive(true);
    }

    public void tableStateWindow()
    {
        tableWindow.SetActive(true);
    }

    public void tapeStateWindow()
    {
        tapeWindow.SetActive(true);
    }

    public void runOrStop()
    {
        turingMachine.GetComponent<TuringMachine>().runOrStop();
    }

    public void resetM()
    {
        turingMachine.GetComponent<TuringMachine>().resetM();
    }

    public void defaultMode()
    {
        selectMode(defaultModeButton);
        string startState = "start";
        string endState = "end";
        string[,] transfers = new string[2,4]
        {
            { "start", "1", "P0 L R R", "s0" },
            { "s0", "1", "P0 L R R", "s0"}
        };
        setRegisterWindow(startState);
        setTableWindow(transfers, startState, endState);
    }
    void setRegisterWindow(string state)
    {
        registerWindow.transform.Find("Content/State").GetComponent<TMP_InputField>().text = state;
    }
    void setTableWindow(string[,] transfers, string startState, string endState)
    {
        clearTableWindow();
        GameObject transferLineTemp = tableWindow.transform.Find("Content/TableMain/Viewport/Content/TransferLineTemp").gameObject;
        for (int i = 0; i < transfers.GetLength(0); i++)
        {
            GameObject transferLine = Instantiate(transferLineTemp, Vector3.zero, Quaternion.identity);
            transferLine.SetActive(true);
            transferLine.transform.SetParent(transferLineTemp.transform.parent, false);
            for (int j = 0; j < 4; j++)
            {
                //Debug.Log(i + ":" + j);
                transferLine.transform.GetChild(j).GetComponent<TMP_InputField>().text = transfers[i, j];
            }
            
        }
        tableWindow.transform.Find("SEState/StartState/InputField").GetComponent<TMP_InputField>().text= startState;
        tableWindow.transform.Find("SEState/EndState/InputField").GetComponent<TMP_InputField>().text= endState;
    }
    void clearTableWindow()
    {
        GameObject content = tableWindow.transform.Find("Content/TableMain/Viewport/Content").gameObject;
        Transform transform;
        for (int i = 1; i < content.transform.childCount; i++)
        {
            transform = content.transform.GetChild(i);
            GameObject.Destroy(transform.gameObject);

        }
    }

    public void freeMode()
    {
        selectMode(freeModeButton);
    }

    void selectMode(GameObject button)
    {
        //Debug.Log("select");
        if(button == defaultModeButton)
        {
            defaultModeButton.GetComponent<Image>().color = new Color32(184,248,194,255);
            freeModeButton.GetComponent <Image>().color = Color.white;
        }
        else
        {
            defaultModeButton.GetComponent<Image>().color = Color.white;
            freeModeButton.GetComponent<Image>().color = new Color32(184, 248, 194, 255);
        }
    }
}
