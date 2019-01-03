import './DirectionStopTimes.css';
import React, { Component } from 'react';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import List from '@material-ui/core/List';
import { connect } from 'react-redux';
import { find, remove } from 'lodash';

import StopTime from './StopTime';

class DirectionStopTimes extends Component {

  render() {
    return (
      <Card>
        <CardContent>
          <Typography variant="subtitle1" component="h2">
            {this.props.direction}
          </Typography>
          <List>
            {this.props.stopTimes.map((stopTime) => (
              <StopTime key={stopTime.tripId} stopTime={stopTime} />
            ))}
          </List>
        </CardContent>
      </Card>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  let stopTimes = ownProps.direction.stopTimes;
  if (state.deletedStopTime !== undefined && state.deletedStopTime !== null) {
    stopTimes = remove(stopTimes, function (stopTime) {
      return stopTime.id === state.deletedStopTime.result.id;
    });
  }

  if (state.updatedStopTime !== undefined && state.updatedStopTime !== null) {
    let stopTimeToUpdate = find(stopTimes, function (stopTime) {
      return stopTime.id === state.updatedStopTime.result.id;
    });
    if (stopTimeToUpdate !== undefined) {
      stopTimeToUpdate.departureTime = state.updatedStopTime.result.departureTime;
    }
  }

  return {
    stopId: state.selectedStop.id,
    direction: ownProps.direction.direction,
    stopTimes
  };
}

export default connect(mapStateToProps)(DirectionStopTimes);
