using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Player
{
    public string Name { get; private set; }
    public int Balance { get; set; }

    public Player(string name, int balance)
    {
        Name = name;
        Balance = balance;
    }
}
