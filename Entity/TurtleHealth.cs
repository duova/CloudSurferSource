using System;
using Management;
using Terrain;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Entity
{
    public class TurtleHealth : Health
    {
        [SerializeField]
        private float shieldTime, maxFallTime;
        private float _shieldTimer, _gameEndTimer, _fallTimer;

        [SerializeField] private GameObject shieldVisual, turtleVisual, fadeScreen;
        private Image _fadeImage;

        public static TurtleHealth Instance { get; private set; }

        private void Awake()
        {
            _fadeImage = fadeScreen.GetComponent<Image>();
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of TurtleHealth should not exist");
            }
        }

        protected override void Start()
        {
            maxHealth = StatManager.Instance.Health;
            CurrentHealth = maxHealth;
            shieldVisual.SetActive(false);
        }

        protected override void OnDeath()
        {
            const string score = "Score";
            if (PlayerPrefs.HasKey(score))
            {
                if (TerrainSpawner.Instance.SectionsTraveled > PlayerPrefs.GetInt(score))
                {
                    PlayerPrefs.SetInt(score, TerrainSpawner.Instance.SectionsTraveled);
                }
            }
            else
            {
                PlayerPrefs.SetInt(score, TerrainSpawner.Instance.SectionsTraveled);
            }
            const string lastScore = "LastScore";
            PlayerPrefs.SetInt(lastScore, TerrainSpawner.Instance.SectionsTraveled);

                _gameEndTimer += Time.deltaTime;
            turtleVisual.transform.localRotation = Quaternion.Euler(
                turtleVisual.transform.localRotation.eulerAngles.x - 180f,
                turtleVisual.transform.localRotation.eulerAngles.y, turtleVisual.transform.localRotation.eulerAngles.z);
            CurrentHealth = maxHealth;
        }

        protected override void Update()
        {
            base.Update();
            _shieldTimer -= Time.deltaTime;
            _fallTimer += Time.deltaTime;

            Physics.Raycast(new Ray(transform.position, -transform.up), out var hit, 1f);
            if (hit.collider && hit.collider.gameObject.TryGetComponent<Floor>(out _))
            {
                _fallTimer = 0;
            }

            if (_fallTimer > maxFallTime)
            {
                OnDeath();
            }

            if (_gameEndTimer > 1f)
            {
                SceneManager.LoadScene(2);
            }
            else if (_gameEndTimer != 0f)
            {
                _gameEndTimer += Time.deltaTime;
                _fadeImage.color = new Color(_fadeImage.color.r, _fadeImage.color.g, _fadeImage.color.b,
                    _gameEndTimer);
            }

            if (_shieldTimer < 0 && shieldVisual.activeSelf)
            {
                shieldVisual.SetActive(false);
            }
            
            if ((!Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.RightShift)) ||
                StatManager.Instance.Shield <= 0) return;
            StatManager.Instance.Shield--;
            _shieldTimer = shieldTime;
            shieldVisual.SetActive(true);
        }

        public override void Damage(float value)
        {
            if (_shieldTimer < 0)
            {
                base.Damage(value);
            }
        }
    }
}