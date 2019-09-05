import React, { Component } from 'react';
import { Route } from 'react-router';
import Home from './components/Home';
import NewTransaction from './components/NewTransaction';
import Accounts from './components/Accounts';
import SignIn from './components/SignIn';
import SignUp from './components/SignUp';
import PasswordForget from './components/PasswordForget';
import * as routes from './constants/routes'
import { Layout } from './components/Layout';
import withAuthentication from './components/WithAuthentication';
import AuthUserContext from './components/AuthUserContext'

class App extends Component {
  displayName = App.name


  render() {
      return (
          <AuthUserContext.Consumer>
              {
                  authUser => !!authUser ?
                      <Layout>
                          <Route
                              exact path={routes.LANDING}
                              component={Home}
                          />

                          <Route
                              path={routes.HOME}
                              component={Home}
                          />

                          <Route
                              path={routes.ACCOUNTS}
                              component={Accounts}
                          />

                          <Route
                              path={routes.NEW_TRANSACTION}
                              component={NewTransaction}
                          />
                          
                      </Layout>
                      : 
                      <div>
                          <Route
                              exact path={routes.LANDING}
                              component={Home}
                          />

                          <Route
                              path={routes.HOME}
                              component={Home}
                          />

                          <Route
                              path={routes.ACCOUNTS}
                              component={Accounts}
                          />

                          <Route
                              path={routes.NEW_TRANSACTION}
                              component={NewTransaction}
                          />
                          <Route
                              exact path={routes.SIGN_IN}
                              component={SignIn}
                          />

                          <Route
                              exact path={routes.SIGN_UP}
                              component={SignUp}
                          />

                          <Route
                              exact path={routes.PASSWORD_FORGET}
                              component={PasswordForget}
                          />
                        
                      </div>
              }
          </AuthUserContext.Consumer>
    );
  }
}

export default withAuthentication(App)

