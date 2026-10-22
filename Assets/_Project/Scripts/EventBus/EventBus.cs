using System;

public static class EventBus<T>
{
    private static event Action<T> OnEventRaised;

    public static void Subscribe(Action<T> listener)
    {
        OnEventRaised += listener;
    }

    public static void Unsubscribe(Action<T> listener)
    {
        OnEventRaised -= listener;
    }

    public static void Publish(T eventData)
    {
        OnEventRaised?.Invoke(eventData);
    }
}