import * as React from "react";
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr";
import { AlertTriangle, Zap } from "react-feather";
import Inputs, { IInput } from "./Inputs";
import Transitions from "./Transitions";
import NextTransition from "./NextTransition";
import TransitionStyle from "./TransitionStyle";
import DownstreamKey from "./DownstreamKey";
import FadeToBlack from "./FadeToBlack";
import MasterAudioMeter from "./MasterAudioMeter";

interface ISceneDetail {
    program: number;
    preview: number;
    inputs: IInput[];
    downstreamKeyOnAir: boolean;
    downstreamKeyTieOn: boolean;
}

export default function Switcher(): JSX.Element {
    const [connection, setConnection] = React.useState<HubConnection | null>(null);
    const [scene, setScene] = React.useState<ISceneDetail | null>(null);
    const [switchConnection, setSwitchConnection] = React.useState<boolean>(false);

    React.useEffect(() => {
        console.log("Creating connection...");

        const newConnection: HubConnection = connection ?? new HubConnectionBuilder()
            .withUrl("/atemhub")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Debug)
            .build();

        setConnection(newConnection);
    }, [connection, switchConnection]);

    React.useEffect(() => {
        if (connection) {
            connection
                .start()
                .then(() => {
                    console.log("Connected!");
                    connection.on("ReceiveSceneChange", message => {
                        const msg = message as ISceneDetail;
                        console.log(`ReceiveSceneChange - ${msg.downstreamKeyOnAir}`);
                        setScene(message);
                        setSwitchConnection(true);
                    });
                    connection.on("ReceiveConnectionStatus", message => {
                        console.log(`ReceivedConnectionStatus - ${message}`);
                        setSwitchConnection(message);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    return (
        <div key="switcher">
            <div className="float-right">
                {connection?.state === HubConnectionState.Connected ? <span className="tab connection-status connected" title="Server Connection"><Zap /></span> : <AlertTriangle />}
                {switchConnection ? <span className="tab connection-status connected" title="Switch Connection"><Zap /></span> : <AlertTriangle />}
            </div>
            <Inputs program={scene?.program} preview={scene?.preview} inputs={scene?.inputs} connection={connection} />
            <Transitions connection={connection} />
            <NextTransition />
            <TransitionStyle connection={connection} />
            <DownstreamKey connection={connection} onAir={scene?.downstreamKeyOnAir ?? false} tieOn={scene?.downstreamKeyTieOn ?? false} />
            <FadeToBlack connection={connection} />
        </div>
    )
}