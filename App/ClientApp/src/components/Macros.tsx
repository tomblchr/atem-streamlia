import * as React from "react";
import { HubConnection } from "@microsoft/signalr";

interface IMacroProps {
    connection: HubConnection | null;
}

interface IMacro {
    id: number;
    name: string;
    description: string;
}

interface IMacroState {
    macros: IMacro[];
}

const Macros = ({ connection }: IMacroProps): React.ReactElement => {

    const [state, setState] = React.useState<IMacroState>({ macros: [] });

    React.useEffect(() => {
        if (connection) {
            connection.on("ReceiveMacros", message => {
                console.log(`ReceiveMacros - ${message}`);
                setState({ macros: message });
            });
        }
    });

    return <section className="macro">
        <h3>Macros</h3>
        <div className="well">
            {state.macros.map((c, i) => (
                <div key={c.id}
                    className="button">
                    <p title={`${c.name} - ${c.description}`}>{c.id}</p>
                </div>
            ))}
        </div>
    </section>
}

export default Macros;