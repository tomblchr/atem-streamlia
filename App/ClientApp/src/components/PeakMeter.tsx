import * as React from "react";
import * as CSS from "csstype";
import { Framer, SkipBack } from "react-feather";
import { HubConnection, HubConnectionState } from "@microsoft/signalr";

export interface IPeakMeterProps {
    vertical: boolean;
    height: number;
    width: number;
    connection: HubConnection | null;
}

interface IPeakMetersState {
    inputId: number;
    sourceId: number;
    levels: number[];
    peaks: number[];
}

interface IIndexedPeakMetersState {
    [key: number]: IPeakMetersState;
}

// inspired  by https://css-tricks.com/using-requestanimationframe-with-react-hooks/

const PeakMeter = ({ vertical, connection, height, width }: IPeakMeterProps): React.ReactElement<IPeakMeterProps> => {

    const inputs: number[] = [0, 1, 2, 3, 4, 1301, 1302];
    const startVolume: IPeakMetersState = { inputId: 0, sourceId: 0, levels: [-30, -30], peaks: [0, 0] };

    const init: IIndexedPeakMetersState = {
        0: { inputId: 0, sourceId: 0, levels: [-120, -120], peaks: [0, 0] },
        1: { inputId: 1, sourceId: 0, levels: [-120, -120], peaks: [0, 0] },
        2: { inputId: 2, sourceId: 0, levels: [-120, -120], peaks: [0, 0] },
        3: { inputId: 3, sourceId: 0, levels: [-120, -120], peaks: [0, 0] },
        4: { inputId: 4, sourceId: 0, levels: [-120, -120], peaks: [0, 0] },
        1301: { inputId: 1301, sourceId: 0, levels: [-120, -120], peaks: [0, 0] },
        1302: { inputId: 1302, sourceId: 0, levels: [-120, -120], peaks: [0, 0] }
    };

    var [state, setState] = React.useState<IIndexedPeakMetersState>(init);
    var next = React.useRef<IIndexedPeakMetersState>(init);
    var isMounted = React.useRef<boolean>(false);

    React.useEffect(() => {
        isMounted.current = true;
        if (connection) {
            console.log("Audio connection established");
            setTimeout(() => connection
                .stream("ReceiveVolumeChange")
                .subscribe({
                    next: (message) => {
                        volumeMessageHandler(message);
                    },
                    complete: () => {
                        console.log("Volume stream completed");
                    },
                    error: (err) => {
                        console.error(err);
                    }
                }), 3000);
        } else {
            console.log("No audio connection to ATEM");
        }

        return () => {
            isMounted.current = false;
        }

    }, [connection]);

    const volumeMessageHandler = (message: IPeakMetersState): void => {
        if (isMounted.current) {
            try {
                const current: IIndexedPeakMetersState = { ...next.current };
                current[message.inputId] = message;
                next.current = current;
                requestAnimationFrame(updateMeter);
            }
            catch (e) {
                console.error(e);
            }
        }
    };

    // volume can be received at hundreds of messages per second
    // only update the screen at the animation rate of the browser
    const updateMeter: FrameRequestCallback = (time: number): void => {
        if (isMounted.current) {
            setState(next.current);
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
        {inputs.map((s, indexOuter) => {
            return <React.Fragment key={`rf${indexOuter}`}>
                {state[s].levels.map((c, index) => {
                    return <div key={`br${indexOuter}${index}`}>
                        <div key={`db${indexOuter}${index}`}
                            className="audio-level-value">{Math.round(state[s].peaks[index])}</div>
                        <div key={`lv${indexOuter}${index}`}
                            className="audio-level"
                            style={{ clipPath: "inset(" + dBFSToY(c) + "px 2px 0 0)" }}></div>
                    </div>
                })}
            </React.Fragment>
        })}
    </div>
  };
  
  export default PeakMeter;