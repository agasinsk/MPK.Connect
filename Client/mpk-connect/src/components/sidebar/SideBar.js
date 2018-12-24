import React, { Component } from 'react';
import { connect } from 'react-redux';
import Typography from '@material-ui/core/Typography';

import PathFinder from './PathFinder';
import TimeTable from './TimeTable';

class SideBar extends Component {

  render() {
    if (this.props.value === 0) {
      return (<PathFinder />)
    }
    if (this.props.value === 1) {
      return (<TimeTable />)
    }
    return (
      <Typography variant="h6" color="inherit">{this.props.name}</Typography>
    )
  };
}

const mapStateToProps = (state) => {
  return {
    value: state.selectedView,
    name: state.views[state.selectedView].name
  };
};

export default connect(mapStateToProps)(SideBar);