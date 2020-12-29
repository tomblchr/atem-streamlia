import * as React from "react";

export default function NextTransition(): JSX.Element {
    return <section className="next-transition">
        <h3>Next Transition</h3>
        <div className="well">
            <div className="button yellow" onClick={c => { alert("Hello"); }}>
                <p>BKGD</p>
            </div>
            <div className="button red" onClick={c => { alert("Hello"); }}>
                <p>ON AIR</p>
            </div>
            <div className="button yellow" onClick={c => { alert("Hello"); }}>
                <p>Key 1</p>
            </div>
        </div>
    </section>
}
