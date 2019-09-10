import React from 'react'
import { withRouter } from 'react-router-dom';

import AuthUserContext from './AuthUserContext';
import { firebase } from '../firebase';
import * as routes from '../constants/routes';



const withAuthorization = (authCondition) => (Component) => {
    class WithAuthorization extends React.Component {

        constructor(props) {
            super(props);
            this.state = {
                token: undefined
            };
        }

        componentDidMount() {
            firebase.auth.onAuthStateChanged(authUser => {
                if (!authCondition(authUser)) {
                    this.props.history.push(routes.SIGN_IN);

                } else {
                    this.setState({ userUid: authUser.uid });
                    authUser.getIdToken().then(function (idToken) {  // 
                        this.setState({ token: idToken });
                    }.bind(this));
                }
            });
        }

        render() {
            return (
                <AuthUserContext.Consumer>
                    {authUser => this.state.token && authUser && authCondition(authUser) ? <Component userUid={this.state.userUid} token={this.state.token} {...this.props} /> : null}
                </AuthUserContext.Consumer>
            )
        }
    }

    return withRouter(WithAuthorization);
}

export default withAuthorization;