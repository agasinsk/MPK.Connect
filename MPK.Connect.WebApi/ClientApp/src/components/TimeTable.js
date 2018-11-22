import './TimeTable.css';
import React, { Component } from 'react';
import Drawer from '@material-ui/core/Drawer';
import Divider from '@material-ui/core/Divider';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import { RouteCard } from './RouteCard';

export class TimeTable extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentRoute: undefined,
      routes: [
        {
          routeId:'31',
          routeType: 'Tram'
        }, 
        {
          routeId:'32',
          routeType: 'Tram'
        }, 
        {
          routeId:'101',
          routeType: 'Bus'
        }],
      stopId: props.stopId
    };
  
    this.handleRouteChoosen = this.handleRouteChoosen.bind(this);

    // fetch('api/TimeTable/' + this.props.stopId)
    //         .then(response => response.json())
    //         .then(data => {
    //             console.log('Received time table for stop ' + data.stopId)
    //             this.setState({
    //                 routes: [{routeId:'31'}, '32', '101'],
    //             });
    //         });
  }

  handleRouteChoosen(routeId){
    console.log('Chosen route: ' + routeId);
    //this.setState({currentRoute : routeId});
  }

  render() {
    return (
      <Drawer open={this.props.open} onClose={this.props.onClose} className="timeTable">
          <div
            tabIndex={0}
            role="button"            
          >
            <Paper className="stopInfo" elevation={1}>
              <Typography variant="h5" component="h3">
                GALERIA DOMINIKAŃSKA
              </Typography>
              <Typography component="p">
                Code: 2112
              </Typography>
            </Paper>
          <Divider/>
          <div>
            {this.state.routes.map((route) => (
              <RouteCard routeId={route.routeId} routeType={route.routeType} onClick={this.handleRouteChoosen(route.routeId)}/>
            ))}       
          </div>
        </div>
        </Drawer>
    );
  }
}
