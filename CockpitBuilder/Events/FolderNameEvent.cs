namespace CockpitBuilder.Events
{
    public class FolderNameEvent
    {
        public string FolderName;

        public FolderNameEvent(string foldername)
        {
            FolderName = foldername;
        }
    }
}
