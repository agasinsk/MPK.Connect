import './StopMap.css';
import React, { Component } from 'react';
import { Map, TileLayer, ZoomControl } from 'react-leaflet';
import { connect } from 'react-redux';

import { getStops } from '../actions';

const mapCenter = [51.12, 17.04];
const zoomLevel = 17;

export class StopMap extends Component {

  constructor(props) {
    super(props);
    this.state = {
      currentZoomLevel: zoomLevel,
    };
    this.handleUpPanClick = this.handleUpPanClick.bind(this);
    this.handleRightPanClick = this.handleRightPanClick.bind(this);
    this.handleLeftPanClick = this.handleLeftPanClick.bind(this);
    this.handleDownPanClick = this.handleDownPanClick.bind(this);
    //this.handleMapChange = this.handleMapChange.bind(this);
  }

  componentDidMount() {
    const leafletMap = this.leafletMap.leafletElement;
    leafletMap.on('zoomend', () => {
      const updatedZoomLevel = leafletMap.getZoom();
      this.handleZoomLevelChange(updatedZoomLevel);
    });

    // leafletMap.on('moveend ', () => {
    //   this.handleMapChange();
    // });

    this.props.getStops();
  }

  // handleMapChange() {
  //   //let filteredStops = this.filterStops(this.state.allStops);
  //   this.setState({
  //     visibleStops: filteredStops
  //   });
  // }

  handleZoomLevelChange(newZoomLevel) {
    this.setState({
      currentZoomLevel: newZoomLevel
    });
  }

  handleUpPanClick() {
    const leafletMap = this.leafletMap.leafletElement;
    leafletMap.panBy([0, -100]);
    window.console.log('Panning up');
  }

  handleRightPanClick() {
    const leafletMap = this.leafletMap.leafletElement;
    leafletMap.panBy([100, 0]);
    window.console.log('Panning right');
  }

  handleLeftPanClick() {
    const leafletMap = this.leafletMap.leafletElement;
    leafletMap.panBy([-100, 0]);
    window.console.log('Panning left');
  }

  handleDownPanClick() {
    const leafletMap = this.leafletMap.leafletElement;
    leafletMap.panBy([0, 100]);
    window.console.log('Panning down');
  }

  render() {
    return (
      <Map ref={m => { this.leafletMap = m; }} center={mapCenter} zoom={zoomLevel} zoomControl={false}>
        <ZoomControl position="bottomright" />
        <TileLayer
          attribution="&amp;copy <a href=&quot;http://osm.org/copyright&quot;>OpenStreetMap</a> contributors"
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
      </Map>
    );
  }
}

export default connect(null, { getStops })(StopMap);