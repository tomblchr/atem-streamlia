using BMDSwitcherAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using SwitcherServer.Atem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SwitcherServer.Controllers
{
    public interface IBMDIterator
    {
        void CreateIterator(ref Guid iid, out IntPtr ppv);
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AtemConnectionController : ControllerBase
    {
        private readonly IBMDSwitcher _switcher;
        private readonly IConfiguration _configuration;

        public AtemConnectionController(IConfiguration configuration, Switcher switcher)
        {
            _switcher = switcher?.SwitcherDirect;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("streamliaurl")]
        public IActionResult GetStreamliaURL()
        {
            // return ATEM IP address?
            return Ok(NetworkInspector.GetUrl(_configuration));
        }

        /*
         * 
         * Moved to signalr
         * 
         * 
        [HttpGet]
        [Route("name")]
        public IActionResult DoSomething()
        {
            _switcher.GetProductName(out var name);

            return Ok(name);
        }



        [HttpGet]
        [Route("videomode")]
        public IActionResult GetVideoMode()
        {
            _switcher.GetVideoMode(out _BMDSwitcherVideoMode name);

            return Ok(name.ToString());
        }

        [HttpGet]
        public IActionResult DoSomethingElse()
        {
            List<string> result = new List<string>();

            Guid inputIteratorIID = typeof(IBMDSwitcherInputIterator).GUID;

            _switcher.CreateIterator(inputIteratorIID, out IntPtr ptr);

            var inputIterator = (IBMDSwitcherInputIterator)Marshal.GetObjectForIUnknown(ptr);

            inputIterator.Next(out IBMDSwitcherInput input);
            while (input != null)
            {
                input.GetLongName(out string name);
                result.Add(name);
                inputIterator.Next(out input);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("inputs")]
        public IActionResult DoSomethingElseWithGenerics()
        {
            List<string> result = new List<string>();

            var inputs = _switcher.GetInputs();

            inputs.ToList().ForEach(c => 
            {
                c.GetLongName(out string name);
                result.Add(name);
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("mix")]
        public IActionResult DoSomethingElseAgain()
        {
            List<long> result = new List<long>();

            Guid mixIteratorIID = typeof(IBMDSwitcherMixEffectBlockIterator).GUID;

            _switcher.CreateIterator(mixIteratorIID, out IntPtr ptr);

            var mixIterator = (IBMDSwitcherMixEffectBlockIterator)Marshal.GetObjectForIUnknown(ptr);

            mixIterator.Next(out IBMDSwitcherMixEffectBlock mix);
            while (mix != null)
            {
                mix.GetProgramInput(out long name);
                result.Add(name);
                mixIterator.Next(out mix);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("transition")]
        public IActionResult Transition()
        {
            _switcher.GetMixEffectBlocks().First().PerformAutoTransition();

            return Ok();
        }

        [HttpGet]
        [Route("audio")]
        public IActionResult AudioInputs()
        {
            List<string> result = new List<string>();

            var audio = (IBMDSwitcherFairlightAudioMixer)_switcher;

            Guid auditInputIID = typeof(IBMDSwitcherFairlightAudioInputIterator).GUID;
            Guid audioSource = typeof(IBMDSwitcherFairlightAudioSourceIterator).GUID;

            audio.CreateIterator(auditInputIID, out IntPtr ptr);

            var audioIterator = (IBMDSwitcherFairlightAudioInputIterator)Marshal.GetObjectForIUnknown(ptr);

            audioIterator.Next(out IBMDSwitcherFairlightAudioInput input);
            while (input != null)
            {
                input.GetId(out long id);
                input.GetCurrentExternalPortType(out _BMDSwitcherExternalPortType config);
                result.Add($"{id}: {config}");

                input.CreateIterator(audioSource, out ptr);
                var sourceInterator = (IBMDSwitcherFairlightAudioSourceIterator)Marshal.GetObjectForIUnknown(ptr);
                sourceInterator.Next(out IBMDSwitcherFairlightAudioSource source);
                while (source != null)
                {
                    source.GetSourceType(out _BMDSwitcherFairlightAudioSourceType sourceType);
                    source.GetFaderGain(out double gain);
                    result.Add($"** {sourceType}: {gain}");
                    source.SetFaderGain(0);
                    sourceInterator.Next(out source);
                }
                audioIterator.Next(out input);
            }

            return Ok(result);
        }
        */
    }
}
