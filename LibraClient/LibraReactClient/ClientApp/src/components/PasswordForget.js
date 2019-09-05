import React, { Component } from 'react';
import { Link } from 'react-router-dom';

import { auth } from '../firebase';
import * as routes from '../constants/routes';
import './PasswordForget.css';


const PasswordForget = () =>
    <div>
        <PasswordForgetForm />
    </div>

const byPropKey = (propertyName, value) => () => ({
    [propertyName]: value
});

const INITIAL_STATE = {
    email: '',
    error: null,
    success: null
}

class PasswordForgetForm extends Component {
    constructor(props) {
        super(props);

        this.state = { ...INITIAL_STATE }
    }

    onSubmit = (event) => {
        const { email } = this.state;

        auth.doPasswordReset(email)
            .then(() => {
                this.setState({ ...INITIAL_STATE });
                this.setState(byPropKey('success', { message: 'Password reset successfull' }));
            }).catch((error) => {
                this.setState(byPropKey('error', error));
            });

        event.preventDefault();
    }

    render() {
        const {
            email,
            error,
            success
        } = this.state;

        const isInvalid = email === '';

        return (
            <div className="wrapper fadeInDown">
                <div id="formContent">
                    <div className="fadeIn first">
                        <img src={require("../images/1950labs.png")} id="icon" alt="Logo" />
                        <div className="PoweredBy">Powered By </div><img src={require("../images/LibraLogo.png")} id="iconLibra" alt="LogoLibra" />
                    </div>
                    <h4>Password forget</h4>
                    <form onSubmit={this.onSubmit}>
                        <input
                            value={this.state.email}
                            onChange={event => this.setState(byPropKey('email', event.target.value))}
                            type="text"
                            className="fadeIn first inputText"
                            placeholder="Email Address"
                        />

                        <input disabled={isInvalid} className="fadeIn second inputButton " type="submit" value="Reset password">
                        </input>

                        {error && <p className="ErrorMessage">{error.message}</p>}
                        {success && <p className="SuccessMessage">{success.message}</p>}
                    </form>
                </div>
            </div>
        );
    }
}

const PasswordForgetLink = () =>
    <p>
        <Link to={routes.PASSWORD_FORGET} className="PasswordForgetLinkColor">Forgot password</Link>
    </p>

export default PasswordForget;

export { PasswordForgetForm, PasswordForgetLink }