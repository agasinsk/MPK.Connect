export default (state = null, action) => {
  switch (action.type) {
    case 'GET_TIMETABLE':
      return action.payload;
    default:
      return state;
  }
};