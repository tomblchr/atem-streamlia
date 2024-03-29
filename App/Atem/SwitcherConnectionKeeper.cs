﻿using BMDSwitcherAPI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        
        string _ipaddress;

        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public SwitcherConnectionKeeper(IMediator mediator, ILogger<SwitcherConnectionKeeper> logger)
        {
            _mediator = mediator;
            _logger = (ILogger)logger ?? NullLogger.Instance;
        }

        public IBMDSwitcher Connect()
        {
            lock (_lock)
            {
                if (_switcher == null || !_isConnected)
                {
                    return ConnectImpl();
                }
                return _switcher;
            }
        }

        public IBMDSwitcher Connect(string ipaddress)
        {
            lock (_lock)
            {
                if (_switcher == null || !_isConnected)
                {
                    _ipaddress = string.IsNullOrEmpty(ipaddress) ? _ipaddress : ipaddress;
                    return ConnectImpl();
                }
                return _switcher;                
            }
        }

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        IBMDSwitcher ConnectImpl()
        { 
            if (string.IsNullOrEmpty(_ipaddress))
            {
                Console.WriteLine("Warning: IP address has not been initialized. I do not like your chances.");
            }

            if (_switcher == null || !_isConnected)
            {
                _BMDSwitcherConnectToFailure failure = 0;
                IBMDSwitcherDiscovery discovery = null;
                try
                {
                    discovery = new CBMDSwitcherDiscovery();
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    _logger.LogError($"ATEM software not installed. Please install ATEM Switchers software from https://www.blackmagicdesign.com/ca/support/family/atem-live-production-switchers");
                    throw;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Could not create local objects");
                    throw;
                }
                try
                {
                    discovery.ConnectTo(_ipaddress, out _switcher, out failure);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Could not connect to ATEM");
                }
                if (failure == 0)
                {
                    _isConnected = true;
                    _switcher.AddCallback(this);
                    _switcher.GetProductName(out string productName);
                    _logger.LogInformation($"Happy Days... connected to {productName}@{_ipaddress}!");
                }
                else
                {
                    _logger.LogWarning(failure.ToString());
                }
            }

            return _switcher;
        }

        public void Disconnect()
        {
            lock (_lock)
            {
                _isConnected = false;
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
                    _mediator.Publish(new ConnectionChangeNotify { Connected = false });
                    break;
                case _BMDSwitcherEventType.bmdSwitcherEventTypeTimeCodeChanged:
                    break;
                default:
                    _mediator.Publish(new SwitcherMessageNotify { Message = $"SwitcherConnectionKeeper says: {eventType}" });
                    break;
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_switcher != null)
                {
                    _switcher.RemoveCallback(this);
                    _switcher = null;
                }
            }
        }
    }
}
