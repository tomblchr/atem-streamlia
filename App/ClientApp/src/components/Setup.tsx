import * as React from "react";
import { QRCodeSVG } from "qrcode.react";
//import { hostURL } from "../api/atemconnection";
import * as Log from "../api/log";
import ServerHubConnection from "./ServerHubConnection";
import { HubConnectionState } from "@microsoft/signalr";
import { GitHub } from "react-feather";

interface ISetupState {
    // IP address of ATEM
    atemIpAddress: string;
    hostAgentIpAddress: string;
    fullscreen: boolean;
}

interface ISetupProps {
    livestreamUrl: string;
    liveStreamEnabled: boolean;
    onLivestreamUrlChange: Function;
    onLivestreamEnabledChange: Function;
    hostAgentNetworkLocation: string;
    onHostAgentNetworkLocationChange: Function;
    server: ServerHubConnection | undefined;
}

const Setup = ({ server, livestreamUrl, liveStreamEnabled, hostAgentNetworkLocation, onLivestreamUrlChange, onLivestreamEnabledChange, onHostAgentNetworkLocationChange }: ISetupProps): React.ReactElement => {
    
    const [state, setState] = React.useState<ISetupState>({ 
        atemIpAddress: "", 
        hostAgentIpAddress: hostAgentNetworkLocation, 
        fullscreen: (document.fullscreenElement !== null) 
    });

    React.useEffect(() => {

        server?.connection.on("ReceiveLivestreamPreviewUrl", message => {
            Log.debug(`ReceiveLivestreamPreviewUrl - ${message}`);
            onLivestreamUrlChange(message);
        });

        return () => {
            // clean up
            server?.connection.off("ReceiveLivestreamPreviewUrl");
        };

    }, [server, onLivestreamUrlChange]);

    const ver = __APP_COMMIT_HASH__;

    /*

    React.useEffect(() => {

        const callapi = (h: string) => {

            if (window.location.host === "atem.streamlia.com") {
                Log.info("Centrally hosted");
                return;
            }
            hostURL(h)
                .then(response => {
                    if (response.status < 300) {
                        response.text().then(value => {
                            setState(s => {return { ...s, atemIpAddress: value.replace("https://", "") }});
                        });
                    } else {
                        Log.error(response.statusText);
                    }
                })
                .catch(reason => {
                    Log.error(reason);
                });
        };

        callapi(state.hostAgentIpAddress);

    }, [state.hostAgentIpAddress, state.atemIpAddress]);  

    */

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        switch (event.target.dataset.field) {
            case "ipaddress": setState({...state, atemIpAddress: event.target.value }); break;
            case "hostipaddress": setState({...state, hostAgentIpAddress: event.target.value }); break;
        }
    };

    const handleLivestreamUrlChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        onLivestreamUrlChange(event.target.value);
        server?.connection.send("SendLivestreamPreviewUrl", event.target.value);
    }

    const handleLivestreamEnabledChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        onLivestreamEnabledChange(event.target.checked);
    }

    const save = () => {

        Log.info(`Connecting to agent@${state.hostAgentIpAddress} and ATEM@${state.atemIpAddress}`);

        onHostAgentNetworkLocationChange(state.hostAgentIpAddress);

        server?.ignition();

        if (server?.connection && server.connection.state === HubConnectionState.Connected) {
            if (state.atemIpAddress !== "") {
                server?.connection.send("SendConnect", state.atemIpAddress);
            }
        } else {
            Log.error(`No server connection ${state.atemIpAddress} - ${state.hostAgentIpAddress}`);
        }
    };

    const goLive = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (server?.connection) {
            if (event.target.checked) {
                server?.connection.send("SendStartStreaming");
            }
            else {
                server.connection.send("SendStopStreaming");
            }
        }
    }

    const goFullscreen = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (!document.exitFullscreen) {
            Log.error("Full screen mode not supported");
            return;
        }

        if (event.target.checked) {
            document.firstElementChild?.requestFullscreen();
        } else {
            document.exitFullscreen();
        }

        setState({...state, fullscreen: event.target.checked});
    }

    return <section className="setup">
        <h3>Setup</h3>
        <div className="well">
            <h4>Agent Host or IP Address</h4>
            <div className="input-group mb-3">                
                <input type="url" data-field="hostipaddress" className="form-control" placeholder={state.hostAgentIpAddress} aria-label="hostipaddress" aria-describedby="basic-addon2" onBlur={handleChange} />
                <a target="_blank" rel="noreferrer" href={`https://${state.hostAgentIpAddress}`} className="btn btn-primary">Test...</a>
            </div>
            <h4>ATEM IP Address</h4>
            <div className="input-group mb-3">
                <input type="url" data-field="ipaddress" className="form-control" placeholder={state.atemIpAddress} aria-label="ipaddress" aria-describedby="basic-addon1" onBlur={handleChange} />
                <a target="_blank" rel="noreferrer" href={`https://${state.atemIpAddress}`} className="btn btn-primary">Test...</a>
            </div>            
            <button type="button" className="btn btn-primary" onClick={e => { save() }}>Save</button>
        </div>
        <h3>View</h3>
        <div className="well well-column">
            <h4>Livestream URL:</h4>
            <div className="input-group mb-3">
                <input type="text" className="form-control" placeholder={livestreamUrl} aria-label="livestreamurl" aria-describedby="basic-addon1" onChange={handleLivestreamUrlChange} />
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="customSwitch2" onChange={handleLivestreamEnabledChange} />
                <label className="form-check-label" htmlFor="customSwitch2"> Enable Livestream Preview</label>
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="switchFullscreen" checked={state.fullscreen} onChange={goFullscreen} />
                <label className="form-check-label" htmlFor="switchFullscreen"> Fullscreen</label>
            </div>
        </div>
        <h3>Streaming</h3>
        <div className="well well-column">
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="customSwitch1" onChange={goLive} />
                <label className="form-check-label" htmlFor="customSwitch1"> Enable Livestream</label>
            </div>
        </div>
        <h3>Instructions</h3>
        <div className="well" style={{ display: "block" }}>
            <p>Instal the <a href="CA.pem">server root certificate</a>.</p>
            <p>Open <a href="https://atem.streamlia.com">https://atem.streamlia.com</a>. 
               Set the network location of the <a href="https://github.com/tomblchr/atem-streamlia/releases">host agent</a>. 
               See <a href="https://github.com/tomblchr/atem-streamlia"><GitHub /></a> for details.
               Version: {ver} Environment: {import.meta.env.MODE}
            </p>
            <div>
                <QRCodeSVG value="https://atem.streamlia.com" />
            </div>
        </div>
    </section>
}

export default Setup;