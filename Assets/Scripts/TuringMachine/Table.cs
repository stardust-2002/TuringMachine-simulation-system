using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Table : MonoBehaviour
{
    public GameObject tableWindow;

    public class Transfer
    {
        public string currentState;
        public char currrentChar;
        public Movement[] movements;
        public string nextState;
        public class Movement
        {
            public Command command;
            public char c;
            public Movement(Command command, char c)
            {
                this.command = command;
                this.c = c;
            }
        }
        public enum Command
        {
            E, P, L, R
        }
        public Transfer(string cState, string cChar, string movements, string nState) 
        { 
            this.currentState = cState;
            this.currrentChar = cChar[0];
            this.movements = toMovents(movements);
            this.nextState = nState;
        }
        //由字符串转换为动作数组，字符串中每个动作的分隔符为空格。
        public Movement[] toMovents(string movementStr)
        {
            string[] movements = movementStr.Split(' ');
            Movement[] result = new Movement[movements.Length];
            for (int i = 0; i < movements.Length; i++)
            {
                Command command;
                char c = ' ';
                switch (movements[i][0])
                {
                    case 'E':
                        command = Command.E;
                        break;
                    case 'P':
                        command = Command.P;
                        c = movements[i][1];
                        break;
                    case 'L':
                        command = Command.L;
                        break;
                    case 'R':
                        command= Command.R;
                        break;
                    default:
                        return null;
                }
                result[i] = new Movement(command, c);
            }
            return result;
        }
    }
    public string startState;
    public string[] endState;
    List<Transfer> transferTable = new List<Transfer>();

    //转换表的初始化。
    public void init()
    {
        Transfer[] transfers = getTransfersFromTableWindow();
        tableClear();
        foreach (var transfer in transfers)
        {
            tableAdd(transfer);
        }
        string[] SEState = getSEState();
        startState = SEState[0];
        endState = SEState.Skip(1).ToArray();
    }
    Transfer[] getTransfersFromTableWindow()
    {
        GameObject content = tableWindow.transform.Find("Content/TableMain/Viewport/Content").gameObject;
        int transfersNum = content.transform.childCount - 1;//去除temp
        Transform transferLineTransform = null;
        Transfer[] transfers = new Transfer[transfersNum];
        for (int i = 1; i <= transfersNum; i++)
        {
            transferLineTransform = content.transform.GetChild(i);
            string cState = transferLineTransform.GetChild(0).GetComponent<TMP_InputField>().text;
            string cChar = transferLineTransform.GetChild(1).GetComponent<TMP_InputField>().text;
            string movements = transferLineTransform.GetChild(2).GetComponent<TMP_InputField>().text;
            string nState = transferLineTransform.GetChild(3).GetComponent<TMP_InputField>().text;
            transfers[i - 1] = new Transfer(cState, cChar, movements, nState);
            //Debug.Log(cState+":"+cChar+ ":" + movements + ":" + nState);
        }    
        return transfers;
    }
    string[] getSEState()
    {
        string startState = tableWindow.transform.Find("SEState/StartState/InputField").GetComponent<TMP_InputField>().text;
        string endState = tableWindow.transform.Find("SEState/EndState/InputField").GetComponent<TMP_InputField>().text;
        return (startState + " " + endState).Split(" ");
    }

    //返回转换表。
    public List<Transfer> getTable()
    {
        return transferTable;
    }

    //转换表新增一项
    public void tableAdd(Transfer transfer)
    {
        transferTable.Add(transfer);
    }

    //根据当前状态和当前指向字符，查找转换表，返回相应转换项，若未查找到，返回null。
    public Transfer transfer(string currentState, char currentChar)
    {
        foreach (var transfer in transferTable)
        {
            if (transfer.currentState == currentState && transfer.currrentChar == currentChar)
            {
                return transfer;
            }
        }
        return null;
    }

    //清空转换表
    public void tableClear()
    {
        transferTable.Clear();
    }
}
