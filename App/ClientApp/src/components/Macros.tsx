import * as React from "react";
import { HubConnection } from "@microsoft/signalr";
import * as Log from "../api/log";

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
    
    const initial: IMacroState = { macros: [{ id: 0, name: "Something", description: "This is what something does" },
                                            { id: 1, name: "Something More", description: "This will show slides on the screen"},
                                            { id: 2, name: "Something Else", description: "How do we wrap this so it is not wider than the button?"}]};

    const [state, setState] = React.useState<IMacroState>(initial);

    React.useEffect(() => {
        
        connection?.on("ReceiveMacros", message => {
            Log.debug(`ReceiveMacros - ${message}`);
            setState({ macros: message });
        });
        
        return () => {
            connection?.off("ReceiveMacros");
        }

    }, [connection]);

    const sendRunMacro = async (id: number): Promise<void> => {
        await connection?.send("SendRunMacro", id)
            .then(() => { Log.debug(`SendRunMacro ${id}`) })
            .catch(e => Log.error("SendRunMacro failed: ", e));
    }

    return <section className="macro">
        <h3>Macros</h3>
        <div className="well">
            {state.macros.map((c, i) => (
            <div title={`${c.description}`} key={`t-${c.id}`}>
                <div key={c.id}
                    className="button"
                    onClick={_ => sendRunMacro(c.id)}>
                    <p>{c.id}</p>
                </div>
                <div className="button-text">{c.name}</div>
            </div>
            ))}
        </div>
    </section>
}

export default Macros;