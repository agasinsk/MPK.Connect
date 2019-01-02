import './StopTime.css';
import React, { Component } from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import IconButton from '@material-ui/core/IconButton';
import CheckIcon from '@material-ui/icons/Check';
import DeleteIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import TextField from '@material-ui/core/TextField';
import Tooltip from '@material-ui/core/Tooltip';
import Snackbar from '@material-ui/core/Snackbar';
import Button from '@material-ui/core/Button';
import CloseIcon from '@material-ui/icons/Close';

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
      onDelete: props.onDelete,
      snackBarText: '',
      snackBarVariant: 'success',
      showSnackbar: false
    };

    this.handleEditMode = this.handleEditMode.bind(this);
    this.handleDeleteMode = this.handleDeleteMode.bind(this);
    this.updateDepartureTime = this.updateDepartureTime.bind(this);
    this.updateStopTime = this.updateStopTime.bind(this);
    this.deleteStopTime = this.deleteStopTime.bind(this);
    this.confirmUpdate = this.confirmUpdate.bind(this);
    this.confirmDelete = this.confirmDelete.bind(this);
    this.closeSnackbar = this.closeSnackbar.bind(this);
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

    var updatedStopTime = {
      stopId: this.state.stopId,
      tripId: this.state.stopTime.tripId,
      departureTime: this.state.stopTime.departureTime,
      updatedDepartureTime: updatedDepartureTime
    };
    console.log(JSON.stringify(updatedStopTime));
    fetch('api/StopTime/', {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(updatedStopTime)
    })
      .then(function (response) {
        if (response.ok) {
          return response.json();
        }
        throw new Error(JSON.stringify(response));
      })
      .then((data) => {
        console.log('Success:', JSON.stringify(data));
        this.setState({
          stopTime: {
            tripId: data.result.tripId,
            departureTime: data.result.departureTime
          },
          updatedDepartureTime: undefined,
          showDialog: false,
          showSnackbar: true,
          snackBarVariant: 'success',
          snackBarText: data.text
        });
      })
      .catch(error => {
        console.log('Error:', JSON.stringify(error));
        this.setState({
          showSnackbar: true,
          snackBarVariant: 'error',
          snackBarText: error
        });
      });

    this.handleEditMode();
  }

  closeSnackbar(reason) {
    if (reason === 'clickaway') {
      return;
    }

    this.setState({ showSnackbar: false });
  }

  deleteStopTime() {
    console.log("Deleting stop time: " + this.state.stopTime.departureTime);

    var stopTimeToDelete = {
      stopId: this.state.stopId,
      tripId: this.state.stopTime.tripId,
      departureTime: this.state.stopTime.departureTime,
    };
    console.log(JSON.stringify(stopTimeToDelete));
    fetch('api/StopTime/', {
      method: 'DELETE',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(stopTimeToDelete)
    })
      .then(function (response) {
        if (response.ok) {
          return response.json();
        }
        throw new Error(JSON.stringify(response));
      })
      .then((data) => {
        console.log('Success:', JSON.stringify(data));
        this.setState({
          stopTime: {
            tripId: data.result.tripId,
            departureTime: data.result.departureTime
          },
          showDialog: false,
          showSnackbar: true,
          snackBarVariant: 'success',
          snackBarText: data.text
        });
      })
      .catch(error => {
        console.log('Error:', JSON.stringify(error));
        this.setState({
          showSnackbar: true,
          snackBarVariant: 'error',
          snackBarText: error
        });
      });

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
          label="Edit stop time"
          type="time"
          defaultValue={this.state.stopTime.departureTime}
          className="stopTimeEdit"
          variant="outlined"
          InputLabelProps={{
            shrink: true
          }}
          inputProps={{
            step: 1,
          }}
          onChange={this.updateDepartureTime} />;

      actionIcon = <Tooltip title="Update">
        <IconButton onClick={this.confirmUpdate}>
          <CheckIcon />
        </IconButton>
      </Tooltip>;
    }
    else {
      stopTime = <ListItemText primary={this.state.stopTime.departureTime} className="stopTimeText" />;
      actionIcon = <Tooltip title="Delete">
        <IconButton onClick={this.confirmDelete}>
          <DeleteIcon />
        </IconButton>
      </Tooltip>;
    }

    return (
      <React.Fragment>
        <ListItem dense>
          <Tooltip title="Edit">
            <IconButton onClick={this.handleEditMode}>
              <EditIcon color="secondary" />
            </IconButton>
          </Tooltip>
          {stopTime}
          {actionIcon}
        </ListItem>
        <StopTimeDialog
          open={this.state.showDialog}
          text={this.state.dialogText}
          description={this.state.dialogDescription}
          handleCancel={this.state.handleCancel}
          handleConfirm={this.state.handleConfirm} />
        <Snackbar
          anchorOrigin={{
            vertical: 'bottom',
            horizontal: 'left',
          }}
          open={this.state.showSnackbar}
          autoHideDuration={6000}
          onClose={this.closeSnackbar}
          message={<span id="message-id">{this.state.snackBarText}</span>}
          action={[
            <Button key="undo" color="secondary" size="small" onClick={this.closeSnackbar}>
              OK
            </Button>,
            <IconButton
              key="close"
              color="inherit"
              onClick={this.handleClose}
            >
              <CloseIcon />
            </IconButton>,
          ]}
        />
      </React.Fragment>
    );
  }
}
