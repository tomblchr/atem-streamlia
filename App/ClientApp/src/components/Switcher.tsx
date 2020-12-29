import * as React from "react";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import Inputs from "./Inputs";
import Transitions from "./Transitions";
import NextTransition from "./NextTransition";
import TransitionStyle from "./TransitionStyle";
import DownstreamKey from "./DownstreamKey";
import FadeToBlack from "./FadeToBlack";

interface ISceneDetail {
    program: number;
}

export default function Switcher(): JSX.Element {
    const [connection, setConnection] = React.useState<HubConnection | null>(null);
    const [scene, setScene] = React.useState<ISceneDetail | null>(null);

    React.useEffect(() => {
        console.log("Creating connection...");

        const newConnection: HubConnection = new HubConnectionBuilder()
            .withUrl("https://localhost:44373/atemhub")
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    React.useEffect(() => {
        if (connection) {
            connection
                .start()
                .then(() => {
                    console.log('Connected!');
                    connection.on('ReceiveSceneChange', message => {
                        setScene(message);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const sendSceneChange = async (): Promise<void> => {
        await connection?.send("SendSceneChange")
            .then(() => { })
            .catch(e => console.log('SendSceneChange failed: ', e));
    };

    return (
        <div id="switcher" className="screen" onClick={sendSceneChange}>
            <h2>{scene?.program}</h2>
            <Inputs />
            <Transitions />
            <NextTransition />
            <TransitionStyle />
            <DownstreamKey />
            <FadeToBlack />
        </div>
    );
}