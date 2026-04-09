using UnityStandardAssets.Characters.FirstPerson;

public enum Direction
{
    left,
    right,
}
public abstract class Command
{
    public FirstPersonController controller;
    public float timeStamp;

    public abstract void Execute();
}

public class MoveLeft : Command
{
    public MoveLeft(FirstPersonController targetPlayer)
    {
        controller = targetPlayer;
    }

    public override void Execute()
    {
        controller.Move();
    }
}
public class Jump : Command
{
    public Jump(FirstPersonController targetPlayer)
    {
        controller = targetPlayer;
    }

    public override void Execute()
    {
        controller.TryJump();
    }
}
