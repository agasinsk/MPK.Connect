import './RouteCard.css';
import React, { Component } from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import TramIcon from '@material-ui/icons/Tram';
import DirectionsBusIcon from '@material-ui/icons/DirectionsBus';
import Typography from '@material-ui/core/Typography';
import Divider from '@material-ui/core/Divider';

export class RouteStopTime extends Component {
 
  render() {
    return (
      <React.Fragment>
        <ListItem button className="routeCard">
            <ListItemIcon>
            {this.props.routeType === "Tram" ? <TramIcon className="icon" color="primary"/> : <DirectionsBusIcon color="primary"/>}
            </ListItemIcon>
            <ListItemText primary={this.props.routeId} />
        </ListItem>
        <Divider />
      </React.Fragment>
    );
  }
}
