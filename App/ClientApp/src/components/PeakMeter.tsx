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

    return <div style={{ backgroundColor: "black", width: "300px" }}>
        {state.levels.map((c, index) => {
            return <div key={index} style={{ marginTop: "2px", height: "10px", width: (300 - (c * -3)) + "px", backgroundColor: "green" }}> &nbsp; </div>
        })}
    </div>
    
  };
  
  export default PeakMeter;