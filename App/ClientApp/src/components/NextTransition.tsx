import * as React from "react";
import { HubConnection } from "@microsoft/signalr";
import * as Log from "../api/log";
export interface IKeyProps {
    connection: HubConnection | undefined;
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
        
        connection?.on("ReceiveNextTransition", message => {
            setState(message);
        });

        return () => {
            connection?.off("ReceiveNextTransition");
        }

    }, [connection]);
    
    const sendBackgroundIncluded = (includeBackground: boolean): void => {
        connection?.send("SendNextTransitionBackground", includeBackground)
            .then(() => { Log.debug(`SendNextTransitionBackground`) })
            .catch(e => Log.error("SendNextTransitionBackground failed: ", e));
    };

    const sendKeyOnAir = (key: number, onAir: boolean): void => {
        connection?.send("SendKeyOnAir", key, onAir)
            .then(() => { Log.debug(`SendKeyOnAir (${key},${onAir})`) })
            .catch(e => Log.error("SendKeyOnAir failed: ", e));
    };

    const sendKeyIncluded = (key: number, included: boolean): void => {
        connection?.send("SendKeyIncludedInTransition", key, included)
            .then(() => { Log.debug(`SendKeyIncludedInTransition (${key},${included})`) })
            .catch(e => Log.error("SendKeyIncludedInTransition failed: ", e));
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