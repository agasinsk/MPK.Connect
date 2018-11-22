import './TimeTable.css';
import React, { Component } from 'react';
import Drawer from '@material-ui/core/Drawer';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import { RouteCard } from './RouteCard';
import { RouteStopTime } from './RouteStopTimes';

export class TimeTable extends Component {

  constructor(props) {
    super(props);
    this.state = {
      isRouteSelected: false,
      currentRoute: undefined,
      routes: [
        {
          routeId: '31',
          routeType: 'Tram',
          directions: [
            {
              direction: "PL. JANA PAWŁA II",
              stopTimes: [
                {
                  tripId: "6_6527011",
                  departureTime: "14:20:00"
                },
                {
                  tripId: "6_6527011",
                  departureTime: "14:23:00"
                },
                {
                  tripId: "6_6527011",
                  departureTime: "14:26:00"
                },
                {
                  tripId: "6_6527011",
                  departureTime: "14:29:00"
                },
                {
                  tripId: "6_6527033",
                  departureTime: "14:50:00"
                },
                {
                  tripId: "6_6527057",
                  departureTime: "15:20:00"
                },
                {
                  tripId: "6_6527013",
                  departureTime: "15:50:00"
                }
              ]
            },
            {
              direction: "GAJ",
              stopTimes: [
                {
                  tripId: "6_6527011",
                  departureTime: "14:20:00"
                },
                {
                  tripId: "6_6527033",
                  departureTime: "14:50:00"
                },
                {
                  tripId: "6_6527057",
                  departureTime: "15:20:00"
                },
                {
                  tripId: "6_6527013",
                  departureTime: "15:50:00"
                }
              ]
            }
          ]
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
        <RouteStopTime route={this.state.currentRoute} onClick={() => this.handleRouteUnselected(this.state.currentRoute)} />
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
            GALERIA DOMINIKAŃSKA
          </Typography>
          <Button variant="outlined">
            21120
          </Button>
        </Paper>
        {timeTableDetail}
      </Drawer>
    );
  }
}
