export default (state = {}, action) => {
  switch (action.type) {
    case 'GET_TRAVEL_PLAN':
      return action.payload;
    default:
      return state;
  }
};