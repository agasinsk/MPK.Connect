export default (selectedView = 0, action) => {
  if (action.type === 'VIEW_SELECTED') {
    return action.payload;
  }

  return selectedView;
};