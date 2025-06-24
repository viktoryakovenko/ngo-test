using UnityEngine;

namespace Code.StaticData
{
    // Конфігурація має проводитися у фабриці, але я грався з VContainer-ом і так і не зміг
    // підключити контейнер по-людськи. Бо VContainer не прокидує залежності в NetworkObject,
    // а якщо спавнити вручну, то прокидаються залежності лише на серверній частині гри чомусь.
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Scriptable Objects/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        public int MaxHealth;
        public float MovementSpeed;
    }
}
