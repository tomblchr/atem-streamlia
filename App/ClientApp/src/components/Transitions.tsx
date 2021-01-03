import * as React from "react";
import { apiTransition } from "../api/atemconnection";

export default function Transitions(): JSX.Element {
    return <section className="transition">
        <h3>Transition</h3>
        <div className="well">
            <div className="button" onClick={c => { alert("Hello"); }}>
                <p>CUT</p>
            </div>
            <div className="button red" onClick={c => { apiTransition(); }}>
                <p>AUTO</p>
            </div>
            <div>
                <input className="slider" type="range" min="0" max="1" step="0.001" />
            </div>
        </div>
    </section>
}
