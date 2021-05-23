import * as React from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import ConnectionMonitor from "./ConnectionMonitor";
import MasterAudioMeter from "./MasterAudioMeter";
import IConnectToServer from "./IConnectToServer";

const Audio = (props: IConnectToServer): React.ReactElement<IConnectToServer> => {

    return <section className="audio">
        <ConnectionMonitor connection={props?.server?.connection ?? null} />
        <h3>Audio</h3>
        <div className="well">
            <MasterAudioMeter connection={props?.server?.connection ?? null} />
        </div>
    </section>
}

export default Audio;