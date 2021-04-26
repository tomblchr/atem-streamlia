import * as React from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import ConnectionMonitor from "./ConnectionMonitor";
import { IInput } from "./Inputs";

interface ISceneDetail {
    program: number;
    preview: number;
    inputs: IInput[];
}

interface ITallyLightState {
    chosen: number | null;
}

const TallyLight = (): React.ReactElement => {
    const [connection, setConnection] = React.useState<HubConnection | null>(null);
    const [scene, setScene] = React.useState<ISceneDetail>({ program: 0, preview: 0, inputs: [] });
    const [state, setState] = React.useState<ITallyLightState>({ chosen: null });    

    React.useEffect(() => {
        console.log("Creating signalr connection...");

        const newConnection: HubConnection = connection ?? new HubConnectionBuilder()
            .withUrl("/atemhub")
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: retryContext => {
                    if (retryContext.elapsedMilliseconds < 60000) {
                        console.log("Will try to connect to server again in 1 second");
                        return 1000;
                    }
                    console.log("Will try to connect to server again in 6 seconds");
                    return 6000;
                }
            })
            .configureLogging(LogLevel.None)
            .build();

        setConnection(newConnection);
    }, []);

    React.useEffect(() => {
        if (connection) {
            connection
                .start()
                .then(() => {
                    console.log("Connected!");
                    connection.onreconnected(id => {
                        console.log(`Connection restored - ${id}`);
                    });
                    connection.on('ReceiveSceneChange', message => {
                        setScene(message);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }

        return () => {
            // clean up
            if (connection) {
                connection.stop();
            }
        };
    }, [connection]);

    const chooseInput = (input: number): void => {
        setState({ chosen: input });
    };

    const isLive: boolean = scene.inputs.filter(c => c.id === scene.program && c.id === state.chosen).length > 0;
    const isPreview: boolean = scene.inputs.filter(c => c.id === scene.preview && c.id === state.chosen).length > 0;

    return (
        <section className="channels">
            <ConnectionMonitor connection={connection} />
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