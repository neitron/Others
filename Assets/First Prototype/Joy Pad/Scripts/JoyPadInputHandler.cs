using UnityEngine;



[RequireComponent(typeof(JoyPad))]
public class JoyPadInputHandler : MonoBehaviour
{


    private JoyPad joypad;
    private NoCommand noCommande;
    


    public void Start()
    {
        joypad = GetComponent<JoyPad>();
        
        noCommande = new NoCommand();
    }


    public ICommand HandleInput()
    {
        if (joypad.GetJosticDrag())
        {
            return new RunCommand(joypad.GetJosticPosition());
        }

        return noCommande;
    }


}
