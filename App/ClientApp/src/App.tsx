import * as React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import Audio from './components/Audio';
import Setup from './components/Setup';
import TallyLight from './components/TallyLight';

import "./global.css";
import "./custom.css";

export default class App extends React.Component {
  static displayName = App.name;

  render () {
      return (
          <Layout>
              <Route exact path='/' component={Home} />
              <Route path='/audio' component={Audio} />
              <Route path='/tally-light' component={TallyLight} />
              <Route path='/setup' component={Setup} />
          </Layout>
      );
  }
}
