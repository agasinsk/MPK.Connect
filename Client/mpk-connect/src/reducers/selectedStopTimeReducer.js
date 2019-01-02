export default (state = null, action) => {
  switch (action.type) {
    case 'SELECT_STOP_TIME':
      return action.payload;
    case 'UPDATE_STOP_TIME':
      return action.payload;
    case 'DELETE_STOP_TIME':
      return action.payload;
    default:
      return state;
  }
};