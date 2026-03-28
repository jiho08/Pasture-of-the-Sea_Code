namespace Code.Core.EventChannel
{
    public static class AquaticEntityInfoUIEvents
    {
        public static readonly AquaticEntityClickEvent AquaticEntityClickEvent = new();
        public static readonly HideAquaticEntityInfoEvent HideAquaticEntityInfoEvent = new();
    }
    
    public class AquaticEntityClickEvent : GameEvent
    {
        public AquaticEntities.AquaticEntity aquaticEntity;
        
        public AquaticEntityClickEvent Initialize(AquaticEntities.AquaticEntity clickEntity)
        {
            aquaticEntity = clickEntity;
            return this;
        }
    }

    public class HideAquaticEntityInfoEvent : GameEvent
    {
    }
}