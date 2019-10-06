import React, { Component } from 'react';
import { Button, Modal, FormGroup, ControlLabel, FormControl, Alert } from 'react-bootstrap';
import withAuthorization from './WithAuthorization';

class TransactionsModal extends Component {
    displayName = TransactionsModal.name

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            transactions: []
        };
    }

   
    componentDidUpdate(prevProps) {
        if (this.props.show && (!prevProps.account || (this.props.account && this.props.account.accountId && prevProps.account.accountId !== this.props.account.accountId))) {
            this.setState({ transactions: [] });

            this.setState({ loading: true });
            fetch('api/Libra/GetTransactions?accountId=' + this.props.account.accountId, {
                headers: { 'Authorization': 'Bearer ' + this.props.token }
                })
                .then(response => response.json())
                .then(data => {
                    this.setState({
                        loading: false,
                        transactions: data.transactions
                    });
                });
        }
    }


    static renderTransactionsTable(transactions) {
        return (
            <div>
                <table className='table'>
                    <thead>
                        <tr>
                            <th># Transaction</th>
                            <th>Receiver</th>
                            <th>Amount</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            transactions.map(transaction =>
                                <tr key={transaction.versionId}>
                                    <td>{transaction.versionId}</td>
                                    <td>{transaction.receiver}</td>
                                    <td>{transaction.amount / 1000000} <img className="libraIconTM" src={require("../images/libra_icon.svg")} alt="logo" /> </td>
                                    
                                </tr>
                            )
                        }
                    </tbody>
                </table>
                {transactions.length == 0 ? <div className="noAccountsMessage">
                    There is no transactions yet
                </div> : null}

            </div>
        );
    }

    render() {
        return (
            <div>
                <div>
                </div>
                <div className="static-modal">
                    <Modal bsSize="large" show={this.props.show}>
                        <Modal.Header>
                            <Modal.Title>
                                Last 10 Transactions - Account: {this.props.account ? this.props.account.addressHashed : ''}
                        </Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            {this.state.loading ?
                                <div className="spinnerContainer">
                                    <img className="spinner" src={require("../images/spinner.gif")} alt="spinner" />
                                </div>
                                :
                                <div>
                                    <div>
                                        {TransactionsModal.renderTransactionsTable(this.state.transactions)}
                                    </div>
                                </div>
                            }   
                        </Modal.Body>
                        <Modal.Footer>
                            <Button onClick={this.props.onClose}>Close</Button>
                        </Modal.Footer>
                    </Modal>

                </div>
            </div>
        );
    }
}

const authCondition = (authUser) => !!authUser

export default withAuthorization(authCondition)(TransactionsModal);