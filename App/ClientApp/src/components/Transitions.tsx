import * as React from "react";
import { HubConnection } from "@microsoft/signalr";
import * as Log from "../api/log";

export interface ITransitionsProps {
    connection: HubConnection | undefined;
}

interface ITransistionState {
    inTransition: boolean;
    position: number;
    framesRemaining: number;
}

const Transitions = ({ connection }: ITransitionsProps): React.ReactElement => {

    const [state, setState] = React.useState<ITransistionState>({ inTransition: false, position: 0, framesRemaining: 0 });

    const sendAutoTransition = async (): Promise<void> => {
        await connection?.send("SendAutoTransition")
            .then(() => { Log.debug(`Auto Transition`) })
            .catch(e => Log.error("Auto transition failed: ", e));
    };

    const sendCutTransition = async (): Promise<void> => {
        await connection?.send("SendCutTransition")
            .then(() => { Log.debug(`Cut Transition`) })
            .catch(e => Log.error("Cut transition failed: ", e));
    };

    React.useEffect(() => {

        connection?.on("ReceiveInTransition", message => {
            const it: boolean = message;
            Log.debug(`ReceiveInTransition - ${it}`);
            setState(s => {return {...s, inTransition: it}});
        });
        connection?.on("ReceiveTransitionPosition", message => {
            const it: ITransistionState = message;
            Log.debug(`ReceiveTransitionPosition - ${it.position},${it.framesRemaining}`);
            setState(s => { return {...s, inTransition: it.inTransition || it.position > 0 }});
        });

        return () => {
            connection?.off("ReceiveInTransition");
            connection?.off("ReceiveTransitionPosition");
        }
    }, [connection]);

    const sliderChange = (value: string): void => {
        Log.debug(`Transition slider set to ${value}`);
    }

    return <section className="transition">
        <h3>Transition</h3>
        <div className="well">
            <div className="button" onClick={sendCutTransition}>
                <p>CUT</p>
            </div>
            <div className={state.inTransition ? "button red" : "button"} onClick={sendAutoTransition}>
                <p>AUTO</p>
            </div>
            <input className="slider" type="range" min="0" max="1" step="0.0001" value={state.position} onChange={e => sliderChange(e.target.value)} />
        </div>
    </section>
}

export default Transitions;