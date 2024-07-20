using System.Collections.Generic;
using System.Linq;
using StaticData;
using UnityEngine;

namespace Services
{
    public interface IStaticDataProvider : IService
    {
        void LoadData();

        LevelDataScriptableObject GetDataForLevel(string levelCode);
    }

    public class StaticDataProvider : IStaticDataProvider
    {
        private Dictionary<string, LevelDataScriptableObject> _levelData;

        public void LoadData()
        {
            _levelData = Resources
                .LoadAll<LevelDataScriptableObject>(Constants.LevelDataResourcesPath)
                .ToDictionary(x => x.LevelCode, y => y);
        }

        public LevelDataScriptableObject GetDataForLevel(string levelCode)
        {
            return _levelData[levelCode];
        }
    }
}