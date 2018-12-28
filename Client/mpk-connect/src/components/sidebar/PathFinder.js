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
import deburr from "lodash/deburr";
import uniqBy from "lodash/uniqBy";
import Downshift from "downshift";
import Paper from "@material-ui/core/Paper";
import MenuItem from "@material-ui/core/MenuItem";


import { selectSource, selectDestination, findTravelPlan } from '../../actions';
import TravelPlan from './TravelPlan';

class PathFinder extends Component {

  constructor(props) {
    super(props);

    this.state = {
      margin: "normal",
      selectedDate: new Date(),
      showTravelPlan: false
    };

    this.handleDateChange = this.handleDateChange.bind(this);
    this.handleSourceChange = this.handleSourceChange.bind(this);
    this.handleDestinationChange = this.handleDestinationChange.bind(this);
    this.handleGoBack = this.handleGoBack.bind(this);
    this.findPath = this.findPath.bind(this);
  }

  handleSourceChange(source) {
    const sourceObject = {
      name: source,
      latitude: null,
      longitude: null
    };
    this.props.selectSource(sourceObject);
  }

  handleDestinationChange(destination) {
    const destinationObject = {
      name: destination,
      latitude: null,
      longitude: null
    };
    this.props.selectDestination(destinationObject);
  }

  handleDateChange(date) {
    this.setState({ selectedDate: date });
  }

  handleGoBack() {
    const showingTravelPlan = this.state.showTravelPlan;
    this.setState({ showTravelPlan: !showingTravelPlan, selectedDate: new Date() });
  }

  findPath() {
    this.props.findTravelPlan(this.props.source, this.props.destination, this.state.selectedDate);
    this.setState({ showTravelPlan: true });
  }

  renderView() {
    if (this.state.showTravelPlan && this.props.travelPlan === null && !this.props.travelPlanError) {
      return (<Grid item xs={12} className="margined centered">
        <CircularProgress />
      </Grid >);
    }
    if (this.state.showTravelPlan && this.props.travelPlan !== null && !this.props.travelPlanError) {
      return this.renderTravelPlans(this.props.travelPlan);
    }

    return this.renderStardardView();
  }

  renderStardardView() {
    return (
      <React.Fragment>
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
        </Grid>
        <Grid item xs={12} className="centered margined">
          <Button variant="contained" color="primary" onClick={this.findPath}>
            Wyszukaj połączenie
      </Button>
        </Grid>
      </React.Fragment>);
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
        <List dense className="path-list">
          {this.props.travelPlan.map(travelPlan => {
            return (<TravelPlan key={travelPlan.id} data={travelPlan} />);
          })}
        </List>
      </Grid>
    </React.Fragment>)
  }

  getSuggestions(value) {
    const inputValue = deburr(value.trim()).toLowerCase();
    const inputLength = inputValue.length;
    let count = 0;

    return inputLength === 0
      ? []
      : this.props.stopNames.filter(suggestion => {
        const keep =
          count < 7 &&
          suggestion.label.slice(0, inputLength).toLowerCase() === inputValue;

        if (keep) {
          count += 1;
        }

        return keep;
      });
  }

  renderInput(inputProps) {
    const { InputProps, ref, ...other } = inputProps;

    return (
      <TextField
        variant="outlined"
        margin={this.state.margin}
        label={inputProps.label}
        InputProps={{
          inputRef: ref,
          ...InputProps
        }}
        {...other}
      />
    );
  }

  renderSuggestion({
    suggestion,
    index,
    itemProps,
    highlightedIndex,
    selectedItem
  }) {
    const isHighlighted = highlightedIndex === index;
    const isSelected = (selectedItem || "").indexOf(suggestion.label) > -1;

    return (
      <MenuItem
        {...itemProps}
        key={suggestion.label}
        selected={isHighlighted}
        component="div"
        style={{
          fontWeight: isSelected ? 600 : 400
        }}
      >
        {suggestion.label}
      </MenuItem>
    );
  }

  render() {
    return (
      <Grid container spacing={0}>
        <Grid item xs={12} className="margined">
          <Downshift id="source-selection" onChange={this.handleSourceChange}>
            {({
              getInputProps,
              getItemProps,
              getMenuProps,
              highlightedIndex,
              inputValue,
              isOpen,
              selectedItem
            }) => (
                <div className="suggestions-container">
                  {this.renderInput({
                    fullWidth: true,
                    label: "Punkt startowy",
                    InputProps: getInputProps({
                      placeholder: "Wybierz punkt startowy..."
                    })
                  })}
                  <div {...getMenuProps()}>
                    {isOpen ? (
                      <Paper square className="suggestion-paper">
                        {this.getSuggestions(inputValue).map((suggestion, index) =>
                          this.renderSuggestion({
                            suggestion,
                            index,
                            itemProps: getItemProps({ item: suggestion.label }),
                            highlightedIndex,
                            selectedItem
                          })
                        )}
                      </Paper>
                    ) : null}
                  </div>
                </div>
              )}
          </Downshift>

        </Grid>
        <Grid item xs={12} className="margined">
          <Downshift id="destination-selection" onChange={this.handleDestinationChange}>
            {({
              getInputProps,
              getItemProps,
              getMenuProps,
              highlightedIndex,
              inputValue,
              isOpen,
              selectedItem
            }) => (
                <div className="suggestions-container">
                  {this.renderInput({
                    fullWidth: true,
                    label: "Punkt końcowy",
                    InputProps: getInputProps({
                      placeholder: "Wybierz punkt końcowy..."
                    })
                  })}
                  <div {...getMenuProps()}>
                    {isOpen ? (
                      <Paper square className="suggestion-paper">
                        {this.getSuggestions(inputValue).map((suggestion, index) =>
                          this.renderSuggestion({
                            suggestion,
                            index,
                            itemProps: getItemProps({ item: suggestion.label }),
                            highlightedIndex,
                            selectedItem
                          })
                        )}
                      </Paper>
                    ) : null}
                  </div>
                </div>
              )}
          </Downshift>
        </Grid>
        {this.renderView()}
      </Grid >
    )
  };
}

const mapStateToProps = (state) => {
  const travelPlanError = state.travelPlan === "ERROR";
  const stopNames = uniqBy(state.stops, 'name').map(stop => ({
    value: stop.name,
    label: stop.name
  }));

  return {
    stopNames: stopNames,
    source: state.selectedSource,
    destination: state.selectedDestination,
    travelOptions: state.travelOptions,
    travelPlan: state.travelPlan,
    travelPlanError: travelPlanError
  };
};

export default connect(mapStateToProps, { selectSource, selectDestination, findTravelPlan })(PathFinder);