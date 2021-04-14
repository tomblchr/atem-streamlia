import * as React from "react";
import { HubConnection } from "@microsoft/signalr";

export interface ITransitionsProps {
    connection: HubConnection | null;
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
            .then(() => { console.log(`Auto Transition`) })
            .catch(e => console.log("Auto transition failed: ", e));
    };

    const sendCutTransition = async (): Promise<void> => {
        await connection?.send("SendCutTransition")
            .then(() => { console.log(`Cut Transition`) })
            .catch(e => console.log("Cut transition failed: ", e));
    };

    React.useEffect(() => {
        if (connection) {
            connection.on("ReceiveInTransition", message => {
                const it: boolean = message;
                console.log(`ReceiveInTransition - ${it}`);
                setState({ inTransition: it, position: state.position, framesRemaining: state.framesRemaining });
            });
            connection.on("ReceiveTransitionPosition", message => {
                const it: ITransistionState = message;
                console.log(`ReceiveTransitionPosition - ${it.position},${it.framesRemaining}`);
                setState({ inTransition: it.inTransition || it.position > 0, position: it.position, framesRemaining: it.framesRemaining });
            });
        }
    }, [connection]);

    const sliderChange = (value: string): void => {
        console.log(`Transition slider set to ${value}`);
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