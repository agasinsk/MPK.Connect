import './PathFinder.css';
import 'date-fns';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import TextField from '@material-ui/core/TextField';
import DateFnsUtils from '@date-io/date-fns';
import { MuiPickersUtilsProvider, DateTimePicker } from 'material-ui-pickers';

import { selectSource, selectDestination, findTravelPlan } from '../../actions';

class PathFinder extends Component {

  constructor(props) {
    super(props);

    this.state = {
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
            margin="normal"
            fullWidth />
        </Grid>
        <Grid item xs={12} className="margined">
          <TextField
            id="destination-point"
            label="Punkt końcowy"
            value={this.props.destination.name}
            onChange={this.handleDestinationChange}
            variant="outlined"
            margin="normal"
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
              format="dd.MM.yyyy, HH:mm "
              label="Data"
              margin="normal"
            />
          </MuiPickersUtilsProvider>
        </Grid>
        <Grid item xs={12} className="padded centered">
          <Button variant="contained" color="secondary" onClick={this.findPath}>
            Wyszukaj połączenie
          </Button>
        </Grid>
      </Grid >
    )
  };
}

const mapStateToProps = (state) => {
  return {
    source: state.selectedSource,
    destination: state.selectedDestination,
    travelOptions: state.travelOptions
  };
};

export default connect(mapStateToProps, { selectSource, selectDestination, findTravelPlan })(PathFinder);