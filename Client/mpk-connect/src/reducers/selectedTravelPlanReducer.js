export default (state = null, action) => {
  switch (action.type) {
    case 'SELECT_TRAVEL_PLAN':
      return action.payload;
    default:
      return state;
  }
};