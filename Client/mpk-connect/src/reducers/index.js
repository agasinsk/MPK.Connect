import { combineReducers } from 'redux';
import { snackbarReducer } from 'material-ui-snackbar-redux'

import selectedViewReducer from './selectedViewReducer';
import viewReducer from './viewReducer';
import stopsReducer from './stopsReducer';
import selectedDestinationReducer from './selectedDestinationReducer';
import selectedSourceReducer from './selectedSourceReducer';
import travelOptionsReducer from './travelOptionsReducer';
import travelPlanReducer from './travelPlanReducer';
import selectedTravelPlanReducer from './selectedTravelPlanReducer';
import timeTableReducer from './timeTableReducer';
import selectedStopReducer from './selectedStopReducer';
import updatedStopTimeReducer from './updatedStopTimeReducer';
import deletedStopTimeReducer from './deletedStopTimeReducer';
import selectedRouteReducer from './selectedRouteReducer';

export default combineReducers({
  views: viewReducer,
  selectedView: selectedViewReducer,
  stops: stopsReducer,
  selectedSource: selectedSourceReducer,
  selectedDestination: selectedDestinationReducer,
  travelOptions: travelOptionsReducer,
  travelPlan: travelPlanReducer,
  selectedTravelPlan: selectedTravelPlanReducer,
  timeTable: timeTableReducer,
  selectedStop: selectedStopReducer,
  updatedStopTime: updatedStopTimeReducer,
  deletedStopTime: deletedStopTimeReducer,
  selectedRoute: selectedRouteReducer,
  snackbar: snackbarReducer
});