import * as React from "react";

export default function FadeToBlack(): JSX.Element {
    return <section className="fade-to-black">
        <h3>Fade to Black</h3>
        <div className="well">
            <div className="button" onClick={c => { alert("Hello"); }}>
                <p>FTB</p>
            </div>
        </div>
    </section>;
}