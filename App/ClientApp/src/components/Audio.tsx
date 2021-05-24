import * as React from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import ConnectionMonitor from "./ConnectionMonitor";
import MasterAudioMeter from "./MasterAudioMeter";
import IConnectToServer from "./IConnectToServer";
import ServerHubConnection from "./ServerHubConnection";

const Audio = (): React.ReactElement => {

    const [connection, setConnection] = React.useState<IConnectToServer>({ server: null});

    React.useEffect(() => {
        const newConnection = new ServerHubConnection();

        setConnection({ server: newConnection });

        return () => {
            // clean up
            newConnection.connection.stop();
        };

    }, []);

    return <section className="audio">
        <ConnectionMonitor connection={connection?.server?.connection ?? null} />
        <h3>Audio</h3>
        <div className="well">
            <MasterAudioMeter connection={connection?.server?.connection ?? null} />
        </div>
    </section>
}

export default Audio;