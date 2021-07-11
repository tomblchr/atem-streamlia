import * as React from "react";
import Inputs, { IInput } from "./Inputs";
import Transitions from "./Transitions";
import NextTransition from "./NextTransition";
import TransitionStyle from "./TransitionStyle";
import DownstreamKey from "./DownstreamKey";
import FadeToBlack from "./FadeToBlack";
import KeyFrameRunner from "./KeyFrameRunner";
import Macros from "./Macros";
import ServerHubConnection from "./ServerHubConnection";
import { HubConnectionState } from "@microsoft/signalr";

interface ISwitcherProps {
    onLivestreamUrlChange: Function;
    server: ServerHubConnection | undefined;
}

interface ISceneDetail {
    program: number;
    preview: number;
    inputs: IInput[];
    downstreamKeyOnAir: boolean;
    downstreamKeyTieOn: boolean;
}

const Switcher = ({ server, onLivestreamUrlChange }: ISwitcherProps): React.ReactElement => {

    const [scene, setScene] = React.useState<ISceneDetail | undefined>();

    //const [connection, setConnection] = React.useState<IConnectToServer>({ server: null});

    React.useEffect(() => {

        server?.connection.on("ReceiveSceneChange", message => {
            const msg = message as ISceneDetail;
            console.log(`ReceiveSceneChange - ${msg.downstreamKeyOnAir}`);
            setScene(message);
        });
        server?.connection.on("ReceiveLivestreamPreviewUrl", message => {
            console.log(`ReceiveLivestreamPreviewUrl - ${message}`);
            onLivestreamUrlChange(message);
        });

        if (server?.connection.state === HubConnectionState.Connected) {
            server?.connection.send("SendSceneChange");
            server?.connection.send("SendMacros");
        }

        return () => {
            server?.connection.off("ReceiveSceneChange");
            server?.connection.off("ReceiveLivestreamPreviewUrl");
        };

    }, [server]);

    return (
        <div key="switcher">
            <Inputs program={scene?.program} preview={scene?.preview} inputs={scene?.inputs} connection={server?.connection} />
            <Transitions connection={server?.connection} />
            <NextTransition connection={server?.connection} />
            <TransitionStyle connection={server?.connection} />
            <KeyFrameRunner connection={server?.connection} />
            <Macros connection={server?.connection} />
            <DownstreamKey connection={server?.connection} onAir={scene?.downstreamKeyOnAir ?? false} tieOn={scene?.downstreamKeyTieOn ?? false} />
            <FadeToBlack connection={server?.connection} />
        </div>
    )
}

export default Switcher;