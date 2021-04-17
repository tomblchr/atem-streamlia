import * as React from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import ConnectionMonitor from "./ConnectionMonitor";
import MasterAudioMeter from "./MasterAudioMeter";

const Audio = (): React.ReactElement => {

    const [connection, setConnection] = React.useState<HubConnection | null>(null);

    React.useEffect(() => {
        console.log("Creating signalr connection...");

        const newConnection: HubConnection = connection ?? new HubConnectionBuilder()
            .withUrl("/atemhub")
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: retryContext => {
                    if (retryContext.elapsedMilliseconds < 60000) {
                        console.log("Will try to connect to server again in 1 second");
                        return 1000;
                    }
                    console.log("Will try to connect to server again in 6 seconds");
                    return 6000;
                }
            })
            .configureLogging(LogLevel.None)
            .build();

        setConnection(newConnection);

    }, []);

    React.useEffect(() => {
        if (connection) {
            connection
                .start()
                .then(() => {
                    console.log("Connected!");
                    connection.onreconnected(id => {
                        console.log(`Connection restored - ${id}`);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }

        return () => {
            // clean up
            if (connection) {
                connection.stop();
            }
        };

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