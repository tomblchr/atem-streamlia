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

    const [state, setState] = React.useState<INextTransitionState>({ includeBackground: false, keys: [{ key: 2, onAir: false, included: false }] });

    React.useEffect(() => {
        if (connection) {
            connection.on("ReceiveNextTransition", message => {
                setState(message);
            });
        }
    }, [connection]);
    
    const sendBackgroundIncluded = (includeBackground: boolean): void => {
        connection?.send("SendNextTransitionBackground", includeBackground)
            .then(() => { console.log(`SendNextTransitionBackground`) })
            .catch(e => console.log("SendNextTransitionBackground failed: ", e));
    };

    const sendKeyOnAir = (key: number, onAir: boolean): void => {
        connection?.send("SendKeyOnAir", key, onAir)
            .then(() => { console.log(`SendKeyOnAir (${key},${onAir})`) })
            .catch(e => console.log("SendKeyOnAir failed: ", e));
    };

    const sendKeyIncluded = (key: number, included: boolean): void => {
        connection?.send("SendKeyIncludedInTransition", key, included)
            .then(() => { console.log(`SendKeyIncludedInTransition (${key},${included})`) })
            .catch(e => console.log("SendKeyIncludedInTransition failed: ", e));
    };
 
    return <section className="next-transition">
        <h3>Next Transition</h3>
        <div className="well">
            <div style={{ display: "flex", flexDirection: "column" }}>
                <div className="button" style={{ visibility: "hidden" }}>
                    <p>T</p>
                </div>
                <div className={state.includeBackground ? "button yellow" : "button"} onClick={_ => { sendBackgroundIncluded(!state.includeBackground); }}>
                    <p>BKGD</p>
                </div>
            </div>
            {state.keys.map((c, index) => (
                <div key={c.key} style={{ display: "flex", flexDirection: "column" }}>
                    <div className={c.onAir ? "button red" : "button"} onClick={_ => { sendKeyOnAir(c.key, !c.onAir); }}>
                        <p>ON<br />AIR</p>
                    </div>
                    <div className={c.included ? "button yellow" : "button"} onClick={_ => { sendKeyIncluded(c.key, !c.included); }}>
                        <p>Key {index + 1}</p>
                    </div>
                </div>
            ))}
        </div>
    </section>
}

export default NextTransition;