using UnityEngine;



public class RunCommand : ICommand
{


    Vector2 dir;



    public RunCommand(Vector2 controllerVector)
    {
        this.dir = controllerVector.normalized;
    }


    public void Execute(PlayerController player)
    {
        player.Run(dir);
    }


}
