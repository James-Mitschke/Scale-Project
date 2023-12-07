using System.Collections.Generic;

namespace Assets.Scripts.Services.RandomGeneratorService
{
    public interface IRandomGenerator
    {
        public int GetRandomInt();

        public int GetRangedRandomInt(int startRange, int endRange);

        public float GetRangedRandomFloat(float startRange, float endRange);

        public int GetWeightedRandomInt(IEnumerable<float> weights);
    }
}
