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

export const findTravelPlan = (source, destination, date) => async (dispatch, getState) => {
  await dispatch(selectTravelPlan(null));
  await dispatch(clearTravelPlan());

  await dispatch(setTravelOptions(source, destination, date));
  const travelOptions = getState().travelOptions;
  await dispatch(getTravelPlan(travelOptions));
  const travelPlans = getState().travelPlan;
  if (travelPlans !== null) {
    await dispatch(selectTravelPlan(travelPlans[0]));
  }
};

export const setTravelOptions = (source, destination, date) => {

  const localDate = new Date(date);
  localDate.setHours(date.getHours() - date.getTimezoneOffset() / 60);
  return {
    type: 'SET_TRAVEL_OPTIONS',
    payload: { source, destination, startDate: localDate },
  };
};

export const getTravelPlan = (travelOptions) => async dispatch => {
  await mpkConnect.post('TravelPlan', travelOptions)
    .then(response => {
      dispatch({ type: 'GET_TRAVEL_PLAN', payload: response.data })
    })
    .catch(error => {
      console.log(error.message);
      dispatch({ type: 'GET_TRAVEL_PLAN', payload: "ERROR" })
    });
};

export const selectTravelPlan = travelPlan => {
  return {
    type: 'SELECT_TRAVEL_PLAN',
    payload: travelPlan,
  };
};

export const clearTravelPlan = () => {
  return {
    type: 'CLEAR_TRAVEL_PLAN'
  };
};

export const getTimeTable = (stopId) => async dispatch => {
  await mpkConnect.get('TimeTable/' + stopId)
    .then(response => {
      dispatch({ type: 'GET_TIMETABLE', payload: response.data })
    })
    .catch(error => {
      console.log(error.message);
      dispatch({ type: 'GET_TIMETABLE', payload: "ERROR" })
    });
};


export const selectStop = stop => {
  return {
    type: 'SELECT_STOP',
    payload: stop,
  };
};