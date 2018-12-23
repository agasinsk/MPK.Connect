import './MenuBar.css';
import React, { Component } from 'react';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';

class MenuBar extends Component {

  constructor(props) {
    super(props);

    this.state = {
      selectedTab: 0
    };

    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(event, value) {
    this.setState({ selectedTab: value });
  };

  render() {
    return (
      <AppBar position="static" className="grow">
        <Toolbar>
          <Typography variant="h6" color="inherit" className="logo">MPK Connect</Typography>
          <Tabs value={this.state.selectedTab} onChange={this.handleChange}>
            <Tab label="Trasy" />
            <Tab label="Rozkład jazdy" />
          </Tabs>
        </Toolbar>
      </AppBar>
    );
  }
}

export default MenuBar;
