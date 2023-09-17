using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    { 
        private const string FOLDER = "WordSearch/Levels/";

        public LevelInfo LoadLevelData(int levelIndex)
        {
            var path = FOLDER + levelIndex.ToString();
            var jsonFile = Resources.Load<TextAsset>(path);

            if (!jsonFile)
                return null;

            var levelInfo = JsonUtility.FromJson<LevelInfo>(jsonFile.text);

            return levelInfo;
        }
    }
}