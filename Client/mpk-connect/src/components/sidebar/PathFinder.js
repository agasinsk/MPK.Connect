import './PathFinder.css';
import 'date-fns';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import TextField from '@material-ui/core/TextField';
import DateFnsUtils from '@date-io/date-fns';
import { MuiPickersUtilsProvider, DateTimePicker } from 'material-ui-pickers';

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
  }

  handleSourceChange(event) {
    this.setState({ source: event.target.value });
  }

  handleDestinationChange(event) {
    this.setState({ destination: event.target.value });
  }

  handleDateChange(date) {
    this.setState({ selectedDate: date });
  }

  render() {
    const selectedDate = this.state.selectedDate;

    return (
      <Grid container spacing={0} alignItems="flex-end">
        <Grid item xs={12} className="margined">
          <TextField
            id="source-point"
            label="Punkt startowy"
            value={this.state.source}
            onChange={this.handleSourceChange}
            variant="outlined"
            margin="normal"
            fullWidth />
        </Grid>
        <Grid item xs={12} className="margined">
          <TextField
            id="destination-point"
            label="Punkt końcowy"
            value={this.state.destination}
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
              value={selectedDate}
              onChange={this.handleDateChange}
              showTodayButton
              format="dd.MM.yyyy, HH:mm "
              label="Data"
              margin="normal"
            />
          </MuiPickersUtilsProvider>
        </Grid>
        <Grid item xs={12} className="padded centered">
          <Button variant="contained" color="secondary">
            Wyszukaj połączenie
          </Button>
        </Grid>
      </Grid >
    )
  };
}

const mapStateToProps = (state) => {
  return {
    name: state.views[state.selectedView].name
  };
};

export default connect(mapStateToProps)(PathFinder);