import { combineReducers } from 'redux';

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
import selectedStopTimeReducer from './selectedStopTimeReducer';

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
  selectedStopTime: selectedStopTimeReducer
});