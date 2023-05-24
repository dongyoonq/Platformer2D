using UnityEngine;

public abstract class StateBase<TOwner> where TOwner : MonoBehaviour
{
    protected TOwner owner;

    protected StateBase(TOwner owner)
    {
        this.owner = owner;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}