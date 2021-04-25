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

            if (o == null)
            {
                return result;
            }

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

            if (o == null)
            {
                return result;
            }

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

        public static IEnumerable<IBMDSwitcherKey> GetKeys(this IBMDSwitcherMixEffectBlock o)
        {
            var result = new List<IBMDSwitcherKey>();

            if (o == null)
            {
                return result;
            }

            Guid g = typeof(IBMDSwitcherKeyIterator).GUID;
            o.CreateIterator(ref g, out IntPtr ptr);
            var iterator = (IBMDSwitcherKeyIterator)Marshal.GetObjectForIUnknown(ptr);
            iterator.Next(out IBMDSwitcherKey input);
            while (input != null)
            {
                result.Add(input);
                iterator.Next(out input);
            }
            return result;
        }

        public static IBMDSwitcherKeyFlyParameters GetKeyFlyParameters(this IBMDSwitcherKey o)
        {
            return QueryInterface<IBMDSwitcherKey, IBMDSwitcherKeyFlyParameters>.GetObject(o);
        }

        public static IEnumerable<IBMDSwitcherDownstreamKey> GetDownstreamKeys(this IBMDSwitcher o)
        {
            var result = new List<IBMDSwitcherDownstreamKey>();

            if (o == null)
            {
                return result;
            }

            Guid g = typeof(IBMDSwitcherDownstreamKeyIterator).GUID;
            o.CreateIterator(ref g, out IntPtr ptr);
            var iterator = (IBMDSwitcherDownstreamKeyIterator)Marshal.GetObjectForIUnknown(ptr);
            iterator.Next(out IBMDSwitcherDownstreamKey input);
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
            return QueryInterface<IBMDSwitcher, IBMDSwitcherFairlightAudioMixer>.GetObject(o);
        }

        public static IBMDSwitcherTransitionParameters GetTransitionParameters(this IBMDSwitcherMixEffectBlock m)
        {
            return QueryInterface<IBMDSwitcherMixEffectBlock, IBMDSwitcherTransitionParameters>.GetObject(m);
        }

        public static IBMDSwitcherTransitionDVEParameters GetTransitionDVEParameters(this IBMDSwitcherMixEffectBlock m)
        {
            return QueryInterface<IBMDSwitcherMixEffectBlock, IBMDSwitcherTransitionDVEParameters>.GetObject(m);
        }

        public static IBMDSwitcherMacroPool GetMacroPool(this IBMDSwitcher o)
        {
            return QueryInterface<IBMDSwitcher, IBMDSwitcherMacroPool>.GetObject(o);
        }

        public static IBMDSwitcherMacroControl GetMacroControl(this IBMDSwitcher o)
        {
            return QueryInterface<IBMDSwitcher, IBMDSwitcherMacroControl>.GetObject(o);
        }

        public static IBMDSwitcherStreamRTMP GetStreamRTMP(this IBMDSwitcher o)
        {
            return QueryInterface<IBMDSwitcher, IBMDSwitcherStreamRTMP>.GetObject(o);
        }

        /// <summary>
        /// Remove dB values of -infinity
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        /// <remarks>
        /// JSON does not support infinity and -infinity as values and hence need to be removed
        /// <seealso cref="https://stackoverflow.com/questions/1423081/json-left-out-infinity-and-nan-json-status-in-ecmascript"/>
        /// </remarks>
        public static MasterOutLevelNotify WithoutInfinity(this MasterOutLevelNotify notification)
        {
            for (int i = 0; i < notification.Levels.Length; i++)
            {
                if (notification.Levels[i] == double.NegativeInfinity)
                {
                    notification.Levels[i] = -120;
                }
            }
            for (int i = 0; i < notification.Peaks.Length; i++)
            {
                if (notification.Peaks[i] == double.NegativeInfinity)
                {
                    notification.Peaks[i] = -120;
                }
            }

            return notification;
        }
    }

    static class QueryInterface<Input, Output>
    {
        public static Output GetObject(Input o)
        {
            Guid g = typeof(Output).GUID;
            Marshal.QueryInterface(Marshal.GetIUnknownForObject(o), ref g, out IntPtr ptr);

            if (ptr == IntPtr.Zero)
            {
                return default(Output);
            }

            return (Output)Marshal.GetObjectForIUnknown(ptr);
        }
    }
}
