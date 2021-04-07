import * as React from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import QRCode from "qrcode.react";
import { hostURL } from "../api/atemconnection";
import ConnectionMonitor from "./ConnectionMonitor";

interface ISetupState {
    ipaddress: string;
    host: string;
}

const Setup = (): React.ReactElement => {
    const [connection, setConnection] = React.useState<HubConnection | null>(null);
    const [state, setState] = React.useState<ISetupState>({ ipaddress: "10.0.0.201", host: "" });

    React.useEffect(() => {
        console.log("Creating connection...");

        const newConnection: HubConnection = new HubConnectionBuilder()
            .withUrl("/atemhub")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        const callapi = () => {
            hostURL()
                .then(response => {
                    response.text().then(value => {
                        setState({ ipaddress: state.ipaddress, host: value });
                    });
                });
        };

        setConnection(newConnection);
        callapi();

    }, []);

    React.useEffect(() => {
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
    }, [connection]);

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setState({ ipaddress: event.target.value, host: state.host });
    };

    const save = () => {
        if (connection) {
            connection.send("SendConnect", state.ipaddress);
        }
    };

    return <section className="setup">
        <ConnectionMonitor connection={connection} />
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
        <h3>Instructions</h3>
        <div className="well">
            <div>Open <span>{state.host}</span> in a browser to access this system. </div>
            <QRCode value={state.host} />
        </div>
    </section>
}

export default Setup;