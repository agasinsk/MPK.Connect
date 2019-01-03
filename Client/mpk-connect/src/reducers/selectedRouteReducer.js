export default (state = null, action) => {
  switch (action.type) {
    case 'SELECT_ROUTE':
      return action.payload;
    default:
      return state;
  }
};