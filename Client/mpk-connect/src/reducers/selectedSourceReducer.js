export default (state = { name: "Kwiska", latitude: null, longitude: null }, action) => {
  switch (action.type) {
    case 'SET_SOURCE':
      return action.payload;
    default:
      return state;
  }
};