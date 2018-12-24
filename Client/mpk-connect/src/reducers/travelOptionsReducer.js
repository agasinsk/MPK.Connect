export default (state = {}, action) => {
  switch (action.type) {
    case 'SET_TRAVEL_OPTIONS':
      return action.payload;
    default:
      return state;
  }
};