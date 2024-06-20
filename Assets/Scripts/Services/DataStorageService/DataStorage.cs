using UnityEngine;

namespace Assets.Scripts.Services.DataStorageService
{
    public class DataStorage : IDataStorage
    {
        private float ScreenSize { get; set; }
        private float ScreenRectWidth { get; set; }
        private int PlayerMaxLength { get; set; }

        public DataStorage(float screenSize = -1, float screenRectWidth = 1, int playerMaxLength = 100)
        {
            ScreenSize = screenSize;
            ScreenRectWidth = screenRectWidth;
            PlayerMaxLength = playerMaxLength;
        }

        public float GetScreenSize()
        {
            return ScreenSize;
        }

        public float GetScreenRectWidth()
        {
            return ScreenRectWidth;
        }

        public int GetPlayerMaxLength()
        {
            return PlayerMaxLength;
        }

        public GameObject GetUniquePlayerPart(string partName)
        {
            return (GameObject)Resources.Load($"objects/{partName}");
        }
    }
}
