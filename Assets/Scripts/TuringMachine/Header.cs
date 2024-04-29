using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Header : MonoBehaviour
{
    public Tape tape;

    int position = 0;

    public int getPosition()
    {
        return position;
    }

    //返回当前指向的字符
    public char getCurrentChar()
    {
        return tape.getCharAt(position);
    }
    
    public void leftMove()
    {
        if (position > 0)
        {
        //纸带右移
        tape.rightMove();
        //位置减1
        position--;
        }

    }
    public void rightMove()
    {
        //Debug.Log("postion after move:" + position + "  " + tape.getCellsNum());
        if(position >= tape.getMaxRange())
        {
            tape.addCell();
        }
        //纸带左移
        tape.leftMove();
        //位置加1
        position++;
    }
    public void write(char value)
    {
        tape.updateAt(position, value);
    }
    public void erase()
    {
        tape.updateAt(position, ' ');
    }

    public void resetM()
    {
        position = 0;
    }
}
