import './StopMap.css';
import React, { Component } from 'react';
import { Map, TileLayer, ZoomControl, Marker, Popup, Polyline } from 'react-leaflet';
import { connect } from 'react-redux';

import { getStops } from '../actions';

const mapCenter = [51.105, 17.035];
const zoomLevel = 15;

export class StopMap extends Component {

  constructor(props) {
    super(props);

    this.state = {
      currentZoomLevel: zoomLevel,
      visibleStops: props.allStops
    };
    this.handleMapChange = this.handleMapChange.bind(this);
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
    let filteredStops = this.filterStops(this.props.allStops);
    this.setState({
      visibleStops: filteredStops
    });
  }

  filterStops(stops) {
    let bounds = this.leafletMap.leafletElement.getBounds();
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

  render() {
    console.log('this.state.currentZoomLevel ->', this.state.currentZoomLevel);
    return (
      <Map ref={m => { this.leafletMap = m; }} center={mapCenter} zoom={zoomLevel} zoomControl={false}>
        <ZoomControl position="bottomright" />
        <TileLayer
          attribution="&amp;copy <a href=&quot;http://osm.org/copyright&quot;>OpenStreetMap</a> contributors"
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        {this.state.visibleStops.map((stop) => {
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
        }
        )}
        <Polyline color="lime" positions={this.props.travelPlanCoordinates} />
      </Map>
    );
  }
}

const mapStateToProps = (state) => {

  const travelPlan = state.travelPlan;
  var travelPlanCoordinates = [];
  if (travelPlan !== undefined && travelPlan.Comfortable !== undefined) {
    const firstTravelPlan = travelPlan.Comfortable[0];
    travelPlanCoordinates = firstTravelPlan.stops.map(stop => [stop.stopInfo.latitude, stop.stopInfo.longitude]);
  }

  return {
    allStops: state.stops,
    travelPlanCoordinates: travelPlanCoordinates
  }
};

export default connect(mapStateToProps, { getStops })(StopMap);