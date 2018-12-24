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

export const filterStops = (stops, bounds) => {
  let visibleStops = stops.filter(function (stop) {
    return stop.latitude < bounds._northEast.lat
      && stop.latitude > bounds._southWest.lat
      && stop.longitude < bounds._northEast.lng
      && stop.longitude > bounds._southWest.lng;
  });

  return {
    type: 'FILTER_STOP',
    payload: visibleStops,
  };
};

export const selectSource = source => {
  return {
    type: 'VIEW_SELECTED',
    payload: source,
  };
};

export const selectDestination = destination => {
  return {
    type: 'VIEW_SELECTED',
    payload: destination,
  };
};