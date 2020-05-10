namespace Util.Control
{
    //axes must match up with the names used in the input manager EXACTLY
    // used an enum rather than a string so that less invalid axes could be used
    public enum Axis
    {
        // needed as a placeholder
        None,
        // custom axes defined for use in my input mappings
        // a MouseDirection 2D axes control
        MouseDirection,
        // a 2D axes control using the arrow keys for horizontal and vertical input
        ArrowKeys,
        //  a 2D axes control using the w, a, s and d keys for horizontal and vertical input
        WASD,

        // custom axes added for controller, *Stick represents a 2D axes made up of x and y
        RightStick,
        RightStickX,
        RightStickY,
        LeftStick,
        LeftStickX,
        LeftStickY,
        RightTrigger,
        LeftTrigger
    }
}