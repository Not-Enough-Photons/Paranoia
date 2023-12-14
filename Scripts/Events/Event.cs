namespace NEP.Paranoia.Events;

public abstract class Event
{
    public abstract void Invoke();
    
    public abstract bool CanInvoke();
}