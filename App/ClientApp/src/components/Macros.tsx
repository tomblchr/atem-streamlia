import * as React from "react";
import { HubConnection } from "@microsoft/signalr";

interface IMacroProps {
    connection: HubConnection | undefined;
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
        
        connection?.on("ReceiveMacros", message => {
            console.log(`ReceiveMacros - ${message}`);
            setState({ macros: message });
        });
        
        return () => {
            connection?.off("ReceiveMacros");
        }

    }, [connection]);

    const sendRunMacro = async (id: number): Promise<void> => {
        await connection?.send("SendRunMacro", id)
            .then(() => { console.log(`SendRunMacro ${id}`) })
            .catch(e => console.log("SendRunMacro failed: ", e));
    }

    return <section className="macro">
        <h3>Macros</h3>
        <div className="well">
            {state.macros.map((c, i) => (
                <div key={c.id}
                    className="button"
                    onClick={_ => sendRunMacro(c.id)}>
                    <p title={`${c.name} - ${c.description}`}>{c.id}</p>
                </div>
            ))}
        </div>
    </section>
}

export default Macros;