import './RouteCard.css';
import React, { Component } from 'react';
import TramIcon from '@material-ui/icons/Tram';
import DirectionsBusIcon from '@material-ui/icons/DirectionsBus';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import Divider from '@material-ui/core/Divider';
import { DirectionStopTimes } from './DirectionStopTimes';

export class RouteStopTime extends Component {

  constructor(props) {
    super(props);
    this.state = {
      stopId: props.stopId,
      route: props.route
    };
  }

  render() {
    return (
      <React.Fragment>
        <Card className="routeCard" >
          <CardActionArea onClick={this.props.onClick}>
            <CardContent className="routeCardButton">
              <div className="MuiButtonBase-root-27 MuiButton-root-1 MuiButton-outlined-9">
                <Typography variant="title">
                  {this.state.route.routeId}
                </Typography>
              </div>
              <div className="MuiButtonBase-root-27 MuiIconButton-root-3 routeTypeIcon">
                {this.state.route.routeType === "Tram" ? <TramIcon /> : <DirectionsBusIcon />}
              </div>
            </CardContent>
          </CardActionArea>
        </Card>
        <Divider />
        <div>
          {this.state.route.directions.map((direction) => (
            <DirectionStopTimes key={direction.direction} direction={direction} stopId={this.state.stopId} />
          ))}
        </div>
      </React.Fragment>
    );
  }
}
