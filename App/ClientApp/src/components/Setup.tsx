import * as React from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import QRCode, { displayName } from "qrcode.react";
import { hostURL } from "../api/atemconnection";
import ConnectionMonitor from "./ConnectionMonitor";
import IConnectToServer from "./IConnectToServer";

interface ISetupState {
    ipaddress: string;
    host: string;
}

const Setup = (props: IConnectToServer): React.ReactElement<IConnectToServer> => {
    
    const [state, setState] = React.useState<ISetupState>({ ipaddress: "10.0.0.201", host: "" });

    React.useEffect(() => {
    
        const callapi = () => {
            hostURL()
                .then(response => {
                    response.text().then(value => {
                        setState({ ipaddress: state.ipaddress, host: value });
                    });
                });
        };

        callapi();

    }, []);

    React.useEffect(() => {

        const connection = props.server?.connection;

        if (connection) {
            connection
                .start()
                .then(() => {
                    console.log("Connection started. Waiting for response from server...");
                    connection.on('ReceiveConnectConfirmation', message => {
                        console.log(message);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }

        return () => {
            // clean up
            if (connection) {
                connection.off("ReceiveConnectConfirmation");
            }
        };
    }, [props?.server?.connection]);

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setState({ ipaddress: event.target.value, host: state.host });
    };

    const save = () => {
        if (props?.server?.connection) {
            props?.server?.connection.send("SendConnect", state.ipaddress);
        }
    };

    const goLive = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (props?.server?.connection) {
            if (event.target.checked) {
                props?.server?.connection.send("SendStartStreaming");
            }
            else {
                props.server.connection.send("SendStopStreaming");
            }
        }
    }

    return <section className="setup">
        <ConnectionMonitor connection={props?.server?.connection ?? null} />
        <h3>Setup</h3>
        <div className="well">
            <div className="input-group mb-3">
                <div className="input-group-prepend">
                    <span className="input-group-text" id="basic-addon1">ATEM IP Address:</span>
                </div>
                <input type="text" className="form-control" placeholder={state.ipaddress} aria-label="ipaddress" aria-describedby="basic-addon1" onChange={ handleChange } />
            </div>
            <button type="button" className="btn btn-primary" onClick={e => { save() }}>Save</button>
        </div>
        <h3>Streaming</h3>
        <div className="well">
            <div className="custom-control custom-switch">
                <input type="checkbox" className="custom-control-input" id="customSwitch1" onChange={ goLive } />
                <label className="custom-control-label" htmlFor="customSwitch1">Enable the live stream</label>
            </div>
        </div>
        <h3>Instructions</h3>
        <div className="well" style={{ display: "block" }}>
            <p>Open <span>{state.host}</span> in a browser to access this system. </p>
            <div>
                <QRCode value={state.host} />
            </div>
        </div>
    </section>
}

export default Setup;