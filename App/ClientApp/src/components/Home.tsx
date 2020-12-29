import React, { Component } from "react";
import Switcher from "./Switcher";

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
        <Switcher />
    );
  }
}
