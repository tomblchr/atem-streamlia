import * as React from "react";
import { HubConnection } from "@microsoft/signalr";
import PeakMeter from "./PeakMeter";

export interface IMasterAudioProps {
    connection: HubConnection | null;
}

interface IMasterAudioState {
    levels: number[];
    peaks: number[];
}

const MasterAudioMeter = ({ connection }: IMasterAudioProps): React.ReactElement<IMasterAudioProps> => {

    return <div>
        <h3>dB</h3>
        <div>
            <PeakMeter vertical={true} connection={connection} height={400} width={80} />
        </div>
    </div>
}

export default MasterAudioMeter;