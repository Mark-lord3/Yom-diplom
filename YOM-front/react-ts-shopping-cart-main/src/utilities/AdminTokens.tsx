
const TOKEN_KEY = 'idsughWIEUFGHJBDSJFSDFBwskejfbsd1243E[p[';

export const getToken = (): string | null => {
   return sessionStorage.getItem(TOKEN_KEY);
};

export const setToken = (token: string): void => {
  sessionStorage.setItem(TOKEN_KEY, token);
};

export const removeToken = (): void => {
  sessionStorage.removeItem(TOKEN_KEY);
};
