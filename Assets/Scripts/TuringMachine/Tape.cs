using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Tape : MonoBehaviour
{
    List<GameObject> cells = new();
    float cellWidth = 0.5f;
    Vector3 initPos;
    public GameObject cell;
    public GameObject tapeWindow;

    public void init()
    {
        //Debug.Log(getProgramFromTapeWindow());
        generateCellsBy(getProgramFromTapeWindow());
    }
    string getProgramFromTapeWindow()
    {
        GameObject content = tapeWindow.transform.Find("Content/Viewport/Content").gameObject;
        int cellsNum = content.transform.childCount - 1;//去除CellTemp
        char[] program = new char[cellsNum];
        for (int i = 1; i <= cellsNum; i++)
        {
            program[i - 1] = content.transform.GetChild(i).gameObject.GetComponent<TMP_InputField>().text[0];
        }
        return new string(program);
    }
    void generateCellsBy(string program)
    {
        for (int i = 0; i < program.Length; i++)
        {
            char c = program[i];
            if (i >= cells.Count)
            {
                GameObject newCell = Instantiate(cell, transform.position + new Vector3(cellWidth * i, 0, 0), transform.rotation);
                newCell.transform.parent = transform;
                cells.Add(newCell);
            }
            ((GameObject)cells[i]).transform.Find("Text").GetComponent<TextMesh>().text = c.ToString();
        }
        for(int i = program.Length; i< cells.Count; i++)
        {
            ((GameObject)cells[i]).transform.Find("Text").GetComponent<TextMesh>().text = "空";
        }
    }
    public void leftMove()
    {
        transform.Translate(-cellWidth, 0, 0);
    }
    public void rightMove()
    {
        transform.Translate(cellWidth, 0, 0);
    }
    //更新指定位置的内容
    public void updateAt(int index, char c)
    {
        cells[index].transform.Find("Text").GetComponent<TextMesh>().text = c.ToString();
        Transform cellTransformInWindow;
        GameObject cellInWindow;
        try 
        {
            cellTransformInWindow = tapeWindow.transform.Find("Content/Viewport/Content").GetChild(index + 1);
            cellInWindow = cellTransformInWindow.gameObject;

        } catch (Exception e)
        {
            GameObject cellTemp = tapeWindow.transform.Find("Content/Viewport/Content/CellTemp").gameObject;
            cellInWindow = Instantiate(cellTemp, Vector3.zero, Quaternion.identity);
            cellInWindow.SetActive(true);
            cellInWindow.transform.SetParent(cellTemp.transform.parent, false);
            cellInWindow.transform.GetChild(0).GetComponent<TMP_Text>().text = (cells.Count - 1).ToString();
        }
        cellInWindow.GetComponent<TMP_InputField>().text = c.ToString();
    }
    //获取指定位置字符
    public char getCharAt(int index)
    {
        //Debug.Log(index + ":" + cells.Count);
        return ((GameObject)cells[index]).transform.Find("Text").GetComponent<TextMesh>().text[0];
    }
    //新增内容格
    public void addCell()
    {
        int newCellIndex = cells.Count;
        GameObject newCell = Instantiate(cell, transform.position + new Vector3(cellWidth * newCellIndex, 0, 0), transform.rotation);
        newCell.transform.parent = transform;
        cells.Add(newCell);
        cells[newCellIndex].transform.Find("Text").GetComponent<TextMesh>().text = "空";
    }
    public int getMaxRange()
    {
        return cells.Count - 1;
    }
    void Start()
    {
        initPos = transform.position;
    }
    public void resetM()
    {
        transform.position = initPos;
    }
}

