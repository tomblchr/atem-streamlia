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


interface ISceneDetail {
    program: number;
    preview: number;
    inputs: IInput[];
    downstreamKeyOnAir: boolean;
    downstreamKeyTieOn: boolean;
}

const Switcher = (props: IConnectToServer): React.ReactElement<IConnectToServer> => {

    const [scene, setScene] = React.useState<ISceneDetail | null>(null);

    React.useEffect(() => {
        if (props?.server?.connection) {
            props?.server?.connection
                .start()
                .then(() => {
                    console.log("Connected!");
                    props?.server?.connection.on("ReceiveSceneChange", message => {
                        const msg = message as ISceneDetail;
                        console.log(`ReceiveSceneChange - ${msg.downstreamKeyOnAir}`);
                        setScene(message);
                    });
                    props?.server?.connection.onreconnected(id => {
                        console.log(`Connection restored - ${id}`);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }

        return () => {
            // clean up
            if (props?.server?.connection) {
                props?.server?.connection.stop();
            }
        };
    }, [props?.server?.connection]);

    return (
        <div key="switcher">
            <ConnectionMonitor connection={props?.server?.connection ?? null} />
            <Inputs program={scene?.program} preview={scene?.preview} inputs={scene?.inputs} connection={props?.server?.connection ?? null} />
            <Transitions connection={props?.server?.connection ?? null} />
            <NextTransition connection={props?.server?.connection ?? null} />
            <TransitionStyle connection={props?.server?.connection ?? null} />
            <KeyFrameRunner connection={props?.server?.connection ?? null} />
            <Macros connection={props?.server?.connection ?? null} />
            <DownstreamKey connection={props?.server?.connection ?? null} onAir={scene?.downstreamKeyOnAir ?? false} tieOn={scene?.downstreamKeyTieOn ?? false} />
            <FadeToBlack connection={props?.server?.connection ?? null} />
        </div>
    )
}

export default Switcher;