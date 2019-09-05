import React from 'react'
import AuthUserContext from './AuthUserContext'
import { firebase } from '../firebase';


const withAuthentication = (Component) => {
    class WithAuthentication extends React.Component {

        constructor(props) {
            super(props);

            this.state = {
                authUser: null,
                loading: false
            }
        }

        componentDidMount() {
            this.setState({ loading: true });
            firebase.auth.onAuthStateChanged(authUser => {
                this.setState({ loading: false });
                authUser ? this.setState({ authUser }) : this.setState({ authUser: null })
            });
        }

        render() {
            const { authUser } = this.state;

            return (
                <AuthUserContext.Provider value={authUser}>
                    {!this.state.loading ? <Component {...this.props} authUser={authUser} /> : null }
                </AuthUserContext.Provider>
            );
        }
    }

    return WithAuthentication;
}

export default withAuthentication;