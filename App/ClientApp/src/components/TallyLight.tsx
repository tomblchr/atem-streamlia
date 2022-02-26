import * as React from "react";
import { IInput } from "./Inputs";
import ServerHubConnection from "./ServerHubConnection";
import { HubConnectionState } from "@microsoft/signalr";

interface ISceneDetail {
    program: number;
    preview: number;
    inputs: IInput[];
}

interface ITallyLightProps {
    server: ServerHubConnection | undefined;
}

interface ITallyLightState {
    chosen: number | null;
}

const TallyLight = ({ server }: ITallyLightProps): React.ReactElement => {

    const [scene, setScene] = React.useState<ISceneDetail>({ program: 0, preview: 0, inputs: [] });
    const [state, setState] = React.useState<ITallyLightState>({ chosen: null });

    React.useEffect(() => {

        server?.connection.on("ReceiveSceneChange", message => {
            setScene(message);
        });

        if (server?.connection.state === HubConnectionState.Connected) {
            server?.connection.send("SendSceneChange");
        } else {
            console.debug("SignalR not connected to server");
        }

        return () => {
            // clean up
            server?.connection.off("ReceiveSceneChange");
        };

    }, [server]);

    const chooseInput = (input: number): void => {
        setState({ chosen: input });
    };

    const isLive: boolean = scene.inputs.filter(c => c.id === scene.program && c.id === state.chosen).length > 0;
    const isPreview: boolean = scene.inputs.filter(c => c.id === scene.preview && c.id === state.chosen).length > 0;

    return (
        <section className="channels">
            <h3>Tally Light</h3>
            <div className={isLive ? "well tally active" : "well tally"}>
                <p>Camera {isLive ? "LIVE!" : isPreview ? "PREVIEW!" : "Off" }</p>
            </div>
            <h3>Choose Your Camera</h3>
            <div className="well">
                {scene.inputs.map(i => (
                    <div key={i.id} className={state.chosen === i.id ? "button red" : "button"} onClick={c => chooseInput(i.id)}>
                        <p>{i.name}</p>
                    </div>
                ))}
            </div>
        </section>
    );
}

export default TallyLight;