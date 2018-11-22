import React, { Component } from 'react';

import { StopMap } from './components/StopMap';


export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <StopMap></StopMap>
    );
  }
}
