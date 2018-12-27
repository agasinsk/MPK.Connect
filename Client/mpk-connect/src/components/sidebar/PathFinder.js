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
import ArrowBack from '@material-ui/icons/ArrowBack';
import CircularProgress from '@material-ui/core/CircularProgress';

import { selectSource, selectDestination, findTravelPlan } from '../../actions';
import TravelPlan from './TravelPlan';

class PathFinder extends Component {

  constructor(props) {
    super(props);

    this.state = {
      margin: "normal",
      selectedDate: new Date(),
      source: "",
      destination: "",
      showTravelPlan: false
    };

    this.handleDateChange = this.handleDateChange.bind(this);
    this.handleSourceChange = this.handleSourceChange.bind(this);
    this.handleDestinationChange = this.handleDestinationChange.bind(this);
    this.handleGoBack = this.handleGoBack.bind(this);
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

  handleGoBack() {
    const showingTravelPlan = this.state.showTravelPlan;
    this.setState({ showTravelPlan: !showingTravelPlan });
  }

  findPath() {
    this.props.findTravelPlan(this.props.source, this.props.destination, this.state.selectedDate);
    this.setState({ showTravelPlan: true });
  }

  renderView() {
    if (this.state.showTravelPlan && this.props.travelPlan === null && this.props.travelPlan !== "ERROR") {
      return (<Grid item xs={12} className="margined centered">
        <CircularProgress />
      </Grid >);
    }
    if (this.state.showTravelPlan && this.props.travelPlan !== null && this.props.travelPlan !== "ERROR") {
      return this.renderTravelPlans(this.props.travelPlan);
    }

    return this.renderStardardView();
  }

  renderStardardView() {
    return (<Grid item xs={12} className="centered margined">
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
      <Button variant="contained" color="primary" onClick={this.findPath}>
        Wyszukaj połączenie
      </Button>
    </Grid>);
  }

  renderTravelPlans() {

    return (<React.Fragment>
      <Grid item xs={12} className="margined centered">
        <Button variant="outlined" color="primary" onClick={this.handleGoBack} className="back-button">
          Wroć
          <ArrowBack />
        </Button>
      </Grid>
      <Grid item xs={12} className="centered">
        <List dense>
          {this.props.travelPlan.map(travelPlan => {
            return (<TravelPlan key={travelPlan.id} data={travelPlan} />);
          })}
        </List>
      </Grid>
    </React.Fragment>)


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
        {this.renderView()}
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