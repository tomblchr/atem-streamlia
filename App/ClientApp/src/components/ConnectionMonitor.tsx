import * as React from "react";
import { HubConnection, HubConnectionState } from "@microsoft/signalr";
import { AlertTriangle, Server, Radio, StopCircle, Sliders } from "react-feather";
import * as Log from "../api/log";

interface IConnectionMonitor {
    connection: HubConnection | undefined;
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

            Log.info(`ConnectionMonitor: ${connection.state}`);
            setState({ switchConnection: false, serverConnection: connection?.state === HubConnectionState.Connected });
            connection.onreconnecting(error => {
                Log.warn(`Reconnecting because - ${error?.message}`);
                setState({ serverConnection: false, switchConnection: false });
            });
            connection.onreconnected(id => {
                Log.info(`Connected to server: ${id}`);
                setState({ switchConnection: false, serverConnection: connection?.state === HubConnectionState.Connected });
            });
            connection.on("ReceiveConnectConfirmation", message => {
                setState({ switchConnection: true, serverConnection: true });
            });
            connection.on("ReceiveConnectionStatus", message => {
                Log.debug(`ReceiveConnectionStatus - ${message}`);
                setState({ switchConnection: message, serverConnection: true });
            });
            connection.on("ReceiveStreamingStatus", message => {
                Log.debug(`ReceiveStreamingStatus - ${message}`);
                setStreaming({ isStreaming: message });
            });
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