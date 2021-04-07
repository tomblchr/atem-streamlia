import * as React from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import ConnectionMonitor from "./ConnectionMonitor";
import MasterAudioMeter from "./MasterAudioMeter";

const Audio = (): React.ReactElement => {

    const [connection, setConnection] = React.useState<HubConnection | null>(null);

    React.useEffect(() => {
        console.log("Creating connection...");

        const newConnection: HubConnection = new HubConnectionBuilder()
            .withUrl("/atemhub")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Debug)
            .build();

        setConnection(newConnection);

    }, []);

    React.useEffect(() => {
        if (connection) {
            connection
                .start();
        }
    }, [connection]);

    return <section className="audio">
        <ConnectionMonitor connection={connection} />
        <h3>Audio</h3>
        <div className="well">
            <MasterAudioMeter connection={connection} />
        </div>
    </section>
}

export default Audio;