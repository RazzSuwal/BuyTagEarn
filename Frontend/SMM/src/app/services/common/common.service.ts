import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const TOSTER_TIME_OUT: number = 1500;
@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(
    private _toastr: ToastrService,
  ) { }

  successAlert(
    title: string,
    text: string,
    timeOut: number = TOSTER_TIME_OUT
  ): void {
    // return this.fireAlert({
    //   icon: 'success',
    //   title,
    //   text,
    // });
    this._toastr.success(text, title, { timeOut: timeOut });
  }

  showAlert(mes: string, type: number, timeOut: number = TOSTER_TIME_OUT) {
    if (type == 200 || type.toString() == "200") {
      this._toastr.success(mes == "" ? "Sucess.!" : mes, "Success", {
        timeOut: timeOut,
      });
    } else if (type == 400 || type.toString() == "400") {
      this._toastr.error(mes, "Error");
    } else {
      this._toastr.error(mes, "Error");
    }
  }

  warning(
    message: string,
    title: string = "Error",
    timeOut: number = TOSTER_TIME_OUT
  ) {
    this._toastr.warning(message, title, { timeOut: timeOut });
  }

  error(
    message: string,
    title: string = "Error",
    timeOut: number = TOSTER_TIME_OUT
  ) {
    this._toastr.warning(message, title, { timeOut: timeOut });
  }

  // openModal(id: string): void {
  //   ($(`#${id}`) as JQuery<HTMLElement>).modal('show');
  // }

  // closeModal(id: string): void {
  //   ($(`#${id}`) as JQuery<HTMLElement>).modal('hide');
  // }

}
