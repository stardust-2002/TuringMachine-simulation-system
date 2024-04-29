using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Table;
using static Table.Transfer;

public class TuringMachine : MonoBehaviour
{
    public Table table;
    public Tape tape;
    public Register register;
    public Header header;

    GameObject registerWindow = null;
    GameObject tableWindow = null;
    GameObject tapeWindow = null;

    bool isRunning = false;
    long frameNum = 0;
    Queue<Movement> movements = new();
    string nextState = null;
    int speedFactor = 30;

    //运行状态转换
    public void runOrStop()
    {
        isRunning = !isRunning;
    }

    //重启设备
    public void resetM()
    {
        init();
        tape.resetM();
        header.resetM();
        register.updateState(table.startState);
    }

    //根据各部件状态框内容初始化各部件
    void init()
    {
        //todo
        tape.init();
        register.init();
        table.init();
    }

    void Start()
    {
        init();
    }

    void Update()
    {
        frameNum++;
        //每10帧进行一次更新
        if (frameNum % speedFactor == 0 )
        {
            if (isRunning)
            {
                if(movements.Count == 0) 
                {
                    //更新状态
                    if(nextState != null) register.updateState(nextState);
                    //获取当前状态与指向字符
                    string currentState = register.getState();
                    char currentChar = header.getCurrentChar();
                    //获取转移项
                    Transfer transferLine = table.transfer(currentState, currentChar);
                    if (transferLine == null)
                    {
                        isRunning = false;
                        Debug.Log("停机");
                        return;
                    }
                    //添加动作到动作集合
                    foreach (var MM in transferLine.movements)
                    {
                        movements.Enqueue(MM);
                    }
                    nextState = transferLine.nextState;
                }
                //获取下一个动作并运行
                Movement movement = movements.Dequeue();
                switch (movement.command)
                {
                    case Transfer.Command.L:
                        header.leftMove();
                        break;
                    case Transfer.Command.R:
                        header.rightMove();
                        break;
                    case Transfer.Command.E:
                        header.erase();
                        break;
                    case Transfer.Command.P:
                        header.write(movement.c);
                        break;

                }
            }
            else
            {
                init();
            }
        }
    }
}
