using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tape : MonoBehaviour
{
    ArrayList cells = new ArrayList();
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
        ((GameObject)cells[index]).transform.Find("Text").GetComponent<TextMesh>().text = c.ToString();
        tapeWindow.transform.Find("Content/Viewport/Content").GetChild(index + 1).GetComponent<TMP_InputField>().text = c.ToString();
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
        ((GameObject)cells[newCellIndex]).transform.Find("Text").GetComponent<TextMesh>().text = " ";
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

