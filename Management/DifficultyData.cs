using System;
using UnityEngine;

namespace Management
{
    [Serializable]
    public struct IntRange
    {
        public int min;
        public int max;

        public IntRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [CreateAssetMenu]
    public class DifficultyData : ScriptableObject
    {
        public IntRange easyLevels;
        public IntRange mediumLevels;
        public IntRange hardLevels;
        public IntRange insaneLevels;
        public IntRange bossLevels;
    }
}