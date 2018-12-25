import './TravelPlan.css';
import 'date-fns';
import React, { Component } from 'react';
import ListItem from '@material-ui/core/ListItem';
import Chip from '@material-ui/core/Chip';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Avatar from '@material-ui/core/Avatar';
import TramIcon from '@material-ui/icons/Tram';

class TravelPlan extends Component {

  constructor(props) {
    super(props);

    this.state = {
      startTime: props.data.startTime.split('T')[1].split('Z')[0],
      endTime: props.data.endTime.split('T')[1].split('Z')[0],
      duration: props.data.duration,
      routes: props.data.routeIds,
      transfers: props.data.transfers,
      stops: props.data.stops,
    };
  }

  render() {

    return (
      <ListItem >
        <Paper elevation={10} className="paper-wide">
          <Paper className="times">
            <Chip color="primary"
              label={this.state.startTime}
              className="chip" />
            <Typography variant="caption" className="margined">
              {this.state.duration} min
            </Typography>
            <Chip color="secondary"
              label={this.state.endTime}
              className="chip" />
          </Paper>

          <Paper className="routes">
            {this.state.routes.map(route => {
              return <Chip label={route} avatar={<Avatar><TramIcon /></Avatar>} className="route-chip" />
            })}
          </Paper>
        </Paper>

      </ListItem >

    )
  };
}

export default TravelPlan;