export default (state = null, action) => {
  switch (action.type) {
    case 'UPDATE_STOP_TIME':
      return action.payload;
    default:
      return state;
  }
};