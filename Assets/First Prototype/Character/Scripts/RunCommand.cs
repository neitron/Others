using UnityEngine;



/// <summary>
/// Gets a
/// </summary>
public class RunCommand : ICommand
{


    readonly Quaternion controlVecOffset;
    Vector2 controlVec;



    public RunCommand(Vector2 controllerVector)
    {
        controlVecOffset = Quaternion.Euler(0.0f, 0.0f, -45.0f);
        this.controlVec = controllerVector.normalized;
    }


    public void Execute(PlayerController player)
    {
        controlVec = ( controlVecOffset * controlVec ).normalized;
        var newDir = new Vector3(controlVec.x, 0.0f, controlVec.y);

        player.Run(player.transform.position + newDir);
    }


}
