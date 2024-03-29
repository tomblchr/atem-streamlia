import * as React from "react";
import { HubConnection } from "@microsoft/signalr";
import * as Log from "../api/log";

export interface ITransitionStyleProps {
    connection: HubConnection | undefined;
}

enum SwitcherTransitionStyle {
    None,
    Mix = 1835628664,
    Dip = 1684631664,
    Wipe = 2003398757,
    DVE = 1685480805,
    Stinger = 1937010279
}

interface ITransitionStyleState {
    current: SwitcherTransitionStyle
}

const TransitionStyle = ({ connection }: ITransitionStyleProps): React.ReactElement => {
    const [state, setState] = React.useState<ITransitionStyleState>({ current: SwitcherTransitionStyle.None });

    React.useEffect(() => {
        
        connection?.on("ReceiveTransitionStyle", message => {
            Log.debug(`ReceiveTransitionStyle - ${message}`);
            setState({ current: message });
        });

        return () => {
            connection?.off("ReceiveTransitionStyle");
        }

    }, [connection]);   

    const sendTransitionStyle = async (transition: SwitcherTransitionStyle): Promise<void> => {
        await connection?.send("SendTransitionStyle", transition)
            .then(() => { Log.debug(`SendTransitionStyle ${transition}`) })
            .catch(e => Log.error("SendTransitionStyle failed: ", e));
    };

    return <section className="transition-style">
        <h3>Transition style</h3>
        <div className="well">
            <div className={state.current === SwitcherTransitionStyle.Mix ? "button yellow" : "button"}
                onClick={c => { sendTransitionStyle(SwitcherTransitionStyle.Mix) }}>
                <p>MIX</p>
            </div>
            <div className={state.current === SwitcherTransitionStyle.Dip ? "button yellow" : "button"}
                onClick={c => { sendTransitionStyle(SwitcherTransitionStyle.Dip) }}>
                <p>DIP</p>
            </div>
            <div className={state.current === SwitcherTransitionStyle.Wipe ? "button yellow" : "button"}
                onClick={c => { sendTransitionStyle(SwitcherTransitionStyle.Wipe) }}>
                <p>WIPE</p>
            </div>
            <div className={state.current === SwitcherTransitionStyle.Stinger ? "button yellow" : "button"}
                onClick={c => { sendTransitionStyle(SwitcherTransitionStyle.Stinger) }}>
                <p>STING</p>
            </div>
            <div className={state.current === SwitcherTransitionStyle.DVE ? "button yellow" : "button"}
                onClick={c => { sendTransitionStyle(SwitcherTransitionStyle.DVE) }}>
                <p>DVE</p>
            </div>
            <div className="button">
                <p>PREV<br />TRAN</p>
            </div>
        </div>
    </section>;
}

export default TransitionStyle;