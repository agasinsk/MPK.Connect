export default (state = { name: "Świdnicka", latitude: null, longitude: null }, action) => {
  switch (action.type) {
    case 'SET_DESTINATION':
      return action.payload;
    default:
      return state;
  }
};