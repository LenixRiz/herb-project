#region Customer Events
public readonly struct CustomerArrivedEvent 
{
    public readonly CustomerDataSO CustomerData;

    public CustomerArrivedEvent(CustomerDataSO customerData)
    {
        CustomerData = customerData;
    }
}

public readonly struct CustomerAngryEvent
{
    public readonly float Reputation;

    public CustomerAngryEvent(float reputation)
    {
        Reputation = reputation;
    }
}
#endregion

#region Economic Events
// Write
public readonly struct GoldChangeEvent
{
    public readonly float GoldIncome;

    public GoldChangeEvent(float goldIncome)
    {
        GoldIncome = goldIncome;
    }
}

public readonly struct RepChangeEvent
{
    public readonly float RepGain;

    public RepChangeEvent(float repGain)
    {
        RepGain = repGain;
    }
}

// Read
public readonly struct GoldUpdateEvent
{
    public readonly float CurrentGold;
    public readonly float Delta;

    /// <summary>
    /// Notify any subscriber to change the UI component related to Gold
    /// </summary>
    /// <param name="currentGold"></param>
    /// <param name="delta"></param>
    public GoldUpdateEvent(float currentGold, float delta)
    {
        CurrentGold = currentGold;
        Delta = delta;
    }
}

public readonly struct RepUpdateEvent
{
    public readonly float RepGain;
    public readonly float Delta;
    public RepUpdateEvent(float currentRep, float delta)
    {
        RepGain = currentRep;
        Delta = delta;
    }
}
#endregion