import './StopMap.css';
import React, { Component } from 'react';
import { Map, TileLayer, Marker, Popup } from 'react-leaflet';
import Button from '@material-ui/core/Button';
import { TimeTable } from './TimeTable';

const mapCenter = [51.12, 17.04];
const zoomLevel = 15;

export class StopMap extends Component {

    constructor(props) {
        super(props);
        this.state = {
            currentZoomLevel: zoomLevel,
            allStops: [],
            visibleStops: [],
            timeTableOpened: false,
            currentStopId: '1000'
        };
        this.handleUpPanClick = this.handleUpPanClick.bind(this);
        this.handleRightPanClick = this.handleRightPanClick.bind(this);
        this.handleLeftPanClick = this.handleLeftPanClick.bind(this);
        this.handleDownPanClick = this.handleDownPanClick.bind(this);
        this.handleMapChange = this.handleMapChange.bind(this);
        this.toggleTimeTable = this.toggleTimeTable.bind(this);
        this.filterStops = this.filterStops.bind(this);

        fetch('api/Stop/GetAll')
            .then(response => response.json())
            .then(data => {
                console.log('Received ' + data.length + ' stops.')
                let filteredStops = this.filterStops(data);
                this.setState({
                    allStops: data,
                    visibleStops: filteredStops
                });
            });
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
    }

    handleMapChange() {
        let filteredStops = this.filterStops(this.state.allStops);
        this.setState({
            visibleStops: filteredStops
        });
    }

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

    toggleTimeTable() {
        let currentOpened = this.state.timeTableOpened;
        this.setState({
          timeTableOpened: !currentOpened,
        });
      };

    render() {
        window.console.log('this.state.currentZoomLevel ->',
            this.state.currentZoomLevel);

        return (
            <React.Fragment>
                <Button onClick={this.toggleTimeTable}>Open TimeTable</Button>
                <TimeTable open={this.state.timeTableOpened} onClose={this.toggleTimeTable} stopId={this.state.currentStopId}></TimeTable>
                <Map ref={m => { this.leafletMap = m; }} center={mapCenter} zoom={zoomLevel}>
                    <TileLayer
                        attribution="&amp;copy <a href=&quot;http://osm.org/copyright&quot;>OpenStreetMap</a> contributors"
                        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                    />
                    {this.state.visibleStops.map((stop) => {
                        let position = [stop.latitude, stop.longitude];
                        return <Marker key={`marker-${stop.id}`} position={position}>
                            <Popup>
                                <span>{stop.name}
                                    <br /> {stop.latitude}
                                    <br /> {stop.longitude}
                                </span>
                            </Popup>
                        </Marker>
                    }
                    )}
                </Map>
            </React.Fragment>
        );
    }
}