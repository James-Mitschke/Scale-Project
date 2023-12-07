using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Services.RandomGeneratorService
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Random _random;

        public RandomGenerator()
        {
            _random = new Random(DateTime.Now.Millisecond);
        }

        public int GetRandomInt()
        {
            return _random.Next();
        }

        public int GetRangedRandomInt(int startRange, int endRange)
        {
            return _random.Next(startRange, endRange);
        }

        public float GetRangedRandomFloat(float startRange, float endRange)
        {
            int whole;
            int randDp1;
            int randDp2;
            int startDecimal1;
            int startDecimal2;
            int endDecimal1;
            int endDecimal2;

            string[] start = startRange.ToString().Split(".");
            string[] end = endRange.ToString().Split(".");

            if (!int.TryParse(start[0], out int startWhole))
            {
                startWhole = (int)startRange;
            }

            if (start.Length == 2)
            {
                if (!int.TryParse(start[1].Substring(0, 1), out startDecimal1))
                {
                    startDecimal1 = _random.Next(0, 9);
                }
            }
            else
            {
                startDecimal1 = 0;
            }

            if (start.Length == 2 && start[1].Length == 2)
            {
                if (!int.TryParse(start[1].Substring(1, 1), out startDecimal2))
                {
                    startDecimal2 = _random.Next(0, 9);
                }
            }
            else
            {
                startDecimal2 = 0;
            }

            if (!int.TryParse(end[0], out int endWhole))
            {
                endWhole = (int)endRange;
            }

            if (end.Length == 2)
            {
                if (!int.TryParse(end[1].Substring(1), out endDecimal1))
                {
                    endDecimal1 = _random.Next(0, 9);
                }
            }
            else
            {
                endDecimal1 = 9;
            }

            if (end.Length == 2 && end[1].Length == 2)
            {
                if (!int.TryParse(end[1].Substring(1, 1), out endDecimal2))
                {
                    endDecimal2 = _random.Next(0, 9);
                }
            }
            else
            {
                endDecimal2 = 9;
            }

            whole = _random.Next(startWhole, endWhole);

            if (whole == endWhole)
            {
                randDp1 = _random.Next(0, endDecimal1);

                if (randDp1 == endDecimal1)
                {
                    randDp2 = _random.Next(0, endDecimal2);
                }
                else 
                {
                    randDp2 = _random.Next(0, 9);
                }
            }
            else if (whole == startWhole)
            {
                randDp1 = _random.Next(startDecimal1, 9);

                if (randDp1 == startDecimal1)
                {
                    randDp2 = _random.Next(startDecimal2, 9);
                }
                else
                {
                    randDp2 = _random.Next(0, 9);
                }
            }
            else
            {
                randDp1 = _random.Next(0, 9);
                randDp2 = _random.Next(0, 9);
            }

            if (randDp1 != 0)
            {
                randDp1 = randDp1 / 10;
            }

            if (randDp2 != 0)
            {
                randDp2 = randDp2 / 100;
            }

            return whole + randDp1 + randDp2;
        }

        public int GetWeightedRandomInt(IEnumerable<float> weights)
        {
            int defaultReturn = GetRangedRandomInt(1, weights.Count());

            if (weights.Sum() != 1)
            {
                return defaultReturn;
            }

            int randomInt = GetRangedRandomInt(0, 100);

            for (int index = 0; index < weights.Count(); ++index)
            {
                float runningTotal = weights.Take(index + 1).Sum() * 100;

                if (randomInt <= runningTotal)
                {
                    return index;
                }
            }

            return defaultReturn;
        }
    }
}
