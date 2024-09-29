import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  debugger;
  const token = localStorage.getItem('authToken');
  const cloneRequest = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });
  return next(cloneRequest);
};
