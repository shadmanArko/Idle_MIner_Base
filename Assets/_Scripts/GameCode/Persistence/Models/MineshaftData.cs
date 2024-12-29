using System;
using UnityEngine;

namespace GameCode.Persistence.Models
{
    [Serializable]
    public class MineshaftData
    {
        public int mineshaftNumber;
        public int level;
        public Vector2 position;
    }
}