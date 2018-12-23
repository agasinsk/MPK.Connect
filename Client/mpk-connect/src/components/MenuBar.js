import './MenuBar.css';
import React, { Component } from 'react';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import { connect } from 'react-redux';
import { selectView } from '../actions';

class MenuBar extends Component {

  render() {
    return (
      <AppBar position="static" className="grow">
        <Toolbar>
          <Typography variant="h6" color="inherit" className="logo">MPK Connect</Typography>
          <Tabs value={this.props.selectedView} onChange={(event, value) => this.props.selectView(value)}>
            <Tab label={this.props.views[0].name} />
            <Tab label={this.props.views[1].name} />
          </Tabs>
        </Toolbar>
      </AppBar>
    );
  }
}

const mapStateToProps = (state) => {
  return {
    views: state.views,
    selectedView: state.selectedView
  };
};

export default connect(mapStateToProps, {
  selectView
})(MenuBar);
