import './DirectionStopTimes.css';
import React, { Component } from 'react';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import { StopTime } from './StopTime';
import List from '@material-ui/core/List';

export class DirectionStopTimes extends Component {

  render() {
    return (
      <div className="directionBackground">
        <Card>
          <CardContent>
            <Typography variant="subtitle1" component="h2">
              {this.props.direction.direction}
            </Typography>
            <List>
              {this.props.direction.stopTimes.map((stopTime) => (
                <StopTime key={stopTime.tripId} stopId={this.props.stopId} stopTime={stopTime} />
              ))}
            </List>
          </CardContent>
        </Card>
      </div>
    );
  }
}
