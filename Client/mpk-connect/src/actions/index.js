import mpkConnect from '../apis/mpkConnect';

export const selectView = view => {
  return {
    type: 'VIEW_SELECTED',
    payload: view,
  };
};

export const getStops = () => async dispatch => {
  const response = await mpkConnect.get('Stop/GetAll');

  dispatch({ type: 'GET_STOPS', payload: response.data })
};

export const selectSource = source => {
  return {
    type: 'SET_SOURCE',
    payload: source,
  };
};

export const selectDestination = destination => {
  return {
    type: 'SET_DESTINATION',
    payload: destination,
  };
};

export const setTravelOptions = (source, destination, date) => {
  const localDate = new Date(date);
  localDate.setHours(date.getHours() - date.getTimezoneOffset() / 60);
  console.log(localDate);
  return {
    type: 'SET_TRAVEL_OPTIONS',
    payload: { source, destination, startDate: localDate },
  };
};

export const findTravelPlan = (source, destination, date) => async (dispatch, getState) => {
  await dispatch(setTravelOptions(source, destination, date));
  const travelOptions = getState().travelOptions;
  dispatch(getTravelPlan(travelOptions));
};

export const getTravelPlan = (travelOptions) => async dispatch => {
  const response = await mpkConnect.post('TravelPlan', travelOptions);

  dispatch({ type: 'GET_TRAVEL_PLAN', payload: response.data })
};


