import mpkConnect from '../apis/mpkConnect';
import { snackbarActions as snackbar } from 'material-ui-snackbar-redux';

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

export const getTimeTable = (stop) => async dispatch => {
  await dispatch(selectStop(stop));
  await mpkConnect.get('TimeTable/' + stop.id)
    .then(response => {
      if (response.data.length === 0) {
        dispatch(snackbar.show({
          message: 'Nie można znaleźć połączeń między wybranymi przystankami.',
          action: 'Zamknij'
        }));
      }
      dispatch({ type: 'GET_TIMETABLE', payload: response.data })
    })
    .catch(error => {
      console.log(error.message);
      dispatch(snackbar.show({
        message: error.message,
        action: 'Zamknij'
      }));
      dispatch({ type: 'GET_TIMETABLE', payload: "ERROR" })
    });
};

export const selectStop = stop => {
  return {
    type: 'SELECT_STOP',
    payload: stop,
  };
};

export const selectStopTime = stopTime => {
  return {
    type: 'SELECT_STOP_TIME',
    payload: stopTime,
  };
};

export const selectRoute = route => {
  return {
    type: 'SELECT_ROUTE',
    payload: route,
  };
};

export const unselectRoute = () => {
  return {
    type: 'SELECT_ROUTE',
    payload: null,
  };
};


export const updateStopTime = (stopTime) => async dispatch => {
  await mpkConnect.put('StopTime', stopTime)
    .then(response => {
      dispatch(snackbar.show({
        message: response.data.text,
        action: 'Zamknij'
      }));
      dispatch({ type: 'UPDATE_STOP_TIME', payload: response.data });
    })
    .catch(error => {
      dispatch(snackbar.show({
        message: error.message,
        action: 'Zamknij'
      }));
      console.log(error.message);
      dispatch({ type: 'UPDATE_STOP_TIME', payload: error });
    });
};


export const deleteStopTime = (stopTimeId) => async dispatch => {
  await mpkConnect.delete('StopTime/' + stopTimeId)
    .then(response => {
      dispatch(snackbar.show({
        message: response.data.text,
        action: 'Zamknij'
      }));
      dispatch({ type: 'DELETE_STOP_TIME', payload: response.data });
    })
    .catch(error => {
      console.log(error.message);
      dispatch(snackbar.show({
        message: error.message,
        action: 'Zamknij'
      }));
      dispatch({ type: 'DELETE_STOP_TIME', payload: error });
    });
};