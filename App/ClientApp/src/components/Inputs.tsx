import * as React from "react";
import { HubConnection, HubConnectionState } from "@microsoft/signalr";
import Toaster from "./Toaster";

export interface IInput {
    id: number;
    name: string;
    longName: string;
    inputType: number;
}

export interface IInputsProps {
    inputs: IInput[] | undefined;
    program: number | undefined;
    preview: number | undefined;
    connection: HubConnection | undefined;
}

const Inputs = ({inputs, program, preview, connection}: IInputsProps): React.ReactElement => {

    const inputPorts = [1702392942, 1651269995, 1836082796];
    const [hubConnected, setHubConnected] = React.useState<boolean>(true);

    const connected = (): boolean => {
        const result = connection?.state === HubConnectionState.Connected;
        setHubConnected(result);
        return result;
    }

    const sendProgramChange = async (channel: number): Promise<void> => {

        if (!connected()) return;

        await connection?.send("SendProgramChange", channel)
            .then(() => { console.log(`Program change: ${channel}`) })
            .catch(e => console.log("SendProgramChange failed: ", e));
    };

    const sendPreviewChange = async (channel: number): Promise<void> => {

        if (!connected()) return;

        await connection?.send("SendPreviewChange", channel)
            .then(() => { console.log(`Preview change: ${channel}`) })
            .catch(e => console.log("SendPreviewChange failed: ", e));
    };

    return <section className="channels">
        <Toaster message="Not connected to server" show={hubConnected} error={true} />
        <h3>Program</h3>
        <div className="well">
            {inputs?.filter(i => inputPorts.includes(i.inputType)).map(i => (
                <div key={i.id} 
                    className={program === i.id ? "button red" : "button"}
                    onClick={c => sendProgramChange(i.id)}>
                    <p title={`${i.longName} - ${i.inputType}`}>{i.name}</p>
                </div>
            ))}
        </div>
        <h3>Preview</h3>
        <div className="well">
            {inputs?.filter(i => inputPorts.includes(i.inputType)).map(i => (
                <div key={i.id} 
                    className={preview === i.id ? "button green" : "button"}
                    onClick={c => sendPreviewChange(i.id)}>
                    <p title={`${i.longName} - ${i.inputType}`}>{i.name}</p>
                </div>
            ))}
        </div>
    </section>
}

export default Inputs;