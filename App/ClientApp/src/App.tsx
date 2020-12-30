import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import TallyLight from './components/TallyLight';

import "./style.css";
import "./global.css";
import "./custom.css";

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/tally-light' component={TallyLight} />
      </Layout>
    );
  }
}
