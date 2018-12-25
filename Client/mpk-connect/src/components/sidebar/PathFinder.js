import './PathFinder.css';
import 'date-fns';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import TextField from '@material-ui/core/TextField';
import DateFnsUtils from '@date-io/date-fns';
import { MuiPickersUtilsProvider, DateTimePicker } from 'material-ui-pickers';
import List from '@material-ui/core/List';

import { selectSource, selectDestination, findTravelPlan } from '../../actions';
import TravelPlan from './TravelPlan';

class PathFinder extends Component {

  constructor(props) {
    super(props);

    this.state = {
      margin: "normal",
      selectedDate: new Date(),
      source: "",
      destination: ""
    };

    this.handleDateChange = this.handleDateChange.bind(this);
    this.handleSourceChange = this.handleSourceChange.bind(this);
    this.handleDestinationChange = this.handleDestinationChange.bind(this);
    this.findPath = this.findPath.bind(this);
  }

  handleSourceChange(event) {
    const source = {
      name: event.target.value
    };
    this.props.selectSource(source);
  }

  handleDestinationChange(event) {
    const destination = {
      name: event.target.value
    };
    this.props.selectDestination(destination);
  }

  handleDateChange(date) {
    this.setState({ selectedDate: date });
  }

  findPath() {
    this.props.findTravelPlan(this.props.source, this.props.destination, this.state.selectedDate);
  }

  renderTravelPlans(travelPlans) {
    if (travelPlans !== undefined && travelPlans.Comfortable !== undefined) {
      return this.props.travelPlan.Comfortable.map((travelPlan) => {
        return <TravelPlan data={travelPlan} />
      });
    }
    else {
      const travelPlan = {
        startTime: "2018-12-25T19:42:00Z",
        endTime: "2018-12-25T19:57:00Z",
        duration: 15,
        routeIds: [
          "33", "22"
        ],
        transfers: 1,
        stops: [
          {
            arrivalTime: "19:42:00",
            departureTime: "19:42:00",
            route: "33",
            routeType: "Tram",
            direction: "SĘPOLNO",
            stopInfo: {
              code: "12206",
              latitude: 51.12527525,
              longitude: 16.98342767,
              name: "Kwiska",
              id: "1501"
            },
            stopSequence: 5,
            tripId: "6_6517658"
          }]
      };
      return <TravelPlan data={travelPlan} />
    }

  }

  render() {
    return (
      <Grid container spacing={0} alignItems="flex-end">
        <Grid item xs={12} className="margined">
          <TextField
            id="source-point"
            label="Punkt startowy"
            value={this.props.source.name}
            onChange={this.handleSourceChange}
            variant="outlined"
            margin={this.state.margin}
            fullWidth />
        </Grid>
        <Grid item xs={12} className="margined">
          <TextField
            id="destination-point"
            label="Punkt końcowy"
            value={this.props.destination.name}
            onChange={this.handleDestinationChange}
            variant="outlined"
            margin={this.state.margin}
            fullWidth />
        </Grid>
        <Grid item xs={12} className="centered margined">
          <MuiPickersUtilsProvider utils={DateFnsUtils}>
            <DateTimePicker
              autoOk
              ampm={false}
              value={this.state.selectedDate}
              onChange={this.handleDateChange}
              showTodayButton
              format="dd.MM.yyyy, HH:mm"
              label="Data"
              margin={this.state.margin}
            />
          </MuiPickersUtilsProvider>
          <Button variant="contained" color="secondary" onClick={this.findPath}>
            Wyszukaj połączenie
          </Button>
        </Grid>
        <Grid item xs={12} className="centered">
          <List >
            {this.renderTravelPlans(this.props.travelPlan)}
          </List>
        </Grid>
      </Grid >
    )
  };
}

const mapStateToProps = (state) => {
  return {
    source: state.selectedSource,
    destination: state.selectedDestination,
    travelOptions: state.travelOptions,
    travelPlan: state.travelPlan
  };
};

export default connect(mapStateToProps, { selectSource, selectDestination, findTravelPlan })(PathFinder);