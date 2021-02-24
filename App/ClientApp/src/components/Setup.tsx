import * as React from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import ConnectionMonitor from "./ConnectionMonitor";

interface ISetupState {
    ipaddress: string;
}

const Setup = (): React.ReactElement => {
    const [connection, setConnection] = React.useState<HubConnection | null>(null);
    const [state, setState] = React.useState<ISetupState>({ ipaddress: "10.0.0.201" });

    React.useEffect(() => {
        console.log("Creating connection...");

        const newConnection: HubConnection = new HubConnectionBuilder()
            .withUrl("/atemhub")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        setConnection(newConnection);
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
        setState({ ipaddress: event.target.value });
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
                    <span className="input-group-text" id="basic-addon1">IP Address:</span>
                </div>
                <input type="text" className="form-control" placeholder={state.ipaddress} aria-label="ipaddress" aria-describedby="basic-addon1" onChange={ handleChange } />
            </div>
            <button type="button" className="btn btn-primary" onClick={e => { save() }}>Save</button>
        </div>
    </section>
}

export default Setup;