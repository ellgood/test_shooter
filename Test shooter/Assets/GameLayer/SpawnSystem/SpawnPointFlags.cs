using System;

namespace GameLayer.SpawnSystem
{
    [Flags]
    public enum SpawnPointFlags : byte
    {
        None = 0,
        Character = 1,
        Enemy = 1 << 1,
        All = Character | Enemy
    }
}