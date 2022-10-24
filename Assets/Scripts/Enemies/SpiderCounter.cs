using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCounter
{
    public int Counter { get; private set; }

    public void Increase()
    {
        Counter++;
    }

    public void Deacrease()
    {
        Counter--;

        if(Counter <= 0)
            Counter = 0;
    }
}
