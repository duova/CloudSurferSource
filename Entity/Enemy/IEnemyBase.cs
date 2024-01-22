namespace Entity.Enemy
{
    public interface IEnemyBase
    { 
        float CurrencyScaling { get; set; }
        int CurrencyValue { get; }
        float HealthScaling { get; set; }
        /// <summary>
        /// Damage scaling should increase the attack rate as well.
        /// </summary>
        float DamageScaling { get; set; }

        bool IsAlive { get; set; }

        void Spawn();
    }
}