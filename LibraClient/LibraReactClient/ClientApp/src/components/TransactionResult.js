import React, { Component } from 'react';
import './TransactionResult.css';
import { ListGroup, ListGroupItem } from 'react-bootstrap';
import withAuthorization from './WithAuthorization';

class TransactionResult extends Component {
    displayName = TransactionResult.name

    constructor(props) {
        super(props);
        this.state = {
            ...props.location.state,
            transactionMessage: undefined,
            transactionIcon: undefined,
            transactionDate:undefined
        };
    }

    componentDidMount() {
        if (this.state.transactionStatus == 'success') {

            const txDate = new Date(this.state.transactionResult.transaction.expirationTime)

            this.setState({
                transactionMessage: 'Transaction successfull',
                transactionIcon: 'success_icon.svg',
                transactionDate: txDate
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
                            <div className={this.state.transactionStatus === 'success' ? 'transactionMessageSuccess' : 'transactionMessageError'}>{this.state.transactionMessage} </div>
                            
                            <div className="MessageContainerTx">
                                <div className="TestnetContainerTx">
                                    <p className="TestnetTextTx">Testnet</p>
                                </div>
                            </div>
                            <div className="informationMessgae">This is a Testnet transaction that has no actual value</div>
                            <ListGroup>
                                <ListGroupItem header="Amount">{this.state.transactionResult.amount}  <img className="libraIconTR" src={require("../images/libra_icon.svg")} alt="logo" /></ListGroupItem>
                                <ListGroupItem header="Source account">{this.state.transactionResult.sourceHex}</ListGroupItem>
                                <ListGroupItem header="Recipient account">{this.state.transactionResult.recipientHex}</ListGroupItem>
                                {
                                        this.state.transactionStatus == 'success' ? 
                                        <div>
                                            <ListGroupItem header="Version ID">{this.state.transactionResult.transaction.versionId}</ListGroupItem>
                                            <ListGroupItem header="Sequence number">{this.state.transactionResult.transaction.sequenceNumber}</ListGroupItem>
                                            <ListGroupItem header="Max gas amount">{this.state.transactionResult.transaction.maxGasAmount / 1000000} <img className="libraIconTR" src={require("../images/libra_icon.svg")} alt="logo" /></ListGroupItem>
                                            <ListGroupItem header="Gas unit price">{this.state.transactionResult.transaction.gasUnitPrice / 1000000} <img className="libraIconTR" src={require("../images/libra_icon.svg")} alt="logo" /></ListGroupItem>
                                            <ListGroupItem header="Expiration date">{this.state.transactionDate.toString() } </ListGroupItem>
                                            <ListGroupItem header="Transaction type">{this.state.transactionResult.transaction.payloadCase == 2 ? 'Peer to peer TX' : 'Mint TX' } </ListGroupItem>
                                        </div>
                                        : null
                                }
                               
                            </ListGroup>

                           
                        </div>
                        : null
                 }
                </div>
        );
    }
}

const authCondition = (authUser) => !!authUser

export default withAuthorization(authCondition)(TransactionResult);

