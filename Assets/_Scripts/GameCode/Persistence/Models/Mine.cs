using System;

namespace GameCode.Persistence.Models
{
    [Serializable]
    public class Mine : Base
    {
        public int price;
        public int quantity;
        public int elevatorLevel;
    }
}