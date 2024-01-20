namespace NEP.Paranoia.Scripts.Events;

/// <summary>
/// Base class for all events.
/// </summary>
public abstract class Event
{
    /// <summary>
    /// A list of every event.
    /// </summary>
    internal static readonly List<Event> Events = new();

    protected internal Event()
    {
        Events.Add(this);
    }

    ~Event()
    {
        Events.Remove(this);
    }
    
    /// <summary>
    /// Code to run when the event is invoked.
    /// </summary>
    public abstract void Invoke();
    
    /// <summary>
    /// If the event can be invoked.
    /// </summary>
    /// <returns>bool</returns>
    public abstract bool CanInvoke();
    
    /// <summary>
    /// Automatically creates a list of all events so event creation is more dynamic.
    /// </summary>
    public static void Initialize()
    {
        // reflection is weird
        foreach (var type in Main.CurrAsm.GetTypes())
        {
            if (type.IsAbstract || !type.IsSubclassOf(typeof(Event))) continue;
            Activator.CreateInstance(type);
        }
    }
}