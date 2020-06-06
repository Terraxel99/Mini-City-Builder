using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Constants
{
    public static class WorldDimensions
    {
        public static class SmallWorld
        {
            public static readonly int MIN_WIDTH = 25;
            public static readonly int MAX_WIDTH = 50;
            public static readonly int MIN_HEIGHT = 20;
            public static readonly int MAX_HEIGHT = 45;
        }

        public static class MediumWorld
        {
            public static readonly int MIN_WIDTH = 50;
            public static readonly int MAX_WIDTH = 125;
            public static readonly int MIN_HEIGHT = 45;
            public static readonly int MAX_HEIGHT = 110;
        }

        public static class BigWorld
        {
            public static readonly int MIN_WIDTH = 125;
            public static readonly int MAX_WIDTH = 300;
            public static readonly int MIN_HEIGHT = 110;
            public static readonly int MAX_HEIGHT = 250;
        }
    }

    public static class Tags
    {
        public static readonly string GROUND_TAG = "Ground";
    }

    public static class GameobjectsName
    {
        public static readonly string GROUND_NAME = "Ground";
    }
}
