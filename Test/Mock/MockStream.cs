using BMDSwitcherAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitcherServerTests.Mock
{
    /// <summary>
    /// The ATEM Mini does not support streaming, but I want to test this capability works
    /// </summary>
    class MockStream : IBMDSwitcherStreamRTMP
    {
        static _BMDSwitcherStreamRTMPState _state = _BMDSwitcherStreamRTMPState.bmdSwitcherStreamRTMPStateIdle;
        static object _lock = new object();

        public void StartStreaming()
        {
            lock (_lock)
            {
                _state = _BMDSwitcherStreamRTMPState.bmdSwitcherStreamRTMPStateStreaming;
                _notifyStatus(_BMDSwitcherStreamRTMPState.bmdSwitcherStreamRTMPStateStreaming, _BMDSwitcherStreamRTMPError.bmdSwitcherStreamRTMPErrorNone);
            }
        }

        public void StopStreaming()
        {
            lock (_lock)
            {
                _state = _BMDSwitcherStreamRTMPState.bmdSwitcherStreamRTMPStateIdle;
                _notifyStatus(_BMDSwitcherStreamRTMPState.bmdSwitcherStreamRTMPStateIdle, _BMDSwitcherStreamRTMPError.bmdSwitcherStreamRTMPErrorNone);
            }
        }

        public void IsStreaming(out int streaming)
        {
            if (_state == _BMDSwitcherStreamRTMPState.bmdSwitcherStreamRTMPStateStreaming)
            {
                streaming = 1;
            }
            else
            {
                streaming = 0;
            }
        }

        public void GetStatus(out _BMDSwitcherStreamRTMPState state, out _BMDSwitcherStreamRTMPError error)
        {
            throw new NotImplementedException();
        }

        public void SetServiceName(string serviceName)
        {
            throw new NotImplementedException();
        }

        public void GetServiceName(out string serviceName)
        {
            throw new NotImplementedException();
        }

        public void SetUrl(string url)
        {
            throw new NotImplementedException();
        }

        public void GetUrl(out string url)
        {
            throw new NotImplementedException();
        }

        public void SetKey(string url)
        {
            throw new NotImplementedException();
        }

        public void GetKey(out string key)
        {
            throw new NotImplementedException();
        }

        public void SetVideoBitrates(uint lowBitrate, uint highBitrate)
        {
            throw new NotImplementedException();
        }

        public void GetVideoBitrates(out uint lowBitrate, out uint highBitrate)
        {
            throw new NotImplementedException();
        }

        public void SetAudioBitrates(uint lowBitrate, uint highBitrate)
        {
            throw new NotImplementedException();
        }

        public void GetAudioBitrates(out uint lowBitrate, out uint highBitrate)
        {
            throw new NotImplementedException();
        }

        public void RequestDuration()
        {
            throw new NotImplementedException();
        }

        public void GetDuration(out ulong duration)
        {
            throw new NotImplementedException();
        }

        public void GetTimeCode(out byte hours, out byte minutes, out byte seconds, out byte frames, out int dropFrame)
        {
            throw new NotImplementedException();
        }

        public void GetEncodingBitrate(out uint encodingBitrate)
        {
            throw new NotImplementedException();
        }

        public void GetCacheUsed(out double cacheUsed)
        {
            throw new NotImplementedException();
        }

        public void SetAuthentication(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void GetAuthentication(out string username, out string password)
        {
            throw new NotImplementedException();
        }

        public void SetLowLatency(int lowLatency)
        {
            throw new NotImplementedException();
        }

        public void GetLowLatency(out int lowLatency)
        {
            throw new NotImplementedException();
        }

        Action<_BMDSwitcherStreamRTMPEventType> _notify;
        Action<_BMDSwitcherStreamRTMPState, _BMDSwitcherStreamRTMPError> _notifyStatus;
        public void AddCallback(IBMDSwitcherStreamRTMPCallback callback)
        {
            _notify = callback.Notify;
            _notifyStatus = callback.NotifyStatus;
        }

        public void RemoveCallback(IBMDSwitcherStreamRTMPCallback callback)
        {
            throw new NotImplementedException();
        }

        public void GetDuration(out byte hours, out byte minutes, out byte seconds, out byte frames, out int dropFrame)
        {
            throw new NotImplementedException();
        }
    }
}
