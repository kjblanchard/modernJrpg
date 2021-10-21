using System;

public class BattleButtonBroadcaster
{

    public event ButtonPressedEventHandler ButtonPressedEvent;
    public event ButtonPressedEventHandler ButtonHoveredEvent;

    public delegate void ButtonPressedEventHandler(object sender, EventArgs e);


    public void OnButtonPressed()
    {
        ButtonPressedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnButtonHovered()
    {
        ButtonHoveredEvent?.Invoke(this,EventArgs.Empty);

    }

}
