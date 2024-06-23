using UnityEngine;

namespace Assets.Scripts.Services.DataStorageService
{
    public interface IDataStorage
    {
        public float GetScreenSize();
        public float GetScreenRectWidth();
        public int GetPlayerMaxLength();
        public GameObject GetUniquePlayerPart(string partName);
    }
}
