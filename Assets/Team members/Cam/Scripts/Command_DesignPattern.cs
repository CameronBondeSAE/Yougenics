using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_DesignPattern : MonoBehaviour
{
    // Start is called before the first frame update
}


public class Command
{
    public virtual void Execute() {}
    public virtual void Execute(float axis) {}
}


public class JumpCommand : Command
{
    public override void Execute()
    {
        // gameActor.Jump();
    }
}


public class MoveCommand : Command
{
    public float amount;
    
    public override void Execute()
    {
        // gameActor.MoveX(amount);
    }
}

public class Playback
{
    List<Command> commands;

    public void Start()
    {
        foreach (Command command in commands)
        {
            command.Execute();
        }
    }
}