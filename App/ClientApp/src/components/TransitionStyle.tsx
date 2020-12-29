import * as React from "react";

export default function TransitionStyle(): JSX.Element {
    return <section className="transition-style">
        <h3>Transition style</h3>
        <div className="well">
            <div className="button yellow" onClick={c => { alert("Hello"); }}>
                <p>MIX</p>
            </div>
            <div className="button yellow" onClick={c => { alert("Hello"); }}>
                <p>DIP</p>
            </div>
            <div className="button" onClick={c => { alert("Hello"); }}>
                <p>WIPE</p>
            </div>

            <div className="button yellow" onClick={c => { alert("Hello"); }}>
                <p>STING</p>
            </div>
            <div className="button" onClick={c => { alert("Hello"); }}>
                <p>DVE</p>
            </div>
            <div className="button">
                <p>PREV<br />TRAN</p>
            </div>
        </div>
    </section>;
}
