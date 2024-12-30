using System;
using System.Collections.Generic;

namespace GameCode.Persistence.Models
{
    [Serializable]
    public class Mine : Base
    {
        public int elevatorLevel;
        public int warehouseLevel;
        public List<MineshaftData> mineshafts;
        public double elevatorStashAmount;
    }
}