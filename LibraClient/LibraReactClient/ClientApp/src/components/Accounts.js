import React, { Component } from 'react';
import { Button, Modal, FormGroup, ControlLabel, FormControl, HelpBlock, Alert, Glyphicon, OverlayTrigger, Tooltip } from 'react-bootstrap';
import './Accounts.css';
import InformationModal from './InformationModal';
import MintModal from './MintModal';
import TransactionsModal from './TransactionsModal';
import withAuthorization from './WithAuthorization';
import ReCAPTCHA from "react-google-recaptcha";

class Accounts extends Component {
    displayName = Accounts.name

    recaptchaRef = React.createRef();

  constructor(props) {
    super(props);
      this.state = {
          accounts: [],
          loading: true,
          showModal: false,
          showAlert: false,
          showForm: false,
          owner: '',
          addAccountResult: false,
          account: undefined,
          savingAccount: false,
          showInformationModal: false,
          showMintModal: false,
          showTransactionsModal: false,
          accountAddress: undefined
      };

      

      this.handleShowModal = this.handleShowModal.bind(this);
      this.handleCloseModal = this.handleCloseModal.bind(this);
      this.handleChangeOwner = this.handleChangeOwner.bind(this);
      this.handleChangeOwner = this.handleChangeOwner.bind(this);
      this.handleConfirm = this.handleConfirm.bind(this);
      this.handleDismissAlert = this.handleDismissAlert.bind(this);
      this.handleShowInformationModal = this.handleShowInformationModal.bind(this);
      this.handleCloseInformationModal = this.handleCloseInformationModal.bind(this);
      this.handleShowMintModal = this.handleShowMintModal.bind(this);
      this.handleCloseMintModal = this.handleCloseMintModal.bind(this);
      this.onChangeReCaptcha = this.onChangeReCaptcha.bind(this);

      this.handleShowTransactionsModal = this.handleShowTransactionsModal.bind(this);
      this.handleCloseTransactionsModal = this.handleCloseTransactionsModal.bind(this);
    }

