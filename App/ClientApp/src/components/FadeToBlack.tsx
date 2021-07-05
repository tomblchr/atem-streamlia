﻿import * as React from "react";
import { HubConnection } from "@microsoft/signalr";

export interface ITransitionsProps {
    connection: HubConnection | undefined;
}

interface IFadeToBlackState {
    isInTransition: boolean;
    isFullyBlack: boolean;
}

const FadeToBlack = ({connection} : ITransitionsProps): React.ReactElement => {

    const [state, setState] = React.useState<IFadeToBlackState>({ isFullyBlack: false, isInTransition: false });

    const sendFadeToBlackTransition = async (toBlack: boolean): Promise<void> => {
        await connection?.send("SendFadeToBlackTransition")
            .then(() => { console.log(`FadeToBlack`) })
            .catch(e => console.log("FadeToBlack failed: ", e));
    };

    React.useEffect(() => {
        
        connection?.on("ReceiveIsFadeToBlack", message => {
            console.log(`ReceiveIsFadeToBlack - ${message}`);
            setState(message);
        });

        return () => {
            // clean-up
            connection?.off("ReceiveIsFadeToBlack");
        }

    }, [connection]);    

    return <section className="fade-to-black">
        <h3>Fade to Black</h3>
        <div className="well">
            <div className={state.isFullyBlack ? "button red flashing" : state.isInTransition ? "button red" : "button"} onClick={c => sendFadeToBlackTransition(false)}>
                <p>FTB</p>
            </div>
        </div>
    </section>;
}

export default FadeToBlack;