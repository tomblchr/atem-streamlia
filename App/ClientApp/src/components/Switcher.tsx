import * as React from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import ConnectionMonitor from "./ConnectionMonitor";
import Inputs, { IInput } from "./Inputs";
import Transitions from "./Transitions";
import NextTransition from "./NextTransition";
import TransitionStyle from "./TransitionStyle";
import DownstreamKey from "./DownstreamKey";
import FadeToBlack from "./FadeToBlack";
import KeyFrameRunner from "./KeyFrameRunner";
import Macros from "./Macros";


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
                    connection.on("ReceiveSceneChange", message => {
                        const msg = message as ISceneDetail;
                        console.log(`ReceiveSceneChange - ${msg.downstreamKeyOnAir}`);
                        setScene(message);
                    });
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

    return (
        <div key="switcher">
            <ConnectionMonitor connection={connection} />
            <Inputs program={scene?.program} preview={scene?.preview} inputs={scene?.inputs} connection={connection} />
            <Transitions connection={connection} />
            <NextTransition connection={connection} />
            <TransitionStyle connection={connection} />
            <KeyFrameRunner connection={connection} />
            <Macros connection={connection} />
            <DownstreamKey connection={connection} onAir={scene?.downstreamKeyOnAir ?? false} tieOn={scene?.downstreamKeyTieOn ?? false} />
            <FadeToBlack connection={connection} />
        </div>
    )
}