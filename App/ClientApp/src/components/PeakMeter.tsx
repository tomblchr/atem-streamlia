import * as React from "react";
import * as CSS from "csstype";
import { SkipBack } from "react-feather";
import { HubConnection, HubConnectionState } from "@microsoft/signalr";

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
    var isMounted = React.useRef<boolean>(false);

    React.useEffect(() => {
        isMounted.current = true;
        if (connection) {
            console.log("Audio connection established");
            connection.on("ReceiveVolume", volumeMessageHandler);
            setTimeout(() => connection.stream("ReceiveVolumeChange")
                    .subscribe({
                        next: (message) => {
                            //console.log("Volume message received");
                            volumeMessageHandler(message);
                        },
                        complete: () => {
                            console.log("Volume stream completed");
                        },
                        error: (err) => {
                            console.error(err);
                        }
                    }), 5000);
        } else {
            console.log("No audio connection to ATEM");
        }

        return () => {
            // cleanup function
            if (connection) {
                connection.off("ReceiveVolume");
            }
            isMounted.current = false;
        }

    }, [connection]);

    const volumeMessageHandler = (message: IPeakMetersState): void => {
        if (isMounted.current) {
            try {
                volume.current = message;
                requestAnimationFrame(updateMeter);
            }
            catch (e) {
                console.error(e);
            }
        }
    };

    const updateMeter: FrameRequestCallback = (time: number): void => {
        if (isMounted.current) {
            setState(volume.current);
            requestAnimationFrame(updateMeter);
        }
    };

    const dBFSToY = (db: number): number => {
        //db = -20;

        // db is between -Infinity and 0dB
        if (db < -60) return 300;
        if (db >= -1) return 0;

        const height = 300;
        const dBthreshold = -60; // lowest visible dB value - below this there is no visual

        // from 0 to -20dB is linear
        // lower than -20dB is logarithmic
        // cutover is at 100px
        const y = db >= -60
            ? height * (Math.abs(db) / Math.abs(dBthreshold))
            : height * Math.log(Math.abs(db)) / Math.log(Math.abs(dBthreshold));        

        return Math.floor(y);
    };

    return <div className="audio-levels">
        {state.levels.map((c, index) => {
            return <div key={index}>
                <div key={index + "db"} className="audio-level-value">{state.peaks[index]}</div>
                <div key={index + "level"}
                    className="audio-level"
                    style={{ clipPath: "inset(" + dBFSToY(c) + "px 2px 0 0)" }}></div>
            </div>
        })}
    </div>
    
  };
  
  export default PeakMeter;