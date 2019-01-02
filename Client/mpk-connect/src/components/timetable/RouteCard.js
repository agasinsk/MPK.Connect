import './RouteCard.css';
import React, { Component } from 'react';
import TramIcon from '@material-ui/icons/Tram';
import DirectionsBusIcon from '@material-ui/icons/DirectionsBus';
import ListItem from '@material-ui/core/ListItem';
import Button from '@material-ui/core/Button';

class RouteCard extends Component {

  render() {
    return (
      <ListItem onClick={this.props.onClick}>
        <Button variant="outlined" size="large" color="primary" className="route-button" onClick={this.props.onClick}>
          {this.props.route.routeType === "Tram" ? <TramIcon className="route-icon" /> : <DirectionsBusIcon className="route-icon" />}
          {this.props.route.routeId}
        </Button>
      </ListItem>
    );
  }
}

export default RouteCard;