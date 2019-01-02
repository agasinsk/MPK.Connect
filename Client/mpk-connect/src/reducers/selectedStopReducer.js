export default (state = null, action) => {
  switch (action.type) {
    case 'SELECT_STOP':
      return action.payload;
    default:
      return state;
  }
};