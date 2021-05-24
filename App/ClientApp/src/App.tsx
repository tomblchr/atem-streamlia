import * as React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Audio from './components/Audio';
import Setup from './components/Setup';
import Switcher from './components/Switcher';
import TallyLight from './components/TallyLight';

import "./global.css";
import "./custom.css";

const App = (): JSX.Element => {

    return (
        <Layout>
            <Route exact path='/'>
                <Switcher />
            </Route>
            <Route path='/audio'>
                <Audio />
            </Route>
            <Route path='/tally-light'>
                <TallyLight />
            </Route>
            <Route path='/setup'>
                <Setup />
            </Route>
        </Layout>
    );

};

export default App;