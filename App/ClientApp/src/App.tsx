import * as React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import ReactPlayer from 'react-player';

import Audio from './components/Audio';
import Setup from './components/Setup';
import Switcher from './components/Switcher';
import TallyLight from './components/TallyLight';

import ServerHubConnection from './components/ServerHubConnection';

import "./global.css";
import "./custom.css";
import ConnectionMonitor from './components/ConnectionMonitor';

interface IAppState {
    livestreamUrl: string;
    livestreamEnabled: boolean;
    hostAgentNetworkLocation: string;
    server: ServerHubConnection | undefined;
}

const App = (): JSX.Element => {

    const [state, setState] = React.useState<IAppState>({ livestreamUrl: "", livestreamEnabled: false, server: undefined, hostAgentNetworkLocation: "10.0.0.171" });

    React.useEffect(() => {
        
        const newConnection = new ServerHubConnection(state.hostAgentNetworkLocation);

        setState({ ...state, server: newConnection });

        return () => {
            // clean up
            newConnection.connection.stop();
        };

    }, [state.hostAgentNetworkLocation]);

    const handleLivestreamUrlChange = (value: string) => {
        setState({...state, livestreamUrl: value });
    }

    const handleLivestreamEnabledChange = (value: boolean) => {
        setState({ ...state, livestreamEnabled: value });
    }

    const handleHostAgentNetworkLocationChange = (value: string) => {
        setState({...state, hostAgentNetworkLocation: value});
    }

    return (
        <Layout>
            <ConnectionMonitor connection={state.server?.connection} />
            {state.livestreamEnabled &&
                <section className='video-player'>
                    <h3>Livestream Preview (Delayed)</h3>
                    <div className="well">
                        <ReactPlayer url={state.livestreamUrl} muted={true} playing={true} controls={true} width='400px' height='225px' />
                    </div>
                </section>
            }
            <Route exact path='/'>
                <Switcher server={state.server} onLivestreamUrlChange={handleLivestreamUrlChange} />
            </Route>
            <Route path='/audio'>
                <Audio server={state.server} />
            </Route>
            <Route path='/tally-light'>
                <TallyLight server={state.server} />
            </Route>
            <Route path='/setup'>
                <Setup server={state.server}
                    livestreamUrl={state.livestreamUrl}
                    liveStreamEnabled={state.livestreamEnabled}
                    onLivestreamUrlChange={handleLivestreamUrlChange}
                    onLivestreamEnabledChange={handleLivestreamEnabledChange}
                    onHostAgentNetworkLocationChange={handleHostAgentNetworkLocationChange} />
            </Route>
        </Layout>
    );

};

export default App;