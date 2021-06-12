import * as React from "react";
import IConnectToServer from "./IConnectToServer";
import ConnectionMonitor from "./ConnectionMonitor";
import { IInput } from "./Inputs";
import ServerHubConnection from "./ServerHubConnection";

interface ISceneDetail {
    program: number;
    preview: number;
    inputs: IInput[];
}

interface ITallyLightState {
    chosen: number | null;
}

const TallyLight = (): React.ReactElement => {

    const [scene, setScene] = React.useState<ISceneDetail>({ program: 0, preview: 0, inputs: [] });
    const [state, setState] = React.useState<ITallyLightState>({ chosen: null });

    const [connection, setConnection] = React.useState<IConnectToServer>({ server: null});

    React.useEffect(() => {
        const newConnection = new ServerHubConnection();

        newConnection.connection.on('ReceiveSceneChange', message => {
            setScene(message);
        });

        setConnection({ server: newConnection });

        return () => {
            // clean up
            newConnection.connection.off("ReceiveSceneChange");
            newConnection.connection.stop();
        };

    }, []);

    const chooseInput = (input: number): void => {
        setState({ chosen: input });
    };

    const isLive: boolean = scene.inputs.filter(c => c.id === scene.program && c.id === state.chosen).length > 0;
    const isPreview: boolean = scene.inputs.filter(c => c.id === scene.preview && c.id === state.chosen).length > 0;

    return (
        <section className="channels">
            <ConnectionMonitor connection={connection?.server?.connection ?? null} />
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