using System.Collections.Generic;
using System.Linq;

namespace GitTask.Storage
{
    public class KeyGeneratorService
    {
        private int _lastKeyValue;

        public KeyGeneratorService(IEnumerable<int> allKeyValues)
        {
            if (allKeyValues == null || !allKeyValues.Any())
            {
                _lastKeyValue = 0;
            }
            else
            {
                _lastKeyValue = allKeyValues.Max();
            }
        }

        public int GenerateKey()
        {
            return ++_lastKeyValue;
        }
    }
}