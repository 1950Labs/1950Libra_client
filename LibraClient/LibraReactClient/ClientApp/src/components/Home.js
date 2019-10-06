import React, { Component } from 'react';
import withAuthorization from './WithAuthorization';

class Home extends Component {
  displayName = Home.name

  render() {
    return (
      <div>
            <h1>Libra Net Core Client</h1>
            <p>Welcome to new Libra Client, developed by <a href='http://1950labs.com/'>1950Labs</a></p>
            <p>Last update: <b>6th October 2019</b></p>
        <p>You will be able to:</p>
        <ul>
          <li>Create new Libra accounts</li>
          <li>Create transactions between accounts</li>
          <li>Query transacitons and accounts</li>
        </ul>
      </div>
    );
  }
}

const authCondition = (authUser) => !!authUser

export default withAuthorization(authCondition)(Home);

