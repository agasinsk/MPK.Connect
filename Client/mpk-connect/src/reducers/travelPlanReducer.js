export default (state = null, action) => {
  switch (action.type) {
    case 'GET_TRAVEL_PLAN':
      return action.payload;
    case 'CLEAR_TRAVEL_PLAN':
      return null;
    default:
      return state;
  }
};