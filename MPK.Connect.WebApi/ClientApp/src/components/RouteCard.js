import './RouteCard.css';
import React, { Component } from 'react';
import TramIcon from '@material-ui/icons/Tram';
import DirectionsBusIcon from '@material-ui/icons/DirectionsBus';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';

export class RouteCard extends Component {

  render() {
    return (
      <Card className="routeCard" >
        <CardActionArea onClick={this.props.onClick}>
          <CardContent className="routeCardButton">
            <div className="MuiButtonBase-root-27 MuiButton-root-1 MuiButton-outlined-9">
              <Typography variant="title">
                {this.props.routeId}
              </Typography>
            </div>
            <div className="MuiButtonBase-root-27 MuiIconButton-root-3 routeTypeIcon">
              {this.props.routeType === "Tram" ? <TramIcon /> : <DirectionsBusIcon />}
            </div>
          </CardContent>
        </CardActionArea>
      </Card>
    );
  }
}
