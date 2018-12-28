import './StopMap.css';
import React, { Component } from 'react';
import Control from 'react-leaflet-control';
import { Map, TileLayer, ZoomControl, Marker, Popup, Polyline } from 'react-leaflet';
import { connect } from 'react-redux';

import { getStops } from '../actions';
import { Button } from '@material-ui/core';

const mapCenter = [51.105, 17.035];
const zoomLevel = 15;

export class StopMap extends Component {

  constructor(props) {
    super(props);

    this.state = {
      currentZoomLevel: zoomLevel,
      bounds: undefined,
      showStops: false,
      visibleStops: []
    };
    this.handleMapChange = this.handleMapChange.bind(this);
    this.handleShowStops = this.handleShowStops.bind(this);
  }

  componentDidMount() {
    const leafletMap = this.leafletMap.leafletElement;
    leafletMap.on('zoomend', () => {
      const updatedZoomLevel = leafletMap.getZoom();
      this.handleZoomLevelChange(updatedZoomLevel);
    });

    leafletMap.on('moveend ', () => {
      this.handleMapChange();
    });

    this.props.getStops();
  }

  handleMapChange() {
    const currentBounds = this.leafletMap.leafletElement.getBounds();
    this.setState({
      bounds: currentBounds
    });
  }

  handleShowStops() {
    const showingShops = this.state.showStops;
    this.setState({ showStops: !showingShops });
  }

  filterStops(stops) {
    let bounds = this.state.bounds;
    if (bounds === undefined) {
      bounds = this.leafletMap.leafletElement.getBounds();
    }
    let visibleStops = stops.filter(function (stop) {
      return stop.latitude < bounds._northEast.lat
        && stop.latitude > bounds._southWest.lat
        && stop.longitude < bounds._northEast.lng
        && stop.longitude > bounds._southWest.lng;
    })
    return visibleStops;
  }

  handleZoomLevelChange(newZoomLevel) {
    this.setState({
      currentZoomLevel: newZoomLevel
    });
  }

  renderStops() {
    if (this.state.showStops) {
      let filteredStops = this.filterStops(this.props.allStops);

      return (filteredStops.map((stop) => {
        let position = [stop.latitude, stop.longitude];
        return <Marker key={`marker-${stop.id}`} position={position}>
          <Popup>
            <span><b>{stop.name}</b>
              <br /> {stop.code}
              <br /> {stop.latitude}
              <br /> {stop.longitude}
            </span>
          </Popup>
        </Marker>
      }));
    }
    return null;
  }

  renderPath() {
    if (this.props.selectedTravelPlan !== null && this.props.selectedTravelPlan.stops !== undefined) {
      return (<React.Fragment>
        {this.props.selectedTravelPlan.stops.map((stop) => {
          let position = [stop.stopInfo.latitude, stop.stopInfo.longitude];
          return (<Marker key={`path-stop-${stop.stopInfo.id}`} position={position}>
            <Popup>
              <span>
                <b>{stop.stopInfo.name}</b>
                <br /> Odjazd: {stop.departureTime}
                <br /> Linia: {stop.route}
              </span>
            </Popup>
          </Marker>)
        })}
        <Polyline color="red" positions={this.props.travelPlanCoordinates} />
      </React.Fragment>)
    }
    return null;
  }

  render() {
    console.log('this.state.currentZoomLevel ->', this.state.currentZoomLevel);
    return (
      <Map ref={m => { this.leafletMap = m; }} center={mapCenter} zoom={zoomLevel} zoomControl={false}>
        <Control position="topright" >
          <Button variant="contained" onClick={this.handleShowStops}>
            {this.state.showStops ? "Ukryj przystanki" : "Pokaż przystanki"}
          </Button>
        </Control>
        <ZoomControl position="bottomright" />
        <TileLayer
          attribution="&amp;copy <a href=&quot;http://osm.org/copyright&quot;>OpenStreetMap</a> contributors"
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        {this.renderStops()}
        {this.renderPath()}
      </Map >
    );
  }
}

const mapStateToProps = (state) => {
  const selectedTravelPlan = state.selectedTravelPlan;
  console.log(selectedTravelPlan);
  var travelPlanCoordinates = [];
  if (selectedTravelPlan !== null && selectedTravelPlan.stops !== undefined) {
    travelPlanCoordinates = selectedTravelPlan.stops.map(stop => [stop.stopInfo.latitude, stop.stopInfo.longitude]);
  }
  return {
    allStops: state.stops,
    selectedTravelPlan: selectedTravelPlan,
    travelPlanCoordinates: travelPlanCoordinates
  }
};

export default connect(mapStateToProps, { getStops })(StopMap);