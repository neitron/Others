
public class InputHandler
{


    private JoyPad joypad;
    private NoCommand noCommande;
   


    public InputHandler(JoyPad joypad)
    {
        this.joypad = joypad;
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
