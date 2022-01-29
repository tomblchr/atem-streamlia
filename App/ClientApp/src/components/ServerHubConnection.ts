import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

class ServerHubConnection {

    public connection: HubConnection;

    public constructor(hostname: string) {
        
        var url: string = "/atemhub";
        if (hostname && hostname !== "localhost" && hostname !== "") {
            // localhost here means the client appis being served up by the agent/backend service
            url = this.getValidUrl(hostname, url);
        }

        const newConnection: HubConnection = new HubConnectionBuilder()
            .withUrl(url)
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

        console.log(`Starting the signalr server connection at ${url}`);

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

    getValidUrl(hostname: string, path: string): string {
        try {
            let urlToTry = `https://${hostname}${path}`;
            new URL(urlToTry);
            return urlToTry;
        }
        catch(x) {
            console.error(`${hostname} is not a valid host. This should be an IP address or host name.`);
            return path;
        }
    }
}

export default ServerHubConnection;