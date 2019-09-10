import React, { Component } from 'react';
import { Button, FormGroup, ControlLabel, FormControl, HelpBlock, Glyphicon, Modal, OverlayTrigger, Tooltip } from 'react-bootstrap';
import './NewTransaction.css';
import withAuthorization from './WithAuthorization';

class NewTransaction extends Component {
    displayName = NewTransaction.name

    constructor(props) {
        super(props);
        this.state = {
            sourceAccountId: 0,
            source: '',
            recipient: '',
            amount: 0,
            value: '',
            showModal: false,
            loadingAccounts: false,
            accounts: [],
            formValid: false,
            loading: false,
            transactionSubmitted: false,
            transactionSuccess: false
        };

        this.handleChangeRecipient = this.handleChangeRecipient.bind(this);
        this.handleChangeAmount = this.handleChangeAmount.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleShowModal = this.handleShowModal.bind(this);
        this.handleCloseModal = this.handleCloseModal.bind(this);
        this.handleSelectSourceAccount = this.handleSelectSourceAccount.bind(this);

    }

    handleChangeRecipient(e) {
        this.setState({ recipient: e.target.value });
    }

    handleShowModal() {
        this.setState({ showModal: true, loadingAccounts: true });
        this.getAccounts();
    }

    handleCloseModal() {
        this.setState({ showModal: false });
    }

    handleSelectSourceAccount(account) {
        this.setState({ sourceAccountId: account.accountId, source: account.owner + ' - ' + account.addressHashed, showModal: false });
    }

    handleChangeAmount(e) {
        this.setState({ amount: +e.target.value });
    }

    validateForm() {
        if (this.state.source && this.state.recipient && this.state.amount && this.state.amount > 0) {
            return true;
        } else {
            return false;
        }
    }

    getAccounts() {
        fetch('api/Libra/GetAccounts?userUID=' + this.props.userUid, {
            headers: { 'Authorization': 'Bearer ' + this.props.token }
             })
            .then(response => response.json())
            .then(data => {
                this.setState({
                    accounts: data,
                    loadingAccounts: false
                });
            });
    }

    handleSubmit(event) {
        this.setState({ transactionSubmitted: true, loading: true });
        fetch('api/Libra/SubmitTransaction', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json', 'Authorization': 'Bearer ' + this.props.token },
            body: JSON.stringify({
                "sourceAccountId": this.state.sourceAccountId,
                "recipient": this.state.recipient,
                "amount": this.state.amount
            })
        })
            .then(response => response.json())
            .then(data => {
                this.setState({ loading: false });
                if (data.sequenceNumber) {
                    this.setState({ transactionSuccess: true });
                } else {
                    this.setState({ transactionSuccess: false });
                }
            });
        event.preventDefault();
    }

    static renderAccountsTable(accounts, handleSelectSourceAccount) {
        return (
            <div>
                <table className='table'>
                    <thead>
                        <tr>
                            <th>Owner</th>
                            <th>Address</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                        <tbody>
                                {
                                    accounts.map(account =>
                                        <tr key={account.accountId}>
                                            <td>{account.owner}</td>
                                            <td>{account.addressHashed}</td>
                                            <td>
                                                <OverlayTrigger placement="top" overlay={<Tooltip id="tooltip">Select</Tooltip>}>
                                                    <Button bsStyle="primary" className="actionButton" onClick={() => handleSelectSourceAccount(account)}> <Glyphicon glyph='ok-circle' /></Button>
                                                </OverlayTrigger>
                                            </td>
                                        </tr>
                                    )
                                }
                        </tbody>
                </table>
                {accounts.length == 0 ? <div className="noAccountsMessage">
                    There is no accounts yet
                </div> : null}

            </div>
        );
    }


    render() {
        return (
            <div>
                <h1>New transaction</h1>
                <form onSubmit={this.handleSubmit}>
                    <FormGroup controlId="source">
                        <ControlLabel>Source Libra account:</ControlLabel>
                        <div className="sourceAccountContainer">
                            <FormControl type="text" placeholder="Select source Libra account" value={this.state.source} readOnly></FormControl>
                            <Button bsStyle="primary" className="searchAccounts" onClick={this.handleShowModal}> <Glyphicon glyph='search' /></Button>
                        </div>
                        <HelpBlock >
                            The source Libra account must exist in the network.
                    </HelpBlock>
                    </FormGroup>
                    <FormGroup controlId="recipient">
                        <ControlLabel>Recipient Libra account:</ControlLabel>
                        <FormControl type="text" placeholder="Enter recipient Libra account" value={this.state.recipient} onChange={this.handleChangeRecipient} />
                        <HelpBlock>
                            The recipient account could not exists in the Libra network
                    </HelpBlock>
                    </FormGroup>
                    <FormGroup controlId="amount">
                        <ControlLabel>Amount:</ControlLabel>
                        <FormControl type="number" placeholder="Amount" value={this.state.amount} onChange={this.handleChangeAmount} />
                        <HelpBlock>
                            The amount must be higher than 0
                    </HelpBlock>
                    </FormGroup>
                    <div>
                        <Button bsStyle="primary" type="submit" className="submitButton" disabled={!(this.validateForm() && !this.state.transactionSubmitted)}>
                            Submit Transaction
                        </Button>
                        {
                            this.state.transactionSubmitted ?
                            <div className="resultContainer">
                            {
                                this.state.loading ?
                                <div>
                                    <img className="spinner" src={require("../images/spinner.gif")} alt="spinner" />
                                </div>
                                :
                                            <div>
                                                {this.state.transactionSuccess ?
                                                    <div className="MessageContainerTx">
                                                        <span className="successMessage">Transaction Success</span>
                                                        <div className="TestnetContainerTx">
                                                            <p className="TestnetTextTx">Testnet</p>
                                                        </div>
                                                    </div> :
                                                    <span className="errorMessage">Transaction Error</span>}
                                </div>
                            }
                            </div> : null
                        }
                    </div>
                </form>

                <div className="static-modal">
                    <Modal show={this.state.showModal} onHide={this.handleCloseModal}
                        bsSize="large"
                        aria-labelledby="contained-modal-title-lg">
                        <Modal.Header closeButton>
                            <Modal.Title>
                                Select an account
                        </Modal.Title>
                        </Modal.Header>

                        <Modal.Body>
                           
                            {this.state.loadingAccounts ?
                                <div className="spinnerContainer">
                                    <img className="spinner" src={require("../images/spinner.gif")} alt="spinner" />
                                </div>
                                :
                                <div>
                                    <div>
                                        {NewTransaction.renderAccountsTable(this.state.accounts, this.handleSelectSourceAccount)}
                                    </div>
                                </div>
                            }

                        </Modal.Body>

                        <Modal.Footer>
                            <Button onClick={this.handleCloseModal} >Close</Button>
                        </Modal.Footer>
                    </Modal>

                </div>
            </div>
        );
    }
}

const authCondition = (authUser) => !!authUser

export default withAuthorization(authCondition)(NewTransaction);

