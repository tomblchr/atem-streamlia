import * as React from "react";

export interface IInput {
    id: number;
    name: string;
}

export interface IInputsProps {
    inputs: IInput[] | undefined;
    program: number | undefined;
    preview: number | undefined;
}

export default function Inputs(props: IInputsProps): JSX.Element {

    return <section className="channels">
        <h3>Program & Preview</h3>
        <div className="well">
            {props.inputs?.map(i => (
                <div key={i.id} className={props.program === i.id ? "button red" : props.preview === i.id ? "button green" : "button"}>
                    <p>{i.id}</p>
                </div>
            ))};
        </div>
    </section>;
}
