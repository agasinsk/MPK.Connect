import './TravelPlan.css';
import 'date-fns';
import React, { Component } from 'react';
import ListItem from '@material-ui/core/ListItem';
import Chip from '@material-ui/core/Chip';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Avatar from '@material-ui/core/Avatar';
import TramIcon from '@material-ui/icons/Tram';
import ArrowBack from '@material-ui/icons/ArrowBack';
import Button from '@material-ui/core/Button';
import List from '@material-ui/core/List';
import ListItemText from '@material-ui/core/ListItemText';

class TravelPlan extends Component {

  constructor(props) {
    super(props);

    this.state = {
      startTime: props.data.startTime.split('T')[1].split('Z')[0],
      endTime: props.data.endTime.split('T')[1].split('Z')[0],
      duration: props.data.duration,
      routes: props.data.routeIds,
      transfers: props.data.transfers,
      stops: props.data.stops,
      showDetails: false
    };

    this.handleTravelPlanSelection = this.handleTravelPlanSelection.bind(this);
    this.renderGeneralView = this.renderGeneralView.bind(this);
    this.renderDetailedView = this.renderDetailedView.bind(this);
  }

  handleTravelPlanSelection() {
    const showingDetails = this.state.showDetails;
    this.setState({
      showDetails: !showingDetails
    });
  }

  renderGeneralView() {
    return (
      <Paper elevation={10} className="paper-wide" onClick={this.handleTravelPlanSelection}>
        <Paper className="times">
          <Chip color="primary"
            label={this.state.startTime}
            className="chip" />
          <Typography variant="caption" className="margined">
            {this.state.duration} min
        </Typography>
          <Chip color="secondary"
            label={this.state.endTime}
            className="chip" />
        </Paper>

        <Paper className="routes">
          {this.state.routes.map(route => {
            return <Chip key={route} label={route} avatar={<Avatar><TramIcon /></Avatar>} className="route-chip" />
          })}
        </Paper>
      </Paper>
    )
  };

  renderDetailedView() {
    return (
      <Paper elevation={10} className="paper-wide">
        <List dense>
          <ListItem>
            <Button variant="contained" color="secondary" onClick={this.handleTravelPlanSelection}>
              Wroć
              <ArrowBack />
            </Button>
          </ListItem>
          <Paper className="detail-list">
          {this.state.stops.map(stop => {
            return <ListItem key={stop.stopInfo.stopId}>
              <Chip color="primary"
                label={stop.departureTime}
                className="chip" />
              <Chip label={stop.route} avatar={<Avatar><TramIcon /></Avatar>} className="route-chip" variant="outlined" />
              <ListItemText
                primary={stop.stopInfo.name}
                secondary={'Kierunek: ' + stop.direction}
              />
            </ListItem>
          })}
          </Paper>
        </List>
      </Paper>)
  };

  render() {
    var view = null;
    if (this.state.showDetails) {
      view = this.renderDetailedView();
    }
    else {
      view = this.renderGeneralView();
    }
    return (
      <ListItem>{view}</ListItem>

    )
  };
}

export default TravelPlan;