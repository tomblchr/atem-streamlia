import * as React from "react";
import { HubConnection, HubConnectionState, LogLevel } from "@microsoft/signalr";
import { AlertTriangle, Camera, Zap, Server, Radio, StopCircle, Sliders } from "react-feather";

interface IConnectionMonitor {
    connection: HubConnection | null;
}

interface IConnectionMonitorState {
    switchConnection: boolean;
    serverConnection: boolean;
}

interface IStreamingState {
    isStreaming: boolean;
}

const ConnectionMonitor = ({ connection } : IConnectionMonitor): React.ReactElement => {

    const [state, setState] = React.useState<IConnectionMonitorState>({ switchConnection: false, serverConnection: false });
    const [streaming, setStreaming] = React.useState<IStreamingState>({ isStreaming: false });

    React.useEffect(() => {
        if (connection) {

            console.log(`ConnectionMonitor: ${connection.state}`);
            setState({ switchConnection: false, serverConnection: connection?.state === HubConnectionState.Connected });
            connection.onreconnecting(error => {
                console.log(`Reconnecting because - ${error?.message}`);
                setState({ serverConnection: false, switchConnection: false });
            });
            connection.onreconnected(id => {
                console.log(`ConnectionMonitor connected to server: ${id}`);
                setState({ switchConnection: false, serverConnection: connection?.state === HubConnectionState.Connected });
            });
            connection.on("ReceiveConnectConfirmation", message => {
                setState({ switchConnection: true, serverConnection: true });
            });
            connection.on("ReceiveConnectionStatus", message => {
                console.log(`ReceiveConnectionStatus - ${message}`);
                setState({ switchConnection: message, serverConnection: true });
            });
            connection.on("ReceiveStreamingStatus", message => {
                console.log(`ReceiveStreamingStatus - ${message}`);
                setStreaming({ isStreaming: message });
            })
        }

        return () => {
            // cleanup
            if (connection) {
                connection.off("ReceiveConnectConfirmation");
                connection.off("ReceiveConnectionStatus");
                connection.off("ReceiveStreamingStatus");
            }
        }
    }, [connection]);

    return <div className="float-right">
        <span className={state.serverConnection ? "tab connection-status connected" : "tab connection-status"} title="Server Connection">{state.serverConnection ? <Server /> : <AlertTriangle />}</span>
        <span className={state.switchConnection ? "tab connection-status connected" : "tab connection-status"} title="Switch Connection">{state.switchConnection ? <Sliders /> : <AlertTriangle />}</span>
        <span className={streaming.isStreaming ? "tab connection-status connected" : "tab connection-status"} title="Streaming">{streaming.isStreaming ? <Radio /> : <StopCircle />}</span>
    </div>
}

export default ConnectionMonitor;