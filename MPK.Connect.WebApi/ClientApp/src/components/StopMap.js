import './StopMap.css';
import React, { Component } from 'react';
import { Map, TileLayer, Marker, Popup } from 'react-leaflet';
import Control from 'react-leaflet-control';
import { Button } from 'react-bootstrap';

const mapCenter = [51.12, 17.04];
const zoomLevel = 15;

export class StopMap extends Component {

    constructor(props) {
        super(props);
        this.state = {
            currentZoomLevel: zoomLevel,
            stops: []
        };
        this.handleUpPanClick = this.handleUpPanClick.bind(this);
        this.handleRightPanClick = this.handleRightPanClick.bind(this);
        this.handleLeftPanClick = this.handleLeftPanClick.bind(this);
        this.handleDownPanClick = this.handleDownPanClick.bind(this);

        fetch('api/Stop/GetAll')
            .then(response => response.json())
            .then(data => {
                console.log('Received data of stops')
                this.setState({ stops: data });
            });
    }

    componentDidMount() {
        const leafletMap = this.leafletMap.leafletElement;
        leafletMap.on('zoomend', () => {
            const updatedZoomLevel = leafletMap.getZoom();
            this.handleZoomLevelChange(updatedZoomLevel);
        });
    }

    handleZoomLevelChange(newZoomLevel) {
        this.setState({ currentZoomLevel: newZoomLevel });
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
        window.console.log('this.state.currentZoomLevel ->',
            this.state.currentZoomLevel);

        return (
            <Map ref={m => { this.leafletMap = m; }} center={mapCenter} zoom={zoomLevel}>
                <TileLayer
                    attribution="&amp;copy <a href=&quot;http://osm.org/copyright&quot;>OpenStreetMap</a> contributors"
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                <Control position="topright">
                    <div
                        style={{
                            backgroundColor: 'black',
                            padding: '5px',
                        }}
                    >
                        <div style={{ marginLeft: '37px' }}>
                            <Button bsStyle="primary" onClick={this.handleUpPanClick}>UP</Button>
                        </div>
                    </div>
                </Control>
                {this.state.stops.map((stop) => {
                    let position = [stop.latitude, stop.longitude];
                    return <Marker key={`marker-${stop.id}`} position={position}>
                        <Popup>
                            <span>{stop.name} <br /> {stop.latitude}
                            <br/> {stop.longitude }
                            </span>
                        </Popup>
                    </Marker>
                }
                )}
            </Map >
        )
    }
}