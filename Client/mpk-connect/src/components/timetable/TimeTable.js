import './TimeTable.css';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import Chip from '@material-ui/core/Chip';
import Grid from '@material-ui/core/Grid';
import List from '@material-ui/core/List';
import Typography from '@material-ui/core/Typography';

import { getTimeTable, selectRoute, unselectRoute } from '../../actions';
import RouteCard from './RouteCard';
import RouteStopTime from './RouteStopTime';

export class TimeTable extends Component {

  constructor(props) {
    super(props);
    this.state = {
      isRouteSelected: false,
      currentRoute: undefined,
    };

    this.handleRouteSelected = this.handleRouteSelected.bind(this);
    this.handleRouteUnselected = this.handleRouteUnselected.bind(this);
  }

  handleRouteSelected(route) {
    this.props.selectRoute(route);
  }

  handleRouteUnselected(route) {
    this.props.unselectRoute();
  }

  renderView() {
    if (this.props.stopId === undefined) {
      return (
        <Typography variant="subtitle1" component="h5" className="top-margin">
          Wybierz przystanek z mapy, aby zobaczyć rozkład jazdy
        </Typography>);
    }
    else {
      let timeTableDetail;

      if (this.props.routes.length === 0 && this.props.timeTable !== null && this.props.stopId === this.props.timeTable.stopId) {
        timeTableDetail =
          <Typography variant="subtitle1" component="h5" className="top-margin">
            O tej porze nie ma odjazdów z tego przystanku.
          </Typography>;
      }
      else {
        if (this.props.selectedRoute !== null) {
          timeTableDetail =
            <RouteStopTime className="top-margin" onClick={() => this.handleRouteUnselected()} />;
        }
        else {
          timeTableDetail =
            <List className="top-margin">
              {this.props.routes.map((route) => (
                <RouteCard key={route.routeId} route={route} onClick={() => this.handleRouteSelected(route)} />
              ))}
            </List>;
        }
      }

      return (
        <React.Fragment>
          <Typography className="top-margin" variant="headline" component="h5" align="center">
            {this.props.stopName}
          </Typography>
          <Chip label={this.props.stopCode} />
          {timeTableDetail}
        </React.Fragment>);
    }
  }

  render() {
    return (
      <Grid container spacing={0}>
        <Grid item xs={12} className="centered margined">
          {this.renderView()}
        </Grid>
      </Grid>
    )
  };
}

const mapStateToProps = state => {
  var routes = [], stopId, stopName, stopCode;
  if (state.timeTable !== null && state.timeTable !== undefined) {
    routes = state.timeTable.routeTimes;
  }
  if (state.selectedStop !== null && state.selectedStop !== undefined) {
    stopId = state.selectedStop.id;
    stopName = state.selectedStop.name;
    stopCode = state.selectedStop.code;
  }
  return {
    selectedRoute: state.selectedRoute,
    stopId,
    stopCode,
    stopName,
    timeTable: state.timeTable,
    routes: routes
  };
};

export default connect(mapStateToProps, { getTimeTable, selectRoute, unselectRoute })(TimeTable);