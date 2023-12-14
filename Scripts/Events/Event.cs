namespace NEP.Paranoia.Events;

public abstract class Event
{
    protected Event() { }

    public abstract void Invoke();
}