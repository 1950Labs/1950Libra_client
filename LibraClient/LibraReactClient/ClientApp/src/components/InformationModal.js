import React, { Component } from 'react';
import { Button, Modal, ListGroup, ListGroupItem } from 'react-bootstrap';
import withAuthorization from './WithAuthorization';

class InformationModal extends Component {
    displayName = InformationModal.name

  constructor(props) {
    super(props);
      this.state = {
          loading: true,
          account: []
      };
    }

    getAccountInfo() {
        fetch('api/Libra/GetAccountInfo?accountId=' + this.props.accountId, {
            headers: { 'Authorization': 'Bearer ' + this.props.token }
            })
            .then(response => response.json())
            .then(output => {
                this.setState({
                    account: output.account,
                    loading: false
                });
            });
    }

    componentDidUpdate(prevProps) {
        if (this.props.show && this.props.accountId && prevProps.accountId !== this.props.accountId) {
            this.getAccountInfo();
        }
    }


  render() {
    let contents = this.state.loading
        ? <div className="spinnerContainer">
            <img className="spinner" src={require("../images/spinner.gif")} alt="spinner" />
          </div>
        : <ListGroup>
            <ListGroupItem header="Account Id">{this.state.account.accountId}</ListGroupItem>
            <ListGroupItem header="Owner">{this.state.account.owner}</ListGroupItem>
            <ListGroupItem header="Address">{this.state.account.addressHashed}</ListGroupItem>
            <ListGroupItem header="Balance">{this.state.account.balance} <img className="libraIconIM" src={require("../images/libra_icon.svg")} alt="logo" /></ListGroupItem>
            <ListGroupItem header="Sequence number">{this.state.account.sequenceNumber}</ListGroupItem>
            <ListGroupItem header="Sent events count">{this.state.account.sentEventsCount}</ListGroupItem>
            <ListGroupItem header="Received events count">{this.state.account.receivedEventsCount}</ListGroupItem>
        </ListGroup>

    return (
      <div>
          <div>
            </div>
            <div className="static-modal">
                <Modal show={this.props.show}>
                    <Modal.Header>
                        <Modal.Title>
                            Account Information
                        </Modal.Title>
                    </Modal.Header>

                    <Modal.Body>
                        {contents}
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

export default withAuthorization(authCondition)(InformationModal);