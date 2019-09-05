import React, { Component } from 'react';
import { Button, Modal, FormGroup, ControlLabel, FormControl, Alert } from 'react-bootstrap';
import './MintModal.css'
import withAuthorization from './WithAuthorization';

class MintModal extends Component {
    displayName = MintModal.name

  constructor(props) {
    super(props);
      this.state = {
          amount: 0,
          loading: false,
          formValid: false,
          minted: undefined,
          showAlert: false,
          amountDisabled: false
      };

      this.handleChangeAmount = this.handleChangeAmount.bind(this);
      this.handleConfirm = this.handleConfirm.bind(this); 
      this.handleDismissAlert = this.handleDismissAlert.bind(this); 
    }

    handleChangeAmount(e) {
        this.setState({ amount: e.target.value });
        if (e.target.value > 0) {
            this.setState({ formValid: true });
        }
        else {
            this.setState({ formValid: false });
        }
    }
    componentDidUpdate(prevProps) {
        if (this.props.show && this.props.accountAddress && prevProps.accountAddress !== this.props.accountAddress) {
            this.setState({ amount: 0 });
        }
    }

    handleDismissAlert(e) {
        this.setState({ showAlert: false });
    }


    handleConfirm() {
        this.setState({ loading: true});
        fetch('api/Libra/Mint', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json', 'Authorization': 'Bearer ' + this.props.token },
            body: JSON.stringify({
                "address": this.props.accountAddress,
                "amount": this.state.amount
            })
        })
            .then(response => response.json())
            .then(data => {
                this.setState({
                    minted: data.minted,
                    loading: false,
                    showAlert: true,
                    formValid: false,
                    amountDisabled: true
                });

                
            });
    }

    getValidationState() {
        const amount = this.state.amount;
        if (amount > 0) {
            return 'success'
        }
        else {
            return 'error';
        }
    }


  render() {
      let contents = 
          <div>
            <form>
              <FormGroup controlId="Address" >
                  <ControlLabel>Address:</ControlLabel>
                  <FormControl type="text" className="addressTextBox" size="lg" value={this.props.accountAddress} disabled />
              </FormGroup>
              <FormGroup controlId="amount" validationState={this.getValidationState()}>
                  <ControlLabel>Amount:</ControlLabel>
                  <FormControl type="number" value={this.state.amount} onChange={this.handleChangeAmount} disabled={this.state.minted} />
              </FormGroup>
              </form>
              { this.state.loading
                  ? <div className="spinnerContainer">
                      <img className="spinner" src={require("../images/spinner.gif")} alt="spinner" />
                  </div> : null }
           </div>

    return (
      <div>
          <div>
            </div>
            <div className="static-modal">
                <Modal show={this.props.show}>
                    <Modal.Header>
                        <Modal.Title>
                            Account Mint
                        </Modal.Title>
                    </Modal.Header>

                    <Modal.Body>
                        {contents}
                        {
                            this.state.showAlert ?
                                <Alert bsStyle={this.state.minted ? "success" : "danger"} onDismiss={this.handleDismissAlert}>
                                    {this.state.minted ?
                                        <div>
                                            <h4>Mint transaction successful</h4>
                                            <p>Your balance will be updated.</p>
                                        </div>
                                        :
                                        <p>
                                            There was a problem minting the account. This could be a problem in the Network..
                                </p>
                                    }
                                </Alert>
                                : null
                        }
                    </Modal.Body>

                    <Modal.Footer>
                        <Button onClick={this.props.onClose}>Close</Button>
                        {!this.state.minted ? <Button bsStyle="primary" onClick={this.handleConfirm} disabled={!this.state.formValid}>Confirm</Button> : null }
                    </Modal.Footer>
                </Modal>

            </div>
        </div>
    );
  }
}

const authCondition = (authUser) => !!authUser

export default withAuthorization(authCondition)(MintModal);