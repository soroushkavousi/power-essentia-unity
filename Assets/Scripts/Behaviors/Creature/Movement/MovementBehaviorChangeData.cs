public class MovementChangeData
{
    public MovementChangeState ChangeState { get; }

    public MovementChangeData(MovementChangeState changeState)
    {
        ChangeState = changeState;
    }
}

public enum MovementChangeState
{
    STARTED,
    STOPPED,
    REACHED
}
