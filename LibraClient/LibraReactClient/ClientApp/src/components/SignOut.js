import React from 'react';
import { auth } from '../firebase'
import { Button, Glyphicon } from 'react-bootstrap';

const SignOutButton = () =>
    <Button bsStyle="danger" onClick={auth.doSignOut}>Log out <Glyphicon glyph='log-out' /></Button>

export default SignOutButton;