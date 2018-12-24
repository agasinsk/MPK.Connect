import { combineReducers } from 'redux';

import selectedViewReducer from './selectedViewReducer';
import viewReducer from './viewReducer';
import stopsReducer from './stopsReducer';

export default combineReducers({
  views: viewReducer,
  selectedView: selectedViewReducer,
  stops: stopsReducer
});