import * as React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Audio from './components/Audio';
import Setup from './components/Setup';
import Switcher from './components/Switcher';
import TallyLight from './components/TallyLight';
import ReactPlayer from 'react-player';

import "./global.css";
import "./custom.css";

interface IAppState {
    livestreamUrl: string
    livestreamEnabled: boolean
}

const App = (): JSX.Element => {

    const [state, setState] = React.useState<IAppState>({ livestreamUrl: "", livestreamEnabled: false });

    const handleLivestreamUrlChange = (value: string) => {
        setState({ livestreamUrl: value, livestreamEnabled: state.livestreamEnabled });
    }

    const handleLivestreamEnabledChange = (value: boolean) => {
        setState({ livestreamUrl: state.livestreamUrl, livestreamEnabled: value });
    }

    return (
        <Layout>
            {state.livestreamEnabled &&
                <section className='video-player'>
                    <h3>Livestream Preview (Delayed)</h3>
                    <div className="well">
                        <ReactPlayer url={state.livestreamUrl} muted={true} playing={true} controls={true} width='400px' height='225px' />
                    </div>
                </section>
            }
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
                <Setup livestreamUrl={state.livestreamUrl}
                    liveStreamEnabled={state.livestreamEnabled}
                    onLivestreamUrlChange={handleLivestreamUrlChange}
                    onLivestreamEnabledChange={handleLivestreamEnabledChange} />
            </Route>
        </Layout>
    );

};

export default App;