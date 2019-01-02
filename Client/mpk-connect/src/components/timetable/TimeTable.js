import React, { Component } from 'react';
import { connect } from 'react-redux';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';

import { RouteCard } from './RouteCard';
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
    console.log('Current stopId: ' + this.state.stopId);
  }

  componentDidMount() {
    if (this.props.stopId !== undefined) {
      this.props.getTimeTable(this.props.stopId);
    }
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

  render() {
    let timeTableDetail;

    if (this.state.isRouteSelected) {
      timeTableDetail = (<div>
        <RouteStopTime stopId={this.props.stopId} route={this.props.currentRoute} onClick={() => this.handleRouteUnselected(this.state.currentRoute)} />
      </div>);
    }
    else {
      timeTableDetail =
        <div>
          {this.props.routes.map((route) => (
            <RouteCard key={route.routeId} route={route} onClick={() => this.handleRouteSelected(route)} />
          ))}
        </div>
    }

    return (
      <div className="timeTable">
        <Paper className="stopInfo" elevation={1}>
          <Typography variant="headline" component="h5" align="center">
            {this.props.stopName}
          </Typography>
          <Button variant="outlined">
            {this.props.stopCode}
          </Button>
        </Paper>
        {timeTableDetail}
      </div>
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