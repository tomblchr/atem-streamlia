import * as React from "react";
import PeakMeter from "./PeakMeter";
import ServerHubConnection from "./ServerHubConnection";

export interface IMasterAudioProps {
    connection: ServerHubConnection | undefined;
}

interface IMasterAudioState {
    levels: number[];
    peaks: number[];
}

const MasterAudioMeter = ({ connection }: IMasterAudioProps): React.ReactElement<IMasterAudioProps> => {

    return <div>
        <h3>dB</h3>
        <div>
            <PeakMeter vertical={true} connection={connection?.connection} height={400} width={80} />
        </div>
    </div>
}

export default MasterAudioMeter;