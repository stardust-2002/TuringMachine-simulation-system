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
    public InputProcesser inputProcesser;

    bool isRunning = false;
    long frameNum = 0;
    int speedFactor = 60;
    Queue<Movement> movements = new();
    string nextState = null;

    public void speedUp()
    {
        if (speedFactor >= 30)
            speedFactor -= 15;
    }

    public void speedDown()
    {
        if (speedFactor < 90)
            speedFactor += 15;
    }

    //运行状态转换
    public void runOrStop()
    {
        isRunning = !isRunning;
    }

    //重启设备
    public void resetM()
    {
        nextState = null;
        movements.Clear();
        init();
        tape.resetM();
        header.resetM();
        register.updateState(table.startState);
    }

    //根据各部件状态框内容初始化各部件
    void init()
    {
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
        //特定帧进行一次更新
        if (isRunning)
        {
            if (frameNum % speedFactor == 0)
            {
                if (movements.Count == 0)
                {
                    //更新状态
                    if (nextState != null) register.updateState(nextState);
                    //获取当前状态与指向字符
                    string currentState = register.getState();
                    char currentChar = header.getCurrentChar();
                    //获取转移项
                    Transfer transferLine = table.convert(currentState, currentChar);
                    if (transferLine == null)
                    {
                        nextState = null;
                        isRunning = false;
                        inputProcesser.popWindow("停机");
                        return;
                    }
                    //添加动作到动作队列
                    foreach (var MM in transferLine.movements)
                    {
                        movements.Enqueue(MM);
                    }
                    //更新下一个状态
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

        }
        else if(frameNum % 20 == 0)
        {
            init();
        }
    }
}
