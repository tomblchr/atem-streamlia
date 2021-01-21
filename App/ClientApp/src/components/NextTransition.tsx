import * as React from "react";
import { HubConnection } from "@microsoft/signalr";

export interface IKeyProps {
    connection: HubConnection | null;
}

interface IKeyState {
    key: number;
    onAir: boolean;
    included: boolean;
}

interface INextTransitionState {
    includeBackground: boolean;
    keys: IKeyState[];
}

const NextTransition = ({ connection }: IKeyProps): React.ReactElement => {

    const [state, setState] = React.useState<INextTransitionState>({ includeBackground: false, keys: [{ key: 0, onAir: false, included: false }] });

    React.useEffect(() => {
        if (connection) {
            connection.on("ReceiveNextTransition", message => {
                setState(message);
            });
        }
    }, [connection]);

    return <section className="next-transition">
        <h3>Next Transition</h3>
        <div className="well">
            <div className={state.includeBackground ? "button yellow" : "button"} onClick={c => { }}>
                <p>BKGD</p>
            </div>
            {state.keys.map((c, index) => (
                <React.Fragment key={c.key}>
                    <div className={c.onAir ? "button red" : "button"} onClick={c => { }}>
                        <p>ON<br />AIR</p>
                    </div>
                    <div className={c.included ? "button yellow" : "button"} onClick={c => { }}>
                        <p>Key {index + 1}</p>
                    </div>
                </React.Fragment>
            ))}
        </div>
    </section>
}

export default NextTransition;