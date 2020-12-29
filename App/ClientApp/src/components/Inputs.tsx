import * as React from "react";

export interface IInput {
    id: number;
    name: string;
    isProgram: boolean;
    isPreview: boolean;
}

export default function Inputs(): JSX.Element {
    const inputs: IInput[] = [
        { id: 1, name: "Channel 1", isProgram: true, isPreview: false },
        { id: 2, name: "Channel 2", isProgram: false, isPreview: false },
        { id: 3, name: "Channel 3", isProgram: false, isPreview: true },
        { id: 4, name: "Channel 4", isProgram: false, isPreview: false }
    ];
    return <section className="channels">
        <h3>Program & Preview</h3>
        <div className="well">
            {inputs.map(i => (
                <div key={i.id} className={i.isProgram ? "button red" : i.isPreview ? "button green" : "button"}>
                    <p>{i.id}</p>
                </div>
            ))};
        </div>
    </section>;
}
