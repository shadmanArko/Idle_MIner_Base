using System;
using System.Collections.Generic;

namespace GameCode.Persistence.Models
{
    [Serializable]
    public class Mine : Base
    {
        public double money;
        public int elevatorLevel;
        public int warehouseLevel;
        public double elevatorStashAmount;
        public List<MineshaftData> mineshafts;
    }
}