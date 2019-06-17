namespace CockpitBuilder.Events
{
    public class TransformEvent
    {
        public double DeltaLeft { get; }
        public double DeltaTop { get; }
        public double Size { get; }
        public bool fromProperty { get; }

        public TransformEvent(double deltaletf = 0, double deltatop = 0, double size = 0, bool fromproperty = true)
        {
            DeltaLeft = deltaletf;
            DeltaTop = deltatop;
            Size = size;
            fromProperty = fromproperty;
        }
    }
}
