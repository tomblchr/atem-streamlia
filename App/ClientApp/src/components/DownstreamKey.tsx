import * as React from "react";

export default function DownstreamKey(): JSX.Element {
    return <section className="downstream-key">
        <h3>Downstream Key 1</h3>
        <div className="well">
            <div
                className="button" onClick={c => { alert("Hello"); }}>
                <p>TIE</p>
            </div>
            <div
                className="button red">
                <p>ON AIR</p>
            </div>
            <div className="button" onClick={c => { alert("Hello"); }}>
                <p>AUTO</p>
            </div>
        </div>
    </section>;
}
