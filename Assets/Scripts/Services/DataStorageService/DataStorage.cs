namespace Assets.Scripts.Services.DataStorageService
{
    public class DataStorage : IDataStorage
    {
        public float ScreenSize { get; set; }
        public float ScreenRectWidth { get; set; }

        public DataStorage(float screenSize = -1, float screenRectWidth = 1)
        {
            ScreenSize = screenSize;
            ScreenRectWidth = screenRectWidth;
        }

        public float GetScreenSize()
        {
            return ScreenSize;
        }

        public float GetScreenRectWidth()
        {
            return ScreenRectWidth;
        }
    }
}
