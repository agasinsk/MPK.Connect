import './DirectionStopTimes.css';
import React, { Component } from 'react';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import { StopTime } from './StopTime';
import List from '@material-ui/core/List';

export class DirectionStopTimes extends Component {

  constructor(props) {
    super(props);
    this.state = {
      stopId: props.stopId,
      direction: props.direction.direction,
      stopTimes: props.direction.stopTimes
    };
  }

  deleteStopTime(stopTime) {
    let stopTimes = this.state.stopTimes;
    let stopTimeIndex = stopTimes.indexOf(stopTime);
    delete stopTimes[stopTimeIndex];
    this.setState({
      stopTimes: stopTimes
    })
  }

  render() {
    return (
      <Card>
        <CardContent>
          <Typography variant="subtitle1" component="h2">
            {this.state.direction}
          </Typography>
          <List>
            {this.state.stopTimes.map((stopTime) => (
              <StopTime key={stopTime.tripId} stopId={this.state.stopId} stopTime={stopTime} onDelete={() => this.deleteStopTime(stopTime)} />
            ))}
          </List>
        </CardContent>
      </Card>
    );
  }
}
