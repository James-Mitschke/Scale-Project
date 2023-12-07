namespace Assets.Scripts.Services.DataStorageService
{
    public class DataStorage : IDataStorage
    {
        public float ScreenSize { get; set; }

        public DataStorage(float screenSize = -1)
        {
            ScreenSize = screenSize;
        }

        public float GetScreenSize()
        {
            return ScreenSize;
        }
    }
}
