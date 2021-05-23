import * as React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import Audio from './components/Audio';
import Setup from './components/Setup';
import TallyLight from './components/TallyLight';
import ServerHubConnection from './components/ServerHubConnection';

import "./global.css";
import "./custom.css";
import Switcher from './components/Switcher';

const App = (): JSX.Element => {

    const [server, setServer] = React.useState<ServerHubConnection | null>(null);

    React.useEffect(() => {
        console.log("Creating signalr connection...");
        const newConnection: ServerHubConnection = server ?? new ServerHubConnection();
        setServer(newConnection);
    }, []);


    return (
        <Layout>
            <Route exact path='/'>
                <Switcher server={server} />
            </Route>
            <Route path='/audio'>
                <Audio server={server} />
            </Route>
            <Route path='/tally-light'>
                <TallyLight server={server} />
            </Route>
            <Route path='/setup'>
                <Setup server={server} />
            </Route>
        </Layout>
    );

};

export default App;