import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
import * as Log from "../api/log";

class ServerHubConnection {

    public connection: HubConnection;

    public constructor(hostname: string) {
        
        var url: string = "/atemhub";
        if (hostname && hostname !== "localhost" && hostname !== "") {
            // localhost here means the client appis being served up by the agent/backend service
            url = this.getValidUrl(hostname, url);

            Log.info(`Connecting to ${url}...`);
        }

        const newConnection: HubConnection = new HubConnectionBuilder()
            .withUrl(url)
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: retryContext => {
                    if (retryContext.elapsedMilliseconds < 60000) {
                        Log.debug("Will attempt to connect to server again in 1 second");
                        return 1000;
                    }
                    Log.debug("Will attempt to connect to server again in 6 seconds");
                    return 6000;
                }
            })
            .configureLogging(new Log.AtemLogger(LogLevel.Error))
            .build();

        this.connection = newConnection;

        Log.debug(`Starting the signalr server connection at ${url}`);

        this.ignition();

        this.connection.onreconnected(id => {
            Log.debug(`Connection restored - ${id}`);
        });
    }

    /* start the signalr connection */
    public ignition() {
        if (this.connection.state === HubConnectionState.Connected) {
            Log.debug("Good news. We're connected!");
            return;            
        }

        this.connection
            .start()
            .then(() => {
                Log.debug("Request healthcheck");
                this.connection?.send("SendHealthCheckRequest");
            })
            .catch((err) => {
                Log.error(`Unable to start signalr connection - ${err}`);
            });
    }

    getValidUrl(hostname: string, path: string): string {
        try {
            let urlToTry = `https://${hostname}${path}`;
            new URL(urlToTry);
            return urlToTry;
        }
        catch(x) {
            Log.error(`${hostname} is not a valid host. This should be an IP address or host name.`);
            return path;
        }
    }
}

export default ServerHubConnection;