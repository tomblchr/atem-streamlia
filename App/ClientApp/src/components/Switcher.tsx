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
import IConnectToServer from "./IConnectToServer";
import { SwitchProps } from "react-router";
import ServerHubConnection from "./ServerHubConnection";


interface ISceneDetail {
    program: number;
    preview: number;
    inputs: IInput[];
    downstreamKeyOnAir: boolean;
    downstreamKeyTieOn: boolean;
}

const Switcher = (): React.ReactElement => {

    const [scene, setScene] = React.useState<ISceneDetail | null>(null);

    const [connection, setConnection] = React.useState<IConnectToServer>(null);

    React.useEffect(() => {
        const newConnection = new ServerHubConnection();

        newConnection.connection.on("ReceiveSceneChange", message => {
            const msg = message as ISceneDetail;
            console.log(`ReceiveSceneChange - ${msg.downstreamKeyOnAir}`);
            setScene(message);
        });

        setConnection({ server: newConnection });

        return () => {
            // clean up
            newConnection.connection.stop();
        };

    }, []);

    return (
        <div key="switcher">
            <ConnectionMonitor connection={connection?.server?.connection} />
            <Inputs program={scene?.program} preview={scene?.preview} inputs={scene?.inputs} connection={connection?.server?.connection} />
            <Transitions connection={connection?.server?.connection} />
            <NextTransition connection={connection?.server?.connection} />
            <TransitionStyle connection={connection?.server?.connection} />
            <KeyFrameRunner connection={connection?.server?.connection} />
            <Macros connection={connection?.server?.connection} />
            <DownstreamKey connection={connection?.server?.connection} onAir={scene?.downstreamKeyOnAir ?? false} tieOn={scene?.downstreamKeyTieOn ?? false} />
            <FadeToBlack connection={connection?.server?.connection} />
        </div>
    )
}

export default Switcher;