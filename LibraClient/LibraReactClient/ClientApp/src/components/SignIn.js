import React, {Component} from 'react';
import { withRouter } from 'react-router-dom';
import { auth } from '../firebase';
import { SignUpLink } from './SignUp';
import { PasswordForgetLink } from './PasswordForget';
import * as routes from '../constants/routes';
import './SignIn.css';



const SignIn = ({history}) =>
  <div>
        <SignInForm history={history} />
       
  </div>


const byPropKey = (propertyName, value) => () => ({
    [propertyName]: value
});

const INITIAL_STATE = {
    email: '',
    password: '',
    error: null
}

class SignInForm extends Component {
    constructor(props){
        super(props);
    
        this.state = { ...INITIAL_STATE };
    }

    onSubmit = (event) => {
        const {
            email,
            password
        } = this.state;
        
        const {
            history
        } = this.props;

        auth.doSignInWithEmailAndPassword(email, password)
            .then(() => {
                this.setState({ ...INITIAL_STATE });
                history.push(routes.HOME);
            }).catch(error => {
                this.setState(byPropKey('error', error));
            });
        
        event.preventDefault();
    }

    render(){
        
        const {
            email,
            password,
            error
        } = this.state;

        const isInvalid = password === '' || email === '';

        return (
            <div className="wrapper fadeInDown">
                <div id="formContent">
                    <div className="fadeIn first">
                        <img src={require("../images/1950labs.png")} id="icon" alt="Logo" />
                        <div className="PoweredBy">Powered By </div><img src={require("../images/LibraLogo.png")} id="iconLibra" alt="LogoLibra" /> 
                    </div>
                        <form onSubmit={this.onSubmit}>
                            <input
                                type="text"
                                value={email}
                                className="fadeIn second inputText"
                                onChange={event => this.setState(byPropKey('email', event.target.value))}
                                placeholder="Email address" />
            
                            <input
                                type="password"
                                value={password}
                                className="fadeIn third inputText"
                                onChange={event => this.setState(byPropKey('password', event.target.value))}
                                placeholder="Password" />

                            <input disabled={isInvalid} className="fadeIn fourth inputButton" type="submit" value="Sign in">
                            </input>

                        {error && <p className="ErrorMessage">{error.message}</p>}
                    </form>
                    <div id="formFooter">
                        <div className="SignUpPasswordForgetLinks">
                            <SignUpLink />&nbsp;|&nbsp;<PasswordForgetLink />
                        </div>
                    </div>
                </div>
                
            </div>
        
        )
    }
}

export default withRouter(SignIn);

export { SignInForm }; 