import React, { Component } from "react";
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import { AlertTriangle, Zap } from "react-feather";

interface INavMenuState {
    collapsed: boolean;
}

export default function HeaderBanner(): JSX.Element {
    const [state, setState] = React.useState<INavMenuState>({ collapsed: false });

    const toggleNavbar = (): void => { setState({ collapsed: !state.collapsed }) };

/*
 * svelte original
<header>
  <h1>{atem.state._pin}</h1>
  <a href="#switcher" class="tab"><Feather icon="grid"/>Switcher</a>
  <a href="#media" class="tab"><Feather icon="film"/>Media</a>
  <a href="#macros" class="tab"><Feather icon="box"/>Macros</a>
  <span class="tab connection-status" class:connected={ws.readyState === 1}
        title="Connection status: green=connected, red=disconnected">
    {#if ws.readyState === 1}<Feather icon="zap"/>{:else}<Feather icon="alert-triangle"/>{/if}
    Server
  </span>
  <span class="tab connection-status" class:connected={atem.connected}
        title="Connection status: green=connected, red=disconnected">
    {#if atem.connected}<Feather icon="zap"/>{:else}<Feather icon="alert-triangle"/>{/if}
    ATEM
  </span>
</header>
 */

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" dark>
                <Container>
                    <NavbarBrand tag={Link} to="/">SwitcherServer</NavbarBrand>
                    <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!state.collapsed} navbar>
                    <ul className="navbar-nav flex-grow">
                        <NavItem>
                            <span className="tab connection-status connected" title="Connection status: green=connected, red=disconnected">
                                <Zap />
                            </span>
                        </NavItem>
                        <NavItem>
                            <span className="tab connection-status" title="Connection status: green=connected, red=disconnected">
                                <AlertTriangle />
                            </span>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} className="tab text-light" to="/">Home</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} className="tab text-light" to="/tally-light">Tally Light</NavLink>
                        </NavItem>
                    </ul>
                    </Collapse>
                </Container>
            </Navbar>            
        </header>
    )
}
