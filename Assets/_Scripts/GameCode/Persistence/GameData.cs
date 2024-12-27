using System;
using System.Collections.Generic;
using GameCode.Persistence.Models;

namespace GameCode.Persistence
{
    [Serializable]
    public class GameData
    {
        public List<Shop> shops;
    }
}