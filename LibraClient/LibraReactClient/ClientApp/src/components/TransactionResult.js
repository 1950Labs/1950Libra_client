import React, { Component } from 'react';
import './TransactionResult.css';
import withAuthorization from './WithAuthorization';

class TransactionResult extends Component {
    displayName = TransactionResult.name

    constructor(props) {
        super(props);
        this.state = {
            ...props.location.state,
            transactionMessage: undefined,
            transactionIcon: undefined,
            transactionSeqNumber: undefined
        };
    }

    componentDidMount() {
        if (this.state.transactionStatus == 'success') {
            this.setState({
                transactionMessage: 'Transaction successfull',
                transactionIcon: 'success_icon.svg',
                transactionSeqNumber: this.state.transaction.transactionResult.sequenceNumber
            });
        } else {
            this.setState({
                transactionMessage: 'Transaction error',
                transactionIcon: 'error_icon.svg',
            });
        }
    }


    render() {
        return (

            <div className="transactionResultContainer">
                {
                    this.state.transactionIcon ?
                        <div className="transactionResult">
                            <img className="transactionIcon" src={require("../images/" + this.state.transactionIcon)} alt="logo" />
                            <div className={this.state.transactionStatus === 'success' ? 'transactionMessageSuccess' : 'transactionMessageError'}>{this.state.transactionMessage}</div>
                            
                            <div className="MessageContainerTx">
                                <div className="TestnetContainerTx">
                                    <p className="TestnetTextTx">Testnet</p>
                                </div>
                            </div>
                            <div className="informationMessgae">This is a Testnet transaction that has no actual value</div>
                            {
                                this.state.transactionSeqNumber ?
                                    <p> Sequence number: {this.state.transactionSeqNumber}</p>
                                    : null
                            }
                        </div>
                        : null
                 }
                </div>
        );
    }
}

const authCondition = (authUser) => !!authUser

export default withAuthorization(authCondition)(TransactionResult);

