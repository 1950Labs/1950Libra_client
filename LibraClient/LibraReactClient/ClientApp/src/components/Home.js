import React, { Component } from 'react';
import withAuthorization from './WithAuthorization';

class Home extends Component {
  displayName = Home.name

  render() {
    return (
      <div>
            <h1>Libra Net Core Client</h1>
            <p>Welcome to new Libra Client, developed by <a href='http://1950labs.com/' target="_blank">1950Labs</a></p>
            <p>For further information about Libra or this Libra Client feel free to contact us in <b>hello@1950labs.com</b></p>
            <p>Last update: <b>11th November 2019</b></p>
            <p>Check our code <a href="https://github.com/1950Labs/2019_POC_Libra" target="_blank">here</a></p>

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

