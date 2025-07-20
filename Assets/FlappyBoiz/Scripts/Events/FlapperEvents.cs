namespace FlappyBoiz.Scripts.Events
{
    public class FlapperPrimaryActionRequest : BaseEmptyEvent<FlapperPrimaryActionRequest>
    {
    }

    public class FlapperCollisionEvent : BaseEmptyEvent<FlapperCollisionEvent>
    {
    }

    public class FlapperCollectableEvent
    {
        public int Amount { get; private set; }

        public FlapperCollectableEvent(int amount)
        {
            Amount = amount;
        }
    }
}