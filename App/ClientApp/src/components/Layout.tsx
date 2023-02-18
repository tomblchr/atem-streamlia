import React, { PropsWithChildren, Component } from "react";
import { Container } from "reactstrap";
import HeaderBanner from "./HeaderBanner";

const Layout = ({children}: PropsWithChildren): JSX.Element => {

    return <React.Fragment>
        <HeaderBanner />
        <Container>
          {children}
        </Container>
      </React.Fragment>
};

export default Layout;