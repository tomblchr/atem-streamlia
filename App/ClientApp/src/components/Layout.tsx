import React, { Component } from "react";
import { Container } from "reactstrap";
import HeaderBanner from "./HeaderBanner";

export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <React.Fragment>
        <HeaderBanner />
        <Container>
          {this.props.children}
        </Container>
      </React.Fragment>
    );
  }
}
