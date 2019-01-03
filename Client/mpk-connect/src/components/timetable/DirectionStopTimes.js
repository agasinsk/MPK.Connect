import './DirectionStopTimes.css';
import React, { Component } from 'react';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import List from '@material-ui/core/List';
import { connect } from 'react-redux';
import { find, findIndex, remove } from 'lodash';

import StopTime from './StopTime';

class DirectionStopTimes extends Component {

  constructor(props) {
    super(props);

    this.state = {
      stopTimes: this.props.stopTimes
    };
  }

  render() {
    return (
      <Card className="margined-side">
        <CardContent>
          <Typography variant="overline" className="direction-text">
            Kierunek:
            <Typography variant="h6">
              {this.props.direction}
            </Typography>
          </Typography>
          <List className="stop-time-list">
            {this.state.stopTimes.map((stopTime) => (
              <StopTime key={stopTime.tripId} stopTime={stopTime} />
            ))}
          </List>
        </CardContent>
      </Card>
    );
  }
}

const mapStateToProps = (state, ownProps) => {
  var stopTimes = ownProps.direction.stopTimes;
  const deleted = state.deletedStopTime;
  if (deleted !== undefined && deleted !== null) {
    let indexToDelete = findIndex(stopTimes, function (stopTime) {
      return stopTime.id === deleted.result.id;
    });
    if (indexToDelete > -1) {
      remove(stopTimes, function (stopTime) {
        return stopTime.id === deleted.result.id;
      });
    }
  }

  const updated = state.updatedStopTime;
  if (updated !== undefined && updated !== null) {
    let stopTimeToUpdate = find(stopTimes, function (stopTime) {
      return stopTime.id === updated.result.id;
    });
    if (stopTimeToUpdate !== undefined) {
      stopTimeToUpdate.departureTime = updated.result.departureTime;
    }
  }

  return {
    stopId: state.selectedStop.id,
    direction: ownProps.direction.direction,
    stopTimes: stopTimes
  };
}

export default connect(mapStateToProps)(DirectionStopTimes);
