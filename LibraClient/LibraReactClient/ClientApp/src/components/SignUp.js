import React, { Component } from 'react';
import { Link, withRouter } from 'react-router-dom';
import { auth } from '../firebase';
import * as routes from '../constants/routes';
import './SignUp.css';

const SignUp = ({ history }) =>
    <div>
        <SignUpForm history={history} />
    </div>


const INITIAL_STATE = {
    username: '',
    email: '',
    passwordOne: '',
    passwordTwo: '',
    error: null,
};

const byPropKey = (propertyName, value) => () => ({
    [propertyName]: value,
})

class SignUpForm extends Component {
    constructor(props) {
        super(props);

        this.state = { ...INITIAL_STATE }
    }

    onSubmit = (event) => {
        const {
            email,
            passwordOne,
        } = this.state;

        const {
            history
        } = this.props;

        auth.doCreateUserWithEmailAndPassword(email, passwordOne)
            .then(authUser => {
                this.setState({ ...INITIAL_STATE });
                history.push(routes.HOME);
            }).catch(error => {
                this.setState(byPropKey('error', error));
            })

        event.preventDefault();
    }

    render() {
        const {
            email,
            passwordOne,
            passwordTwo,
            error
        } = this.state;

        const isInvalid =
            passwordOne !== passwordTwo ||
            passwordOne === '' ||
            email === '';

        return (
            <div className="wrapper fadeInDown">
                <div id="formContent">
                    <div className="fadeIn first">
                        <img src={require("../images/1950labs.png")} id="icon" alt="Logo" />
                        <div className="PoweredBy">Powered By </div><img src={require("../images/LibraLogo.png")} id="iconLibra" alt="LogoLibra" />
                    </div>
                    <h4>Create account</h4>
                    <form onSubmit={this.onSubmit}>
                        <input
                            value={email}
                            onChange={event => this.setState(byPropKey('email', event.target.value))}
                            type="text"
                            className="fadeIn first inputText"
                            placeholder="Email address"
                        />
                        <input
                            value={passwordOne}
                            onChange={event => this.setState(byPropKey('passwordOne', event.target.value))}
                            type="password"
                            className="fadeIn second inputText"
                            placeholder="Password"
                        />
                        <input
                            value={passwordTwo}
                            onChange={event => this.setState(byPropKey('passwordTwo', event.target.value))}
                            type="password"
                            className="fadeIn third inputText"
                            placeholder="Confirm password"
                        />

                        <input disabled={isInvalid} className="fadeIn fourth inputButton " type="submit" value="Sign up">
                        </input>

                        {error && <p className="ErrorMessage">{error.message}</p>}
                    </form>
                </div>
            </div>
        )
    }
}

const SignUpLink = () =>
        <p>
            Don't have an account?
            {' '}
        <Link to={routes.SIGN_UP} className="SignUpLinkColor">Sign up</Link>
        </p>

export default withRouter(SignUp);

export {
    SignUpForm,
    SignUpLink
}