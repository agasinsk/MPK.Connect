export default (state = null, action) => {
  switch (action.type) {
    case 'SELECT_TRAVEL_PLAN':
      if (action.payload !== undefined) {
        return action.payload;
      }
      else {
        return state;
      }
    default:
      return state;
  }
};