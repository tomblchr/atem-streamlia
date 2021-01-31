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

    return <section>
        <div className="well">
            <PeakMeter vertical={true} connection={connection} height={200} width={80} />
        </div>
    </section>
}

export default MasterAudioMeter;