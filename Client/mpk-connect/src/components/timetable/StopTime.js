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
      actionAwaited: false,
      editMode: false,
      updatedDepartureTime: undefined,
      dialogText: undefined,
      dialogDescription: undefined,
      showDialog: false
    };

    this.handleEditMode = this.handleEditMode.bind(this);
    this.updateDepartureTime = this.updateDepartureTime.bind(this);
    this.updateStopTime = this.updateStopTime.bind(this);
    this.deleteStopTime = this.deleteStopTime.bind(this);
    this.confirmUpdate = this.confirmUpdate.bind(this);
    this.confirmDelete = this.confirmDelete.bind(this);
    this.handleUpdateMode = this.handleUpdateMode.bind(this);
    this.handleDialogCancel = this.handleDialogCancel.bind(this);
  }

  updateDepartureTime(event) {
    let currentValue = event.target.value;
    if (currentValue !== null && currentValue !== undefined) {
      this.setState({ updatedDepartureTime: currentValue });
    }
  }

  handleEditMode() {
    let currentEditMode = this.state.editMode;
    this.setState({
      editMode: !currentEditMode,
      showDialog: false
    });
  }

  handleDialogCancel() {
    this.setState({
      editMode: false,
      showDialog: false
    });
  }

  handleUpdateMode() {
    this.setState({
      actionAwaited: true,
      editMode: false,
      showDialog: false
    });
  }

  handleDeleteMode() {
    this.setState({
      actionAwaited: true,
      showDialog: false
    });
  }

  confirmUpdate() {
    let updatedDepartureTime = this.state.updatedDepartureTime;
    if (updatedDepartureTime !== null && updatedDepartureTime !== undefined) {
      this.setState({
        dialogText: "Zaktualizować czas odjazdu?",
        dialogDescription: "Czy jesteś pewny, że chcesz zmienić czas odjazdu na " + updatedDepartureTime + "?",
        showDialog: true,
        handleCancel: this.handleDialogCancel,
        handleConfirm: this.updateStopTime
      });
    }
  }

  confirmDelete() {
    this.setState({
      dialogText: "Usunąć przystanek z kursu?",
      dialogDescription: "Czy jesteś pewny, że chcesz usunąć przystanek z kursu?",
      showDialog: true,
      handleCancel: this.handleDialogCancel,
      handleConfirm: this.deleteStopTime
    });
  }

  updateStopTime() {
    let updatedDepartureTime = this.state.updatedDepartureTime;

    var updatedStopTime = {
      id: this.props.stopTime.id,
      departureTime: this.props.stopTime.departureTime,
      updatedDepartureTime: updatedDepartureTime
    };

    this.props.updateStopTime(updatedStopTime);
    this.handleUpdateMode();
  }

  deleteStopTime() {
    console.log("Deleting stop time with id " + this.props.stopTime.id);
    this.props.deleteStopTime(this.props.stopTime.id);
    this.handleUpdateMode();
  }

  renderView() {
    if (this.props.result === undefined && this.state.actionAwaited) {
      return (<Grid item xs={12} className="margined centered">
        <CircularProgress />
      </Grid>);
    }

    if (this.state.editMode) {
      return (<React.Fragment>
        <Tooltip title="Edytuj">
          <IconButton onClick={this.handleEditMode}>
            <EditIcon color="primary" />
          </IconButton>
        </Tooltip>
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

    const updatedStopTime = this.props.result;
    var departureTime = this.props.stopTime.departureTime;
    if (updatedStopTime !== undefined && updatedStopTime.tripId === this.props.stopTime.tripId) {
      departureTime = updatedStopTime.departureTime;
    }

    return (<React.Fragment>
      <Tooltip title="Edytuj">
        <IconButton onClick={this.handleEditMode}>
          <EditIcon color="primary" />
        </IconButton>
      </Tooltip>
      <ListItemText primary={departureTime} className="stopTimeText" />
      <Tooltip title="Usuń">
        <IconButton onClick={this.confirmDelete}>
          <DeleteIcon />
        </IconButton>
      </Tooltip>
    </React.Fragment>);
  }

  render() {
    return (
      <React.Fragment>
        <ListItem>
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

const mapStateToProps = (state, ownProps) => {
  let result, resultText, status;

  if (state.updatedStopTime !== undefined && state.updatedStopTime !== null) {
    resultText = state.updatedStopTime.text;
    result = state.updatedStopTime.result;
    status = state.updatedStopTime.statusCode;
  }

  if (state.deletedStopTime !== undefined && state.deletedStopTime !== null) {
    resultText = state.deletedStopTime.text;
    result = state.deletedStopTime.result;
    status = state.deletedStopTime.statusCode;
  }

  return {
    stopId: state.selectedStop.id,
    resultText,
    status,
    result
  };
};

export default connect(mapStateToProps, { updateStopTime, deleteStopTime })(StopTime);
