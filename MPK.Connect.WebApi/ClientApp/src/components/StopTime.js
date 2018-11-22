import './StopTime.css';
import React, { Component } from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import IconButton from '@material-ui/core/IconButton';
import CheckIcon from '@material-ui/icons/Check';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import TextField from '@material-ui/core/TextField';
import { StopTimeDialog } from './StopTimeDialog';

export class StopTime extends Component {

  constructor(props) {
    super(props);
    this.state = {
      stopId: props.stopId,
      stopTime: props.stopTime,
      editMode: false,
      deleteMode: false,
      updatedDepartureTime: undefined,
      dialogText: undefined,
      dialogDescription: undefined,
      showDialog: false,
      onDelete: props.onDelete
    };

    this.handleEditMode = this.handleEditMode.bind(this);
    this.handleDeleteMode = this.handleDeleteMode.bind(this);
    this.updateDepartureTime = this.updateDepartureTime.bind(this);
    this.updateStopTime = this.updateStopTime.bind(this);
    this.deleteStopTime = this.deleteStopTime.bind(this);
    this.confirmUpdate = this.confirmUpdate.bind(this);
    this.confirmDelete = this.confirmDelete.bind(this);
  }

  updateDepartureTime(e) {
    let currentValue = e.target.value;
    console.log("Wants to update stop time to: " + currentValue);
    if (currentValue !== null && currentValue !== undefined) {
      this.setState({ updatedDepartureTime: currentValue });
    }
  }

  confirmUpdate() {
    let updatedDepartureTime = this.state.updatedDepartureTime;
    if (updatedDepartureTime !== null && updatedDepartureTime !== undefined) {
      this.setState({
        dialogText: "Update stop time?",
        dialogDescription: "Are you sure you want to update stop time to " + updatedDepartureTime + "?",
        showDialog: true,
        andleCancel: this.handleEditMode,
        handleConfirm: this.updateStopTime
      })
    }
  }

  confirmDelete() {
    this.setState({
      dialogText: "Delete stop time?",
      dialogDescription: "Are you sure you want to delete this stop time?",
      showDialog: true,
      handleCancel: this.handleDeleteMode,
      handleConfirm: this.deleteStopTime
    })
  }

  updateStopTime() {
    let updatedDepartureTime = this.state.updatedDepartureTime;
    console.log("Updating stop time to: " + updatedDepartureTime);
    //TODO: PUT StopTime
    this.setState({
      stopTime: {
        tripId: this.state.stopTime.tripId,
        departureTime: updatedDepartureTime
      },
      updatedDepartureTime: undefined,
      showDialog: false
    });
    this.handleEditMode();
  }

  deleteStopTime() {
    console.log("Deleting stop time: " + this.state.stopTime.departureTime);
    //TODO: DELETE StopTime
    this.handleDeleteMode();
    this.state.onDelete();
  }

  handleEditMode() {
    let currentEditMode = this.state.editMode;
    console.log("Edit mode: " + !currentEditMode);
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

  render() {
    let stopTime, actionIcon;

    if (this.state.editMode) {
      stopTime =
        <TextField
          id="updatedStopTime"
          label="Edit time"
          type="time"
          defaultValue={this.state.stopTime.departureTime}
          className="stopTimeEdit"
          variant="outlined"
          InputLabelProps={{
            shrink: true,
          }}
          inputProps={{
            step: 1,
          }}
          onChange={this.updateDepartureTime} />

      actionIcon = <IconButton onClick={this.confirmUpdate}>
        <CheckIcon />
      </IconButton>
    }
    else {
      stopTime = <ListItemText primary={this.state.stopTime.departureTime} className="stopTimeText" />;
      actionIcon = <IconButton onClick={this.confirmDelete}>
        <DeleteIcon />
      </IconButton>;
    }

    return (
      <React.Fragment>
        <ListItem dense>
          <IconButton onClick={this.handleEditMode}>
            <EditIcon color="secondary" />
          </IconButton>
          {stopTime}
          {actionIcon}
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
