import * as React from "react";
import { HubConnection, HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { AlertTriangle, Zap } from "react-feather";
import Inputs, { IInput } from "./Inputs";
import Transitions from "./Transitions";
import NextTransition from "./NextTransition";
import TransitionStyle from "./TransitionStyle";
import DownstreamKey from "./DownstreamKey";
import FadeToBlack from "./FadeToBlack";

interface ISceneDetail {
    program: number;
    preview: number;
    inputs: IInput[];
}

export default function Switcher(): JSX.Element {
    const [connection, setConnection] = React.useState<HubConnection | null>(null);
    const [scene, setScene] = React.useState<ISceneDetail | null>(null);
    const [switchConnection, setSwitchConnection] = React.useState<boolean>(false);

    React.useEffect(() => {
        console.log("Creating connection...");

        const newConnection: HubConnection = new HubConnectionBuilder()
            .withUrl("/atemhub")
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    React.useEffect(() => {
        if (connection) {
            connection
                .start()
                .then(() => {
                    console.log("Connected!");
                    connection.on("ReceiveSceneChange", message => {
                        console.log(`ReceiveSceneChange - ${message}`);
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
        <div>
            <div className="float-right">
                {connection?.state === HubConnectionState.Connected ? <span className="tab connection-status connected" title="Server Connection"><Zap /></span> : <AlertTriangle />}
                {switchConnection ? <span className="tab connection-status connected" title="Switch Connection"><Zap /></span> : <AlertTriangle />}
            </div>
            <Inputs program={scene?.program} preview={scene?.preview} inputs={scene?.inputs} connection={connection} />
            <Transitions connection={connection} />
            <NextTransition />
            <TransitionStyle />
            <DownstreamKey />
            <FadeToBlack />
        </div>
    );
}