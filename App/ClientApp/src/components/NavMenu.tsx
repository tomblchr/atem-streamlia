import * as React from "react";
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import "./NavMenu.css";

interface INavMenuState {
  collapsed: boolean;
}

class NavMenu extends React.Component<any, INavMenuState> {
  static displayName = NavMenu.name;

  constructor (props: any) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render () {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3 navbar" light>
          <Container>
            <NavbarBrand tag={Link} to="/">SwitcherServer</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/audio">Audio</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/tally-light">Tally</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/setup">Setup</NavLink>
                </NavItem>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }
}

export default NavMenu;