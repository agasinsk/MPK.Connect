import './RouteCard.css';
import React, { Component } from 'react';
import TramIcon from '@material-ui/icons/Tram';
import DirectionsBusIcon from '@material-ui/icons/DirectionsBus';
import ListItem from '@material-ui/core/ListItem';
import Button from '@material-ui/core/Button';
import Divider from '@material-ui/core/Divider';
import { DirectionStopTimes } from './DirectionStopTimes';

class RouteStopTime extends Component {

  constructor(props) {
    super(props);
    this.state = {
      stopId: props.stopId,
      route: props.route
    };
  }

  render() {
    return (
      <React.Fragment>
        <ListItem onClick={this.props.onClick}>
          <Button variant="outlined" size="large" color="primary" className="route-button" onClick={this.props.onClick}>
            {this.props.route.routeType === "Tram" ? <TramIcon className="route-icon" /> : <DirectionsBusIcon className="route-icon" />}
            {this.props.route.routeId}
          </Button>
        </ListItem>
        <Divider />
        <div>
          {this.state.route.directions.map((direction) => (
            <DirectionStopTimes key={direction.direction} direction={direction} stopId={this.state.stopId} />
          ))}
        </div>
      </React.Fragment>
    );
  }
}

export default RouteStopTime;