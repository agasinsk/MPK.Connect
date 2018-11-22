import './TimeTable.css';
import React, { Component } from 'react';
import Drawer from '@material-ui/core/Drawer';
import Divider from '@material-ui/core/Divider';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import { RouteCard } from './RouteCard';

export class TimeTable extends Component {

  constructor(props) {
    super(props);
    this.state = {
      isRouteSelected: false,
      currentRoute: undefined,
      routes: [
        {
          routeId: '31',
          routeType: 'Tram'
        },
        {
          routeId: '32',
          routeType: 'Tram'
        },
        {
          routeId: '101',
          routeType: 'Bus'
        }],
      stopId: props.stopId
    };

    this.handleRouteSelected = this.handleRouteSelected.bind(this);
    console.log('Current stopId: ' + this.state.stopId);
  }

  handleRouteSelected(routeId) {
    console.log('Chosen route: ' + routeId);
    this.setState({
      currentRoute: routeId,
      isRouteSelected: true
    });
  }

  render() {
    let timeTableDetail;

    if (this.state.isRouteSelected) {
      timeTableDetail = null;
    }
    else {
      timeTableDetail =
        <div>
          {this.state.routes.map((route) => (
            <RouteCard key={route.routeId} routeId={route.routeId} routeType={route.routeType} onClick={() => this.handleRouteSelected(route.routeId)} />
          ))}
        </div>
    }
    return (
      <Drawer open={this.props.open} onClose={this.props.onClose} className="timeTable">
        <Paper className="stopInfo" elevation={1}>
          <Typography variant="headline" component="h5" align="center">
            GALERIA DOMINIKAŃSKA
          </Typography>
          <Button variant="outlined">
            21120
          </Button>
        </Paper>
        <Divider />
        {timeTableDetail}
      </Drawer>
    );
  }
}
