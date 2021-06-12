import * as React from "react";
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";

class ServerHubConnection {

    public connection: HubConnection;

    public constructor() {
        const newConnection: HubConnection = new HubConnectionBuilder()
            .withUrl("/atemhub")
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: retryContext => {
                    if (retryContext.elapsedMilliseconds < 60000) {
                        console.log("Try to connect to server again in 1 second");
                        return 1000;
                    }
                    console.log("Try to connect to server again in 6 seconds");
                    return 6000;
                }
            })
            .configureLogging(LogLevel.Information)
            .build();

        this.connection = newConnection;

        console.log("Starting the signalr server connection");

        this.connection
            .start()
            .then(() => {
                console.log("Request healthcheck");
                this.connection?.send("SendHealthCheckRequest");
            })
            .catch((err) => {
                console.error(`Unable to start signalr connection - ${err}`);
            });

        this.connection.onreconnected(id => {
            console.log(`Connection restored - ${id}`);
        });
    }
}

export default ServerHubConnection;