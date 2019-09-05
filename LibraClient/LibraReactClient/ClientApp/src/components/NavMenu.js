import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import * as routes from '../constants/routes';
import './NavMenu.css';

export class NavMenu extends Component {
  displayName = NavMenu.name

  render() {
    return (
      <Navbar inverse fixedTop fluid collapseOnSelect>
        <Navbar.Header>
          <Navbar.Brand>
            <Link to={'/'} >
                <img className="logo" src={require("../images/1950labs.png")} alt="logo" />    
            </Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
                <Nav>
                    <LinkContainer to={routes.HOME} exact>
              <NavItem>
                <Glyphicon glyph='home' /> Home
              </NavItem>
                    </LinkContainer>
                    <LinkContainer to={routes.ACCOUNTS}>
                <NavItem>
                    <Glyphicon glyph='piggy-bank' /> Accounts
                </NavItem>
            </LinkContainer>
            <LinkContainer to={routes.NEW_TRANSACTION}>
                <NavItem>
                    <Glyphicon glyph='transfer' /> Create Transaction
                </NavItem>
            </LinkContainer>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}
