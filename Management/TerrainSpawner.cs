using System;
using System.Collections.Generic;
using System.Linq;
using Terrain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Management
{
    public enum TerrainDifficulty
    {
        Start,
        Easy,
        Medium,
        Hard,
        Insane,
        Boss
    }

    [Serializable]
    public struct SectionData
    {
        public GameObject terrainObject;
        public TerrainDifficulty difficulty;
    }

    public class TerrainSpawner : MonoBehaviour
    {
        public static TerrainSpawner Instance { get; private set; }

        [SerializeField] private SectionData[]
            sections; //Section prefabs contain child objects with SectionEndMarker script and SectionSpawnMarker script

        [SerializeField] private DifficultyData difficultyData;

        public int SectionsTraveled { get; set; }

        public GameObject CurrentSectionObject { get; private set; }
        public int CurrentSectionIndex { get; private set; }

        public int BossHeatRaw { get; set; }

        public float BossHeat { get; private set; }

        public TerrainDifficulty CurrentTerrainDifficulty { get; private set; }

        /// <summary>
        /// Multiplier * Sections travelled after initial ones = Percentage chance for next section to be a boss.
        /// </summary>
        [SerializeField] private float bossHeatMultiplier;

        [HideInInspector] public bool OnBossLevel;

        private Queue<GameObject> _spawnedSections = new();

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of TerrainSpawner should not exist");
            }

            //Spawn initial sections and add them to despawn queue.
            foreach (var section in CreateTerrainRecursively(3))
            {
                _spawnedSections.Enqueue(section);
            }
        }

        private void Update()
        {
            if (SectionsTraveled + 1 > CurrentSectionIndex)
            {
                var terrainObjects = CreateTerrainRecursively();
                foreach (var obj in terrainObjects)
                {
                    _spawnedSections.Enqueue(terrainObjects.First());
                    Destroy(_spawnedSections.Dequeue());
                }
            }
            
            BossHeat = bossHeatMultiplier * BossHeatRaw;
        }

        private GameObject[] CreateTerrainRecursively(int amount = 1)
        {
            if (OnBossLevel) return Array.Empty<GameObject>(); //don't spawn terrain until onBossLevel is set back to false.
            
            GameObject objectToSpawn;

            List<GameObject> spawnedObjectList = new ();

            if (CurrentSectionObject == null)
            {
                objectToSpawn = sections.First(section => section.difficulty == TerrainDifficulty.Start).terrainObject;
                CurrentSectionObject = Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity);
                spawnedObjectList.Add(CurrentSectionObject);
                amount -= 1;
            }

            for (var i = 0; i < amount; i++)
            {
                objectToSpawn = DetermineSpawnedTerrain(out var difficulty);
                CurrentTerrainDifficulty = difficulty;
                if (difficulty == TerrainDifficulty.Boss)
                {
                    OnBossLevel = true;
                    BossHeatRaw = -1;
                }
                CurrentSectionObject = Instantiate(objectToSpawn,
                    CurrentSectionObject.GetComponentInChildren<SectionEndMarker>().transform.position,
                    CurrentSectionObject.GetComponentInChildren<SectionEndMarker>().transform.rotation);
                spawnedObjectList.Add(CurrentSectionObject);
                CurrentSectionIndex++;
            }

            return spawnedObjectList.ToArray();
        }

        public GameObject DetermineSpawnedTerrain(out TerrainDifficulty difficulty)
        {
            var level = DifficultyManager.Instance.DifficultyLevel;
            if (BossCheck(BossHeat))
            {
                difficulty = TerrainDifficulty.Boss;
                return RandomlySelectTerrain(TerrainDifficulty.Boss);
            }

            if (CheckLevelRangeInclusive(level, difficultyData.easyLevels))
            {
                difficulty = TerrainDifficulty.Easy;
                return RandomlySelectTerrain(TerrainDifficulty.Easy);
            }

            if (CheckLevelRangeInclusive(level, difficultyData.mediumLevels))
            {
                difficulty = TerrainDifficulty.Medium;
                return RandomlySelectTerrain(TerrainDifficulty.Medium);
            }

            if (CheckLevelRangeInclusive(level, difficultyData.hardLevels))
            {
                difficulty = TerrainDifficulty.Hard;
                return RandomlySelectTerrain(TerrainDifficulty.Hard);
            }
            
            difficulty = TerrainDifficulty.Insane;
            return RandomlySelectTerrain(TerrainDifficulty.Insane);
        }

        private bool BossCheck(float bossHeat)
        {
            return Random.Range(0f, 100f) < bossHeat;
        }

        private bool CheckLevelRangeInclusive(int value, IntRange range)
        {
            return value >= range.min && value <= range.max;
        }

        private GameObject RandomlySelectTerrain(TerrainDifficulty difficulty)
        {
            var sectionDatas = sections.Where(section => section.difficulty == difficulty).ToList();
            return sectionDatas[Random.Range(0, sectionDatas.Count)].terrainObject;
        }
    }
}