import React, { Component } from 'react';
import './TransactionResult.css';
import withAuthorization from './WithAuthorization';
import queryString from 'query-string';

class TransactionResult extends Component {
    displayName = TransactionResult.name

    constructor(props) {
        super(props);

        let url = this.props.location.search;
        let params = queryString.parse(url);

        this.state = {
            sourceAccountId: params.sourceAccount,
            sequenceNumber: params.sequenceNumber
        };
    }
   



    render() {
        return (
            <div>
                HOLA
            </div>
        );
    }
}

const authCondition = (authUser) => !!authUser

export default withAuthorization(authCondition)(TransactionResult);

