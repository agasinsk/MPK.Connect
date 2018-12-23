import './App.css';
import React, { Component } from 'react';
import Grid from '@material-ui/core/Grid';

import MenuBar from './MenuBar';
import StopMap from './StopMap';

class App extends Component {
  render() {
    return (
      <Grid container spacing={0}>
        <Grid item xs={12}>
          <MenuBar></MenuBar>
        </Grid>
        <Grid item xs={3}>
        </Grid>
        <Grid item xs={9}>
          <StopMap></StopMap>
        </Grid>
      </Grid>
    );
  }
}

export default App;
