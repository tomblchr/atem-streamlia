import * as React from "react";
import { HubConnection } from "@microsoft/signalr";

export interface ITransitionsProps {
    connection: HubConnection | null;
}

interface ITransistionState {
    inTransition: boolean;
}

const Transitions = ({ connection }: ITransitionsProps): React.ReactElement => {

    const [state, setState] = React.useState<ITransistionState>({ inTransition: false });

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
                console.log(`ReceiveInTransition - ${message}`);
                setState({ inTransition : message })
            });
        }
    }, [connection]);

    return <section className="transition">
        <h3>Transition</h3>
        <div className="well">
            <div className="button" onClick={sendCutTransition}>
                <p>CUT</p>
            </div>
            <div className={state.inTransition ? "button red" : "button"} onClick={sendAutoTransition}>
                <p>AUTO</p>
            </div>
            <div>
                <input className="slider" type="range" min="0" max="1" step="0.001" />
            </div>
        </div>
    </section>
}

export default Transitions;