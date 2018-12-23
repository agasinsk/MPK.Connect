import { combineReducers } from 'redux';

// Reducers

const viewReducer = () => {
  return [
    { value: 0, name: 'Trasa' },
    { value: 1, name: 'Rozkład jazdy' }
  ];
};

const selectedViewReducer = (selectedView = 0, action) => {
  if (action.type === 'VIEW_SELECTED') {
    return action.payload;
  }

  return selectedView;
};

export default combineReducers({
  views: viewReducer,
  selectedView: selectedViewReducer
});