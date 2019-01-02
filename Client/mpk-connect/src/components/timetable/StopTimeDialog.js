import React, { Component } from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';

class StopTimeDialog extends Component {

  render() {
    return (
      <Dialog
        open={this.props.open}
        onClose={this.props.handleCancel}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title">{this.props.text}</DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            {this.props.description}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={this.props.handleCancel} color="primary">
            Cancel
       </Button>
          <Button onClick={this.props.handleConfirm} color="primary" autoFocus>
            OK
       </Button>
        </DialogActions>
      </Dialog>
    );
  }
}

export default StopTimeDialog;
