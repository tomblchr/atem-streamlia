import * as React from "react";
import QRCode from "qrcode.react";
import { hostURL } from "../api/atemconnection";
import ServerHubConnection from "./ServerHubConnection";
import { HubConnectionState } from "@microsoft/signalr";

interface ISetupState {
    // IP address of ATEM
    ipaddress: string;
    host: string;
}

interface ISetupProps {
    livestreamUrl: string;
    liveStreamEnabled: boolean;
    onLivestreamUrlChange: Function;
    onLivestreamEnabledChange: Function;
    server: ServerHubConnection | undefined;
}

const Setup = ({ server, livestreamUrl, liveStreamEnabled, onLivestreamUrlChange, onLivestreamEnabledChange }: ISetupProps): React.ReactElement => {
    
    const [state, setState] = React.useState<ISetupState>({ ipaddress: "10.0.0.201", host: "" });

    React.useEffect(() => {

        server?.connection.on("ReceiveLivestreamPreviewUrl", message => {
            console.log(`ReceiveLivestreamPreviewUrl - ${message}`);
            onLivestreamUrlChange(message);
        });

        return () => {
            // clean up
            server?.connection.off("ReceiveLivestreamPreviewUrl");
        };

    }, [server]);

    React.useEffect(() => {
        callapi();
    }, []);  

    const callapi = (h?: string) => {
        hostURL(h)
            .then(response => {
                response.text().then(value => {
                    //alert(value);
                    //setState({ ipaddress: state.ipaddress, host: value });
                });
            })
            .catch(reason => {
                console.error(reason);
            });
    };

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.dataset.field === "ipaddress") {
            setState({ ipaddress: event.target.value, host: state.host });
        } else if (event.target.dataset.field === "hostipaddress") {
            setState({ ipaddress: state.ipaddress, host: event.target.value });
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

        callapi(state.host);
        
        if (server?.connection && server.connection.state === HubConnectionState.Connected) {
            server?.connection.send("SendConnect", state.ipaddress);
        } else {
            alert(`No server connection ${state.ipaddress} - ${state.host}`);
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

    return <section className="setup">
        <h3>Setup</h3>
        <div className="well">
            <div className="input-group mb-3">
                <div className="input-group-prepend">
                    <span className="input-group-text" id="basic-addon1">ATEM IP Address:</span>
                </div>
                <input type="text" data-field="ipaddress" className="form-control" placeholder={state.ipaddress} aria-label="ipaddress" aria-describedby="basic-addon1" onChange={handleChange} />
            </div>
            <div className="input-group mb-3">
                <div className="input-group-prepend">
                    <span className="input-group-text" id="basic-addon2">Host Agent IP Address:</span>
                </div>
                <input type="text" data-field="hostipaddress" className="form-control" placeholder={state.host} aria-label="hostipaddress" aria-describedby="basic-addon2" onChange={handleChange} />
            </div>
            <button type="button" className="btn btn-primary" onClick={e => { save() }}>Save</button>
        </div>
        <h3>Streaming</h3>
        <div className="well well-column">
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="customSwitch1" onChange={goLive} />
                <label className="form-check-label" htmlFor="customSwitch1"> Enable Livestream</label>
            </div>
        </div>
        <h3>Preview</h3>
        <div className="well well-column">
            <div className="input-group mb-3">
                <div className="input-group-prepend">
                    <span className="input-group-text" id="basic-addon1">Livestream URL:</span>
                </div>
                <input type="text" className="form-control" placeholder={livestreamUrl} aria-label="livestreamurl" aria-describedby="basic-addon1" onChange={handleLivestreamUrlChange} />
            </div>
            <div className="form-check form-switch">
                <input className="form-check-input" type="checkbox" role="switch" id="customSwitch2" checked={liveStreamEnabled} onChange={handleLivestreamEnabledChange} />
                <label className="form-check-label" htmlFor="customSwitch2"> Enable Livestream Preview</label>
            </div>
        </div>
        <h3>Instructions</h3>
        <div className="well" style={{ display: "block" }}>
            <p>Open <a href="https://atem.streamlia.com">https://atem.streamlia.com</a> in a browser to access this system (obviously you have done that already, which is why you are here!). </p>
            <div>
                <QRCode value="https://atem.streamlia.com" />
            </div>
        </div>
    </section>
}

export default Setup;