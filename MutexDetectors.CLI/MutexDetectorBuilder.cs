using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutexDetectors.CLI
{
    public enum DetectorOptions
    {
        EffectBalance
    }
    public static class MutexDetectorBuilder
    {
        private static Dictionary<DetectorOptions, Func<IMutexDetectors>> _detectors = new Dictionary<DetectorOptions, Func<IMutexDetectors>>()
        {
            { DetectorOptions.EffectBalance, () => new EffectBalanceMutexes() }
        };

        public static IMutexDetectors GetDetector(DetectorOptions option) => _detectors[option]();
    }
}
