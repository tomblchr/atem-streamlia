import * as React from 'react';
import { Routes, Route } from 'react-router';
import Layout from './components/Layout';
import ReactPlayer from 'react-player/youtube';

import Audio from './components/Audio';
import Setup from './components/Setup';
import Switcher from './components/Switcher';
import TallyLight from './components/TallyLight';

import ServerHubConnection from './components/ServerHubConnection';

import "./global.css";
import "./custom.css";
import ConnectionMonitor from './components/ConnectionMonitor';

import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

interface IAppState {
    livestreamUrl: string;
    livestreamEnabled: boolean;
    hostAgentNetworkLocation: string;
    atemNetworkLocation: string;
    showToast?: boolean;
    server: ServerHubConnection | undefined;
}

const App = (): JSX.Element => {

    const hostAgentNetworkLocationDefault = window.localStorage.getItem("hostAgentNetworkLocation") ?? "";
    const atemNetworkLocationDefault = window.localStorage.getItem("atemNetworkLocation") ?? "";

    const [state, setState] = React.useState<IAppState>({ server: undefined, livestreamUrl: "", livestreamEnabled: false, hostAgentNetworkLocation: hostAgentNetworkLocationDefault, atemNetworkLocation: atemNetworkLocationDefault });

    React.useEffect(() => {
        
        const newConnection = new ServerHubConnection(state.hostAgentNetworkLocation);

        setState(s => { return {...s, server: newConnection, showToast: true }});

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
        window.localStorage.setItem("hostAgentNetworkLocation", value);
    }

/*
    const handleAtemNetworkLocationChange = (value: string) => {
        setState({...state, atemNetworkLocation: value});
        window.localStorage.setItem("atemNetworkLocation", value);
    }
*/
    return (
        <Layout>
            <ConnectionMonitor connection={state.server?.connection} />
            <ToastContainer position="bottom-right"
                theme="dark"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={true}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover />
            {state.livestreamEnabled &&
                <section className='video-player'>
                    <h3>Livestream Preview (Delayed)</h3>
                    <div className="well">
                        <ReactPlayer url={state.livestreamUrl} muted={true} playing={true} controls={true} width='400px' height='225px' />
                    </div>
                </section>
            }
            <Routes>
                <Route path='/' element={<Switcher server={state.server} onLivestreamUrlChange={handleLivestreamUrlChange} />} />            
                <Route path='/audio' element={<Audio server={state.server} />} />            
                <Route path='/tally-light' element={<TallyLight server={state.server} />} />
                <Route path='/setup' element={<Setup server={state.server}
                        livestreamUrl={state.livestreamUrl}
                        liveStreamEnabled={state.livestreamEnabled}
                        hostAgentNetworkLocation={state.hostAgentNetworkLocation}
                        onLivestreamUrlChange={handleLivestreamUrlChange}
                        onLivestreamEnabledChange={handleLivestreamEnabledChange}
                        onHostAgentNetworkLocationChange={handleHostAgentNetworkLocationChange} /> } />
            </Routes>
        </Layout>
    );

};

export default App;