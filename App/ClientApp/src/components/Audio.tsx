import * as React from "react";
import MasterAudioMeter from "./MasterAudioMeter";
import ServerHubConnection from "./ServerHubConnection";

interface IAudioProps {
    server: ServerHubConnection | undefined;
}

const Audio = ({ server }: IAudioProps): React.ReactElement => {

    return <section className="audio">
        <h3>Audio</h3>
        <div className="well">
            <MasterAudioMeter connection={server} />
        </div>
    </section>
}

export default Audio;