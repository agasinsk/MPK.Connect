import './RouteCard.css';
import React, { Component } from 'react';
import TramIcon from '@material-ui/icons/Tram';
import DirectionsBusIcon from '@material-ui/icons/DirectionsBus';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import IconButton from '@material-ui/core/IconButton';

export class RouteCard extends Component {
 
  render() {
    return (
      <Card className="routeCard" >
        <CardActionArea>
          <CardContent className="routeCardButton">  
            <Button variant="outlined">
              <Typography variant="title" component="h2">
                {this.props.routeId} 
              </Typography>
            </Button>
            <IconButton>
              <div className="routeTypeIcon">
                {this.props.routeType === "Tram" ?<TramIcon /> : <DirectionsBusIcon />}
              </div>
            </IconButton>
          </CardContent>
        </CardActionArea>
      </Card>
    );
  }
}