    componentDidMount() {
        if (this.props.token) {
            this.getAccounts();
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
                        loading: false
                    });
                });
        

       
    }
    onChangeReCaptcha(value) {
        if (value) {
            this.handleConfirm();
        }
    }


    handleCloseModal() {
        this.setState({ showModal: false });
    }

    handleShowModal() {
        this.setState({ showModal: true, savingAccount: false, showAlert: false });
    }

    handleChangeOwner(e) {
        this.setState({ owner: e.target.value });
    }

    handleDismissAlert(e) {
        this.setState({ showAlert: false });
    }


    handleConfirm() {
        this.setState({ savingAccount: true });
        
        fetch('api/Libra/AddAccountAsync', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json', 'Authorization': 'Bearer ' + this.props.token },
            body: JSON.stringify({
                "owner": this.state.owner,
                "userUID": this.props.userUid
            })
        })
            .then(response => response.json())
            .then(data => {
                this.setState({
                    addAccountResult: data.operationSuccess,
                    showAlert: true,
                    showModal: false,
                    account: data.account,
                    loading: true
                });

                this.getAccounts();
            });
    }

    handleShowInformationModal(accountId) {
        this.setState({ accountId: accountId, showInformationModal: true, showAlert: false });
    }

    handleCloseInformationModal() {
        this.setState({ showInformationModal: false, accountId: undefined, account: undefined });
    }

    handleShowMintModal(accountAddress) {
        this.setState({ accountAddress: accountAddress, showMintModal: true, showAlert: false });
    }

    handleCloseMintModal() {
        this.setState({ showMintModal: false, accountAddress: undefined });
    }

    handleShowTransactionsModal(accountId) {
        this.setState({ accountId: accountId, showTransactionsModal: true });
    }

    handleCloseTransactionsModal() {
        this.setState({ showTransactionsModal: false, accountId: undefined });
    }

    static renderAccountsTable(accounts, handleShowInformationModal, handleShowMintModal, handleShowTransactionsModal) {
      return (
      <div>
          <table className='table'>
            <thead>
              <tr>
                <th>AccountId</th>
                <th>Owner</th>
                <th>Address</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {accounts.map(account =>
                  <tr key={account.accountId}>
                      <td>{account.accountId}</td>
                      <td>{account.owner}</td>
                      <td>{account.addressHashed}</td>
                      <td>
                          <OverlayTrigger placement="top" overlay={<Tooltip id="tooltip">More info</Tooltip>}>
                            <Button bsStyle="primary" className="actionButton" onClick={() => handleShowInformationModal(account.accountId)}> <Glyphicon glyph='info-sign' /></Button>
                          </OverlayTrigger>
                          <OverlayTrigger placement="top" overlay={<Tooltip id="tooltip">Mint</Tooltip>}>
                              <Button bsStyle="primary" className="actionButton" onClick={() => handleShowMintModal(account.addressHashed)}> <Glyphicon glyph='plus' /></Button>
                          </OverlayTrigger>
                          <OverlayTrigger placement="top" overlay={<Tooltip id="tooltip">Last 10 transactions</Tooltip>}>
                              <Button bsStyle="primary" className="actionButton" onClick={() => handleShowTransactionsModal(account.accountId)}> <Glyphicon glyph='list-alt' /></Button>
                          </OverlayTrigger>
                      </td>
                </tr>
                      )}

            
            </tbody>
              </table>
              {accounts.length == 0 ? <div className="noAccountsMessage">
                  There is no accounts yet
            </div> : null}
              
      </div>
    );
  }

  render() {
    let contents = this.state.loading
        ? <div className="spinnerContainer">
            <img className="spinner" src={require("../images/spinner.gif")} alt="spinner" />
        </div>

        :
        <div>
            {Accounts.renderAccountsTable(this.state.accounts, this.handleShowInformationModal, this.handleShowMintModal, this.handleShowTransactionsModal)}
            <InformationModal show={this.state.showInformationModal} accountId={this.state.accountId} onClose={this.handleCloseInformationModal} />
            <MintModal show={this.state.showMintModal} accountAddress={this.state.accountAddress} onClose={this.handleCloseMintModal} />
            <TransactionsModal show={this.state.showTransactionsModal} accountId={this.state.accountId} onClose={this.handleCloseTransactionsModal} />
        </div>

    return (
      <div>
          <div>
            <h1>Accounts</h1>
                <p>Here you can manage all the accounts that belongs to the client.</p>
                <p>At the moment we call the faucet service in order to create an account (We are waiting that somebody anwser us in the Libra Community)</p>
                <div>
                    <Button variant="primary" onClick={this.handleShowModal}>
                        Add account
                    </Button>
                </div>
                <br />
                {
                    this.state.showAlert ?
                        <Alert bsStyle={this.state.addAccountResult ? "success" : "danger"} onDismiss={this.handleDismissAlert}>
                            <h3>{this.state.addAccountResult ? 'Account created successfully' : 'Account not created'}</h3>
                            {this.state.addAccountResult ?
                                <div>
                                    <h4>New Account Info</h4>
                                    <p>The account was created successfully.</p>
                                    <p>Owner: {this.state.account.owner}</p>
                                    <p>Address: {this.state.account.addressHashed}</p>
                                </div>
                                :
                                <p>
                                    There was a problem creating the account. This could be a problem in the network or simple the owner already exists in the Libra Network.
                                </p>
                            }
                            <p>
                                <Button bsStyle="primary" onClick={this.handleDismissAlert}>Close</Button>
                            </p>
                        </Alert>
                        : null
                }
                <br />
                {contents}
            </div>


            <div className="static-modal">
                <Modal show={this.state.showModal}>
                    <Modal.Header>
                        <Modal.Title>
                            New account
                        </Modal.Title>
                    </Modal.Header>

                    <Modal.Body>
                        {!this.state.savingAccount ?
                            <div>
                                <form>
                                    <FormGroup controlId="owner">
                                        <ControlLabel>Owner:</ControlLabel>
                                        <FormControl type="text" placeholder="Enter the account owner" value={this.state.recipient} onChange={this.handleChangeOwner} />
                                        <HelpBlock>
                                            Write a unique name to create an new account.
                                    </HelpBlock>
                                    </FormGroup>
                                </form>
                                <ReCAPTCHA
                                    ref={this.recaptchaRef}
                                    sitekey="6Le0LrgUAAAAAK1rJfEnyZvL7boTYKfm71ULxLHH"
                                    size="invisible"
                                    badge="inline"
                                    onChange={this.onChangeReCaptcha}
                                />
                            </div>
                            :
                            <div className="spinnerContainer">
                                <img className="spinner" src={require("../images/spinner.gif")} alt="spinner" />    
                            </div>
                            
                        }
                        
                    </Modal.Body>

                    <Modal.Footer>
                        <Button onClick={this.handleCloseModal} disabled={this.state.savingAccount}>Close</Button>
                        <Button bsStyle="primary" onClick={() => { this.recaptchaRef.current.execute(); }} disabled={this.state.savingAccount}>Confirm</Button>
                    </Modal.Footer>
                </Modal>

            </div>
        </div>
    );
  }
}

const authCondition = (authUser) => !!authUser

export default withAuthorization(authCondition)(Accounts);
