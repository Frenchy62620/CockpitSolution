
namespace CockpitBuilder.Events
{
    public class NewAppearanceEvent
    {
        public object Appearance ;
        public NewAppearanceEvent(object appearance)
        {
            Appearance = appearance;
        }
    }
}
