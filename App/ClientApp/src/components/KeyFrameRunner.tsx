import * as React from "react";
import { HubConnection } from "@microsoft/signalr";

enum FlyKeyFrame {
    None = 0,
    FrameFull = 1,
    FrameInfinityCentreOfKey = 2,
    FrameInfinityTopLeft = 4,
    FrameInfinityTop = 8,
    FrameInfinityTopRight = 16,
    FrameInfinityLeft = 32,
    FrameInfinityCentre = 64,
    FrameInfinityRight = 128,
    FrameInfinityBottomLeft = 256,
    FrameInfinityBottom = 512,
    FrameInfinityBottomRight = 1024,
    FrameA = 2048,
    FrameB = 4096
}

export interface IKeyFrameRunnerProps {
    connection: HubConnection | undefined;
}

interface IKeyFrameRunnerState {
    isRunning: boolean;
    destination: FlyKeyFrame;
}

const KeyFrameRunner = ({ connection }: IKeyFrameRunnerProps): React.ReactElement => {

    const [state, setState] = React.useState<IKeyFrameRunnerState>({ isRunning: false, destination: 0 });

    React.useEffect(() => {
        
        connection?.on("ReceiveKeyFlyParameters", message => {
            setState(message);
        });

        return () => {
            // clean-up
            connection?.off("ReceiveKeyFlyParameters");
        }

    }, [connection]);

    const sendRunKeyFrame = async (keyFrame: FlyKeyFrame): Promise<void> => {
        await connection?.send("SendRunKeyFrame", keyFrame)
            .then(() => { console.log(`SendRunKeyFrame ${keyFrame}`) })
            .catch(e => console.log("SendRunKeyFrame failed: ", e));
    }

    return <section className="keyframerunner">
        <h3>Key Frame Runner</h3>
        <div className="well">
            <div style={{ display: "flex", flexDirection: "row" }}>
                <div className={state.isRunning && state.destination === FlyKeyFrame.FrameA ? "button red" : "button"} onClick={c => sendRunKeyFrame(FlyKeyFrame.FrameA)}>
                    <p>A</p>
                </div>
                <div className={state.isRunning && state.destination === FlyKeyFrame.FrameB ? "button red" : "button"} onClick={c => sendRunKeyFrame(FlyKeyFrame.FrameB)}>
                    <p>B</p>
                </div>
                <div className={state.isRunning && state.destination === FlyKeyFrame.FrameFull ? "button red" : "button"} onClick={c => sendRunKeyFrame(FlyKeyFrame.FrameFull)}>
                    <p>Full</p>
                </div>
                <div className={state.isRunning && state.destination === FlyKeyFrame.FrameInfinityCentreOfKey ? "button red" : "button"} onClick={c => sendRunKeyFrame(FlyKeyFrame.FrameInfinityCentreOfKey)}>
                    <p>&#8734;</p>
                </div>
            </div>
        </div>
    </section>
}

export default KeyFrameRunner;