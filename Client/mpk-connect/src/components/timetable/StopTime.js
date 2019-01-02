import './StopTime.css';
import React, { Component } from 'react';
import Grid from '@material-ui/core/Grid';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import IconButton from '@material-ui/core/IconButton';
import CheckIcon from '@material-ui/icons/Check';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import TextField from '@material-ui/core/TextField';
import Tooltip from '@material-ui/core/Tooltip';
import CircularProgress from '@material-ui/core/CircularProgress';
import { connect } from 'react-redux';

import StopTimeDialog from './StopTimeDialog';
import { updateStopTime, deleteStopTime } from '../../actions';

class StopTime extends Component {

  constructor(props) {
    super(props);
    this.state = {
      editMode: false,
      deleteMode: false,
      updatedDepartureTime: undefined,
      dialogText: undefined,
      dialogDescription: undefined,
      showDialog: false,
      onDelete: props.onDelete,
    };

    this.handleEditMode = this.handleEditMode.bind(this);
    this.handleDeleteMode = this.handleDeleteMode.bind(this);
    this.updateDepartureTime = this.updateDepartureTime.bind(this);
    this.updateStopTime = this.updateStopTime.bind(this);
    this.deleteStopTime = this.deleteStopTime.bind(this);
    this.confirmUpdate = this.confirmUpdate.bind(this);
    this.confirmDelete = this.confirmDelete.bind(this);
  }

  updateDepartureTime(event) {
    let currentValue = event.target.value;
    console.log("Wants to update stop time to: " + currentValue);
    if (currentValue !== null && currentValue !== undefined) {
      this.setState({ updatedDepartureTime: currentValue });
    }
  }

  confirmUpdate() {
    let updatedDepartureTime = this.state.updatedDepartureTime;
    if (updatedDepartureTime !== null && updatedDepartureTime !== undefined) {
      this.setState({
        dialogText: "Zaktualizować czas odjazdu?",
        dialogDescription: "Czy jesteś pewny, że chcesz zmienić czas odjazdu na " + updatedDepartureTime + "?",
        showDialog: true,
        handleCancel: this.handleEditMode,
        handleConfirm: this.updateStopTime
      });
    }
  }

  confirmDelete() {
    this.setState({
      dialogText: "Usunąć przystanek z kursu?",
      dialogDescription: "Czy jesteś pewny, że chcesz to zrobić?",
      showDialog: true,
      handleCancel: this.handleDeleteMode,
      handleConfirm: this.deleteStopTime
    })
  }

  updateStopTime() {
    let updatedDepartureTime = this.state.updatedDepartureTime;
    console.log("Updating stop time to: " + updatedDepartureTime);

    var updatedStopTime = {
      stopId: this.props.stopId,
      tripId: this.props.stopTime.tripId,
      departureTime: this.props.stopTime.departureTime,
      updatedDepartureTime: updatedDepartureTime
    };
    console.log(JSON.stringify(updatedStopTime));

    this.props.updateStopTime(updatedStopTime);
    this.handleEditMode();
  }

  deleteStopTime() {
    console.log("Deleting stop time: " + this.props.stopTime.departureTime);

    var stopTimeToDelete = {
      stopId: this.props.stopId,
      tripId: this.props.stopTime.tripId,
      departureTime: this.props.stopTime.departureTime,
    };
    console.log(JSON.stringify(stopTimeToDelete));

    this.props.deleteStopTime(stopTimeToDelete);
    this.handleDeleteMode();
    this.state.onDelete();
  }

  handleEditMode() {
    let currentEditMode = this.state.editMode;
    this.setState({
      editMode: !currentEditMode,
      showDialog: false
    });
  }

  handleDeleteMode() {
    let currentDeleteMode = this.state.deleteMode;
    console.log("Delete mode: " + !currentDeleteMode);
    this.setState({
      deleteMode: !currentDeleteMode,
      showDialog: false
    });
  }

  renderView() {
    if (this.state.editMode) {
      if (this.props.updatedStopTime === null && !this.props.stopTimeError) {
        return (<Grid item xs={12} className="margined centered">
          <CircularProgress />
        </Grid >);
      }

      return (<React.Fragment>
        <TextField
          id="updatedStopTime"
          label="Edytuj czas"
          type="time"
          defaultValue={this.props.stopTime.departureTime}
          className="stopTimeEdit"
          variant="outlined"
          InputLabelProps={{
            shrink: true
          }}
          inputProps={{
            step: 1,
          }}
          onChange={this.updateDepartureTime} />
        <Tooltip title="Akceptuj">
          <IconButton onClick={this.confirmUpdate}>
            <CheckIcon />
          </IconButton>
        </Tooltip>
      </React.Fragment>);
    }

    const updatedStopTime = this.props.updatedStopTime;
    var departureTime = this.props.stopTime.departureTime;
    if (updatedStopTime !== undefined && updatedStopTime.tripId === this.props.stopTime.tripId) {
      departureTime = this.props.updatedStopTime.departureTime;
    }

    return (<React.Fragment>
      <ListItemText primary={departureTime} className="stopTimeText" />
      <Tooltip title="Usuń">
        <IconButton onClick={this.confirmDelete}>
          <DeleteIcon />
        </IconButton>
      </Tooltip>
    </React.Fragment >);
  }

  render() {
    console.log(this.props);

    return (
      <React.Fragment>
        <ListItem>
          <Tooltip title="Edit">
            <IconButton onClick={this.handleEditMode}>
              <EditIcon color="primary" />
            </IconButton>
          </Tooltip>
          {this.renderView()}
        </ListItem>
        <StopTimeDialog
          open={this.state.showDialog}
          text={this.state.dialogText}
          description={this.state.dialogDescription}
          handleCancel={this.state.handleCancel}
          handleConfirm={this.state.handleConfirm} />
      </React.Fragment>
    );
  }
}

const mapStateToProps = (state) => {
  const stopTimeError = state.selectedStopTime === "ERROR";
  let stopId, updatedStopTime, resultText;
  if (state.selectedStop !== undefined && state.selectedStop !== null) {
    stopId = state.selectedStop.id;
  }

  if (state.selectedStopTime !== undefined && state.selectedStopTime !== null) {
    resultText = state.selectedStopTime.text;
    updatedStopTime = state.selectedStopTime.result;
  }

  return {
    stopId,
    resultText,
    updatedStopTime,
    stopTimeError
  };
};

export default connect(mapStateToProps, { updateStopTime, deleteStopTime })(StopTime);
