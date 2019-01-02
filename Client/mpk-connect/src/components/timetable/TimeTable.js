import React, { Component } from 'react';
import { connect } from 'react-redux';
import Typography from '@material-ui/core/Typography';
import Chip from '@material-ui/core/Chip';
import Grid from '@material-ui/core/Grid';
import List from '@material-ui/core/List';

import RouteCard from './RouteCard';
import { RouteStopTime } from './RouteStopTime';
import { getTimeTable } from '../../actions';

export class TimeTable extends Component {

  constructor(props) {
    super(props);
    this.state = {
      isRouteSelected: false,
      currentRoute: undefined,
    };

    this.handleRouteSelected = this.handleRouteSelected.bind(this);
    this.handleRouteUnselected = this.handleRouteUnselected.bind(this);
  }

  handleRouteSelected(route) {
    console.log('Selected route: ' + route.routeId);
    this.setState({
      currentRoute: route,
      isRouteSelected: true
    });
  }

  handleRouteUnselected(route) {
    console.log('Unselected route: ' + route.routeId);
    this.setState({
      currentRoute: undefined,
      isRouteSelected: false
    });
  }

  renderView() {
    if (this.props.stopId === undefined) {
      return (<Typography variant="headline" component="h5">
        Wybierz przystanek z mapy, aby zobaczyć rozkład jazdy
      </Typography>);
    }
    else {
      let timeTableDetail;

      if (this.state.isRouteSelected) {
        timeTableDetail = (<div>
          <RouteStopTime stopId={this.props.stopId} route={this.props.currentRoute} onClick={() => this.handleRouteUnselected(this.state.currentRoute)} />
        </div>);
      }
      else {
        timeTableDetail =
          <List>
            {this.props.routes.map((route) => (
              <RouteCard key={route.routeId} route={route} onClick={() => this.handleRouteSelected(route)} />
            ))}
          </List>
      }

      return (
        <React.Fragment>
          <Typography variant="headline" component="h5" align="center">
            {this.props.stopName}
          </Typography>
          <Chip label={this.props.stopCode} />
          {timeTableDetail}
        </React.Fragment>);
    }
  }

  render() {
    return (
      <Grid container spacing={0}>
        <Grid item xs={12} className="centered margined">
          {this.renderView()}
        </Grid>
      </Grid>
    )
  };
}

const mapStateToProps = (state) => {
  var routes = [], stopId, stopName, stopCode;
  if (state.timeTable !== null && state.timeTable !== undefined) {
    routes = state.timeTable.routeTimes;
  }
  if (state.selectedStop !== null && state.selectedStop !== undefined) {
    stopId = state.selectedStop.id;
    stopName = state.selectedStop.name;
    stopCode = state.selectedStop.code;
  }
  return {
    stopId,
    stopCode,
    stopName,
    timeTable: state.timeTable,
    routes: routes
  };
};

export default connect(mapStateToProps, { getTimeTable })(TimeTable);