import * as React from "react";
import { HubConnection, HubConnectionState, LogLevel } from "@microsoft/signalr";
import { AlertTriangle, Zap } from "react-feather";

interface IConnectionMonitor {
    connection: HubConnection | null;
}

const ConnectionMonitor = ({ connection } : IConnectionMonitor): React.ReactElement => {

    const [switchConnection, setSwitchConnection] = React.useState<boolean>(false);
    const [serverConnection, setServerConnection] = React.useState<boolean>(false);

    React.useEffect(() => {
        if (connection) {

            console.log(`ConnectionMonitor: ${connection.state}`);
            setServerConnection(connection?.state === HubConnectionState.Connected);

            connection.onreconnected(id => {
                console.log(`ConnectionMonitor connected to server: ${id}`);
                setServerConnection(connection?.state === HubConnectionState.Connected);
            });
            connection.on("ReceiveConnectionStatus", message => {
                console.log(`ReceiveConnectionStatus - ${message}`);
                setServerConnection(connection?.state === HubConnectionState.Connected);
                setSwitchConnection(message);
            });
        }
    }, [connection]);

    return <div className="float-right">
        <span className={serverConnection ? "tab connection-status connected" : "tab connection-status"} title="Server Connection">{serverConnection ? <Zap /> : <AlertTriangle />}</span>
        <span className={switchConnection ? "tab connection-status connected" : "tab connection-status"} title="Switch Connection">{switchConnection ? <Zap /> : <AlertTriangle />}</span>
    </div>
}

export default ConnectionMonitor;