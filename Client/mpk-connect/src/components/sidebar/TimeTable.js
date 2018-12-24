import React, { Component } from 'react';
import { connect } from 'react-redux';
import Typography from '@material-ui/core/Typography';

class TimeTable extends Component {

  render() {
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

export default connect(mapStateToProps)(TimeTable);