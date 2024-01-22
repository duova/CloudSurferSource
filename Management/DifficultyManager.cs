using System;
using UnityEngine;

namespace Management
{
    public class DifficultyManager : MonoBehaviour
    {
        public static DifficultyManager Instance { get; private set; }
        public int DifficultyLevel { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of DifficultyManager should not exist");
            }
        }

        private void Update()
        {
            DifficultyLevel = CalculateDifficulty(TerrainSpawner.Instance.SectionsTraveled);
        }

        private int CalculateDifficulty(int sectionsTravelled)
        {
            //DifficultyLevel is equal to sections travelled except it can be adjusted by the following factors.
            //Recent damage taken, and whether a boss battle has just happened.
            return sectionsTravelled;
        }
    }
}
