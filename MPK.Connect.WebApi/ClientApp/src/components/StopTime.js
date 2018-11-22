import './RouteCard.css';
import React, { Component } from 'react';
import Typography from '@material-ui/core/Typography';
import ListItem from '@material-ui/core/ListItem';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import ListItemText from '@material-ui/core/ListItemText';
import Checkbox from '@material-ui/core/Checkbox';
import IconButton from '@material-ui/core/IconButton';
import DeleteIcon from '@material-ui/icons/Delete';

export class StopTime extends Component {

  constructor(props) {
    super(props);
    this.state = {
      stopId: props.stopId,
      stopTime: props.stopTime
    };
  }

  render() {
    return (
      <ListItem key={this.state.stopTime.tripId} button>
        <Checkbox
          checked={true}
          tabIndex={-1}
          disableRipple
        />
        <ListItemText primary={this.state.stopTime.departureTime} />
        <ListItemSecondaryAction>
          <IconButton>
            <DeleteIcon />
          </IconButton>
        </ListItemSecondaryAction>
      </ListItem>
    );
  }
}
