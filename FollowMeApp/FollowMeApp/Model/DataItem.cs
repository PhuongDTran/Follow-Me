namespace FollowMeApp.Model
{
    public class DataItem
    {
        public DataItem(string title, string startButtonText)
        {
            Title = title;
            StartButtonText = startButtonText;
        }
       
        public string StartButtonText
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }
    }
}
