import * as React from "react";
import * as CSS from "csstype";
import { SkipBack } from "react-feather";
import { HubConnection } from "@microsoft/signalr";

export interface IPeakMeterProps {
    vertical: boolean;
    height: number;
    width: number;
    connection: HubConnection | null;
}

interface IPeakMetersState {
    levels: number[];
    peaks: number[];
}

// inspired  by https://css-tricks.com/using-requestanimationframe-with-react-hooks/

const PeakMeter = ({ vertical, connection, height, width }: IPeakMeterProps): React.ReactElement<IPeakMeterProps> => {

    const init: IPeakMetersState = { levels: [0, 0], peaks: [0, 0] };

    var [state, setState] = React.useState<IPeakMetersState>(init);
    var volume = React.useRef<IPeakMetersState>(init);

    React.useEffect(() => {
        if (connection) {
            console.log("Audio connection established");
            connection.on("ReceiveVolume", volumeMessageHandler);
        } else {
            console.log("No audio connection to ATEM");
        }
    }, [connection]);

    const volumeMessageHandler = (message: IPeakMetersState): void => {
        try {
            volume.current = message;
            requestAnimationFrame(updateMeter);
        }
        catch (e) {
            console.error(e);
        }
    };

    const updateMeter: FrameRequestCallback = (time: number) => {
        setState(volume.current);
        requestAnimationFrame(updateMeter);
    };

    const dBFSToY = (db: number): number => {
        //db = -2;

        // db is between -Infinity and 0dB
        if (db < -60) return 300;
        if (db >= -1) return 0;

        const height = 300;

        //const y = height * Math.log(Math.abs(db)) / Math.log(60);

        const y = 300 * (Math.abs(db) / 60);

        return Math.floor(y);
    };

    return <div className="audio-levels">
        {state.levels.map((c, index) => {
            return <div key={index}
                className="audio-level"
                style={{ clipPath: "inset(" + dBFSToY(c) + "px 2px 0 0)" }}></div>
        })}
    </div>
    
  };
  
  export default PeakMeter;