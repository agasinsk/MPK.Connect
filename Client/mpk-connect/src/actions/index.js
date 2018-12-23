// Action Creators

export const selectView = view => {
  return {
    type: 'VIEW_SELECTED',
    payload: view,
  };
};