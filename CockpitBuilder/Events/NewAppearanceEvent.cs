
namespace CockpitBuilder.Events
{
    public class NewAppearanceEvent
    {
        public object Appearance ;
        string NameUC;
        public NewAppearanceEvent(string nameuc, object appearance)
        {
            Appearance = appearance;
            NameUC = nameuc;
        }
    }
}
