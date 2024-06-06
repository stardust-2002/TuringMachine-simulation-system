using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
    public GameObject infoWindow;

    public void Start()
    {
        defaultMode();
    }
    //弹出窗口
    public void popWindow(string info)
    {
        infoWindow.SetActive(true);
        infoWindow.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = info;
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
            programInputActiveStatus = true;
            programInputResizeButton.GetComponent<Image>().sprite = min;
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
        for(int i = 0; i < program.Length; i++)
        {
            char ch = program[i];
            if (ch != '\n')
            {
                GameObject cell = Instantiate(cellTemp, Vector3.zero, Quaternion.identity);
                cell.SetActive(true);
                cell.transform.SetParent(cellTemp.transform.parent, false);
                cell.GetComponent<TMP_InputField>().text = ch.ToString();
                cell.transform.GetChild(0).GetComponent<TMP_Text>().text = i.ToString();
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
        setStateWritepermissions(false);
        UTM();
    }
    public void UTM()
    {
        string startState = "start";
        string endState = "end";
        string[,] transfers = new string[54, 4]
        {
            { "start", "0", "R", "t1" },
            { "start", "1", "R", "t1" },
            { "t1", "0", "R", "t1" },
            { "t1", "1", "R", "t1" },
            { "t1", ":", "P1 R", "t2" },
            { "t2", ":", "P0", "q1" },
            { "q1", "0", "R", "q5" },
            { "q1", "1", "R", "q2" },
            { "q2", "0", "P1 R", "q1" },
            { "q2", "1", "L", "q3" },
            { "q3", "0", "L", "q4" },
            { "q3", "1", "P0 L", "q2" },
            { "q4", "0", "P1 L", "q12" },
            { "q4", "1", "P0 L", "q9" },
            { "q5", "0", "P1 R", "q1" },
            { "q5", "1", "P0 L", "q6" },
            { "q6", "0", "L", "q7" },
            { "q6", "1", "L", "q7" },
            { "q7", "0", "L", "q8" },
            { "q7", "1", "P0 L", "q6" },
            { "q8", "0", "L", "q7" },
            { "q8", "1", "R", "q2" },
            { "q9", "0", "R", "q19" },
            { "q9", "1", "L", "q4" },
            { "q10", "0", "P1 L", "q4" },
            { "q10", "1", "P0 R", "q13" },
            { "q11", "0", "L", "q4" },
            { "q11", "1", "P1", "end" },
            { "q12", "0", "R", "q19" },
            { "q12", "1", "L", "q14" },
            { "q13", "0", "R", "q10" },
            { "q13", "1", "R", "q24" },
            { "q14", "0", "L", "q15" },
            { "q14", "1", "L", "q11" },
            { "q15", "0", "R", "q16" },
            { "q15", "1", "R", "q17" },
            { "q16", "0", "R", "q15" },
            { "q16", "1", "R", "q10" },
            { "q17", "0", "R", "q16" },
            { "q17", "1", "R", "q21" },
            { "q18", "0", "R", "q19" },
            { "q18", "1", "R", "q20" },
            { "q19", "0", "P1 L", "q3" },
            { "q19", "1", "R", "q18" },
            { "q20", "0", "P1 R", "q18" },
            { "q20", "1", "P0 R", "q18" },
            { "q21", "0", "R", "q22" },
            { "q21", "1", "R", "q23" },
            { "q22", "0", "P1 L", "q10" },
            { "q22", "1", "R", "q21" },
            { "q23", "0", "P1 R", "q21" },
            { "q23", "1", "P0 R", "q21" },
            { "q24", "0", "R", "q13" },
            { "q24", "1", "P0 L", "q3" }
        };
        setRegisterWindow(startState);
        setTableWindow(transfers, startState, endState);
    }
    public void binaryAddition()
    {
        string startState = "start";
        string endState = "end";
        string[,] transfers = new string[7, 4]
        {
            { "start", "0", "R", "start" },
            { "start", "1", "R", "q1" },
            { "q1", "0", "P1 R", "q2" },
            { "q1", "1", "R", "q1" },
            { "q2", "0", "L P0", "end" },
            { "q2", "1", "R", "q2" },
            { "q2", "空", "P0", "q2" },
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
        tableWindow.transform.Find("SEState/StartState/InputField").GetComponent<TMP_InputField>().text = startState;
        tableWindow.transform.Find("SEState/EndState/InputField").GetComponent<TMP_InputField>().text = endState;
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
        setStateWritepermissions(true);
        setRegisterWindow("");
        clearTableWindow();
        clearTapeWindow();
    }
    void setStateWritepermissions(bool canWrite)
    {
        List<GameObject> inputs = FindObjects("StateWindowInput");
        foreach (var input in inputs)
        {
            if (canWrite)
                input.GetComponent<TMP_InputField>().readOnly = false;
            else
            {
                input.GetComponent<TMP_InputField>().readOnly = true;
            }
        }
    }
    
    private List<GameObject> FindObjects(string tag)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {

                if (go.tag == tag)
                    gameObjects.Add(go);
        }
        return gameObjects;
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
    //图灵机运行速度增加
    public void speedUp()
    {
        turingMachine.GetComponent<TuringMachine>().speedUp();
    }
    //图灵机运行速度减小
    public void speedDown()
    {
        turingMachine.GetComponent<TuringMachine>().speedDown();
    }
    //规则表状态窗口新增一行空白规则项
    public void addNewInTableWindow()
    {
        GameObject transferLineTemp = tableWindow.transform.Find("Content/TableMain/Viewport/Content/TransferLineTemp").gameObject;
        GameObject transferLine = Instantiate(transferLineTemp, Vector3.zero, Quaternion.identity);
        transferLine.SetActive(true);
        transferLine.transform.SetParent(transferLineTemp.transform.parent, false);
    }
}
