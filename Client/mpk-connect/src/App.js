import './App.css';
import React, { Component } from 'react';
import MenuBar from './MenuBar';
import StopMap from './components/StopMap';
import Grid from '@material-ui/core/Grid';

class App extends Component {
  render() {
    return (
      <Grid container spacing={12}>
        <Grid item xs={12}>
          <MenuBar></MenuBar>
        </Grid>
        <Grid item xs={12}>
          <StopMap></StopMap>
        </Grid>
      </Grid>
    );
  }
}

export default App;
