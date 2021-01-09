using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public static class ExtensionMethods
    {
        public static IEnumerable<IBMDSwitcherInput> GetInputs(this IBMDSwitcher o)
        {
            var result = new List<IBMDSwitcherInput>();
            Guid g = typeof(IBMDSwitcherInputIterator).GUID;
            o.CreateIterator(ref g, out IntPtr ptr);
            var iterator = (IBMDSwitcherInputIterator)Marshal.GetObjectForIUnknown(ptr);
            iterator.Next(out IBMDSwitcherInput input);
            while (input != null)
            {
                result.Add(input);
                iterator.Next(out input);
            }
            return result;
        }

        public static IEnumerable<IBMDSwitcherMixEffectBlock> GetMixEffectBlocks(this IBMDSwitcher o)
        {
            var result = new List<IBMDSwitcherMixEffectBlock>();
            Guid g = typeof(IBMDSwitcherMixEffectBlockIterator).GUID;
            o.CreateIterator(ref g, out IntPtr ptr);
            var iterator = (IBMDSwitcherMixEffectBlockIterator)Marshal.GetObjectForIUnknown(ptr);
            iterator.Next(out IBMDSwitcherMixEffectBlock input);
            while (input != null)
            {
                result.Add(input);
                iterator.Next(out input);
            }
            return result;
        }

        /// <summary>
        /// Query the switcher for the Fairlight Audio Mixer
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IBMDSwitcherFairlightAudioMixer GetFairlightAudioMixer(this IBMDSwitcher o)
        {
            Guid g = typeof(IBMDSwitcherFairlightAudioMixer).GUID;
            Marshal.QueryInterface(Marshal.GetIUnknownForObject(o), ref g, out IntPtr ptr);
            return (IBMDSwitcherFairlightAudioMixer)Marshal.GetObjectForIUnknown(ptr);
        }

        public static IBMDSwitcherTransitionParameters GetTransitionParameters(this IBMDSwitcherMixEffectBlock m)
        {
            Guid g = typeof(IBMDSwitcherTransitionParameters).GUID;
            Marshal.QueryInterface(Marshal.GetIUnknownForObject(m), ref g, out IntPtr ptr);
            return (IBMDSwitcherTransitionParameters)Marshal.GetObjectForIUnknown(ptr);
        }
    }
}
