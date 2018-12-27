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
  await dispatch(getTravelPlan(travelOptions));
  const travelPlans = getState().travelPlan;
  await dispatch(selectTravelPlan(travelPlans));
};

export const getTravelPlan = (travelOptions) => async dispatch => {
  mpkConnect.post('TravelPlan', travelOptions)
    .then(response => {
      console.log(response.data);
      dispatch({ type: 'GET_TRAVEL_PLAN', payload: response.data })
    })
    .catch(error => {
      console.log(error.message);
      dispatch({ type: 'GET_TRAVEL_PLAN', payload: "ERROR" })
    });


};

export const selectTravelPlan = travelPlan => {
  var selectedTravelPlan = null;
  if (travelPlan !== null && travelPlan !== undefined) {
    selectedTravelPlan = travelPlan[0];
  }

  return {
    type: 'SELECT_TRAVEL_PLAN',
    payload: selectedTravelPlan,
  };
};


