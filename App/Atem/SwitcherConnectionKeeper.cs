using BMDSwitcherAPI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitcherServer.Atem
{
    public class SwitcherConnectionKeeper : ISwitcherConnection, IBMDSwitcherCallback, IDisposable
    {
        static IBMDSwitcher _switcher;
        static bool _isConnected = false;
        static readonly object _lock = new object();
        readonly string _ipaddress;
        private readonly IMediator _mediator;

        public SwitcherConnectionKeeper(string ipaddress, IMediator mediator)
        {
            _ipaddress = ipaddress;
            _mediator = mediator;
        }

        public IBMDSwitcher Connect()
        {
            if (string.IsNullOrEmpty(_ipaddress))
            {
                Console.WriteLine("Warning: IP address has not been initialized. I do not like your chances.");
            }

            if (_switcher == null || !_isConnected)
            {
                var discovery = new CBMDSwitcherDiscovery();
                discovery.ConnectTo(_ipaddress, out _switcher, out var failure);
                if ((int)failure == 0)
                {
                    _isConnected = true;
                    Console.WriteLine("Happy Days... Connected!");
                }
                else
                {
                    Console.WriteLine(failure.ToString());
                }
                _switcher.AddCallback(this);
            }
            _mediator.Publish(new ConnectionChangeNotify { Connected = _isConnected });
            return _switcher;
        }

        void Disconnect()
        {
            lock (_lock)
            {
                _isConnected = false;
                _mediator.Publish(new ConnectionChangeNotify { Connected = _isConnected });
            }
        }

        /// <summary>
        /// In case the switcher is disconnected we manage the reconnection here
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="coreVideoMode"></param>
        public void Notify(_BMDSwitcherEventType eventType, _BMDSwitcherVideoMode coreVideoMode)
        {
            switch (eventType)
            {
                case _BMDSwitcherEventType.bmdSwitcherEventTypeDisconnected:
                    Disconnect();
                    break;
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_mediator != null)
                {
                    _mediator.Publish(new ConnectionChangeNotify { Connected = false });
                }
                if (_switcher != null)
                {
                    _switcher.RemoveCallback(this);
                    _switcher = null;
                }
            }
        }
    }
}
