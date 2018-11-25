import './TimeTable.css';
import React, { Component } from 'react';
import Drawer from '@material-ui/core/Drawer';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import { RouteCard } from './RouteCard';
import { RouteStopTime } from './RouteStopTime';

export class TimeTable extends Component {

  constructor(props) {
    super(props);
    this.state = {
      timeTable: props.timeTable,
      isRouteSelected: false,
      currentRoute: undefined,
      routes: props.timeTable.routeTimes,
      stopId: props.timeTable.stopId,
      stopName: props.timeTable.stopName,
      stopCode: props.timeTable.stopCode
    };

    this.handleRouteSelected = this.handleRouteSelected.bind(this);
    this.handleRouteUnselected = this.handleRouteUnselected.bind(this);
    console.log('Current stopId: ' + this.state.stopId);
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
      timeTableDetail = <div>
        <RouteStopTime stopId={this.state.stopId} route={this.state.currentRoute} onClick={() => this.handleRouteUnselected(this.state.currentRoute)} />
      </div>;
    }
    else {
      timeTableDetail =
        <div>
          {this.state.routes.map((route) => (
            <RouteCard key={route.routeId} route={route} onClick={() => this.handleRouteSelected(route)} />
          ))}
        </div>
    }
    return (
      <Drawer open={this.props.open} onClose={this.props.onClose} className="timeTable">
        <Paper className="stopInfo" elevation={1}>
          <Typography variant="headline" component="h5" align="center">
            {this.state.stopName}
          </Typography>
          <Button variant="outlined">
            {this.state.stopCode}
          </Button>
        </Paper>
        {timeTableDetail}
      </Drawer>
    );
  }
}
