import * as React from "react";
import { HubConnection } from "@microsoft/signalr";

export interface IDownstreamKeyProps {
    onAir: boolean;
    tieOn: boolean;
    connection: HubConnection | undefined;
}

interface IDownstreamKeyState {
    inTransition: boolean;
}

const DownstreamKey = ({onAir, tieOn, connection }: IDownstreamKeyProps): React.ReactElement => {

    const [state, setState] = React.useState<IDownstreamKeyState>({ inTransition: false });

    const sendDownstreamKeyOnAir = async (on: boolean): Promise<void> => {
        await connection?.send("SendDownstreamKeyOnAir", on)
            .then(() => { console.log(`DownstreamKeyOnAir`) })
            .catch(e => console.log("DownstreamKeyOnAir failed: ", e));
    };

    const sendDownstreamKeyTieOn = async (on: boolean): Promise<void> => {
        await connection?.send("SendDownstreamKeyTie", on)
            .then(() => { console.log(`DownstreamKeyTie`) })
            .catch(e => console.log("SendDownstreamKeyTie failed: ", e));
    };

    const sendDownstreamKeyAutoTransition = async (): Promise<void> => {
        await connection?.send("SendDownstreamKeyAutoTransition")
            .then(() => { console.log(`DownstreamKeyAutoTransition`) })
            .catch(e => console.log("DownstreamKeyAutoTransition failed: ", e));
    };

    React.useEffect(() => {

        connection?.on("ReceiveDownstreamKeyInTransition", message => {
            console.log(`ReceiveDownstreamKeyInTransition - ${message}`);
            setState({ inTransition: message });
        });
        connection?.on("ReceiveTransitionPosition", message => {
            //console.log(`ReceiveTransitionPosition - ${message}`);
            //setState({ inTransition: state.inTransition, position: message.position, framesRemaining: message.framesRemaining });
        });

        return () => {
            // clean-up
            connection?.off("ReceiveDownstreamKeyInTransition");
            connection?.off("ReceiveTransitionPosition");
        }
    }, [connection]);

    return <section className="downstream-key">
        <h3>Downstream Key 1</h3>
        <div className="well">
            <div
                className={tieOn ? "button yellow" : "button"}
                onClick={c => { sendDownstreamKeyTieOn(!tieOn); }}>
                <p>TIE</p>
            </div>
            <div className={onAir ? "button red" : "button"}
                onClick={c => { sendDownstreamKeyOnAir(!onAir); }}>
                <p>ON<br/>AIR</p>
            </div>
            <div className={state.inTransition ? "button red" : "button" } onClick={c => { sendDownstreamKeyAutoTransition(); }}>
                <p>AUTO</p>
            </div>
        </div>
    </section>;
}

export default DownstreamKey;