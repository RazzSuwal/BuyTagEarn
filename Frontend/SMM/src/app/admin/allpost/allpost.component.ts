import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { AdminService } from '../../services/admin/admin.service';
import { CommonService } from '../../services/common/common.service';
import { AuthserviceService } from '../../services/auth/authservice.service';
import { DataTableDirective } from 'angular-datatables';

@Component({
  selector: 'app-allpost',
  templateUrl: './allpost.component.html',
  styleUrl: './allpost.component.scss',
})
export class AllpostComponent implements OnInit, AfterViewInit, OnDestroy{
  @ViewChild(DataTableDirective, { static: false }) dtElement: DataTableDirective | undefined;
  dtOptions: any = {};
  dtTrigger: Subject<any> = new Subject<any>();
  posts!: any[];
  userId: any;

  constructor(
    private _adminService: AdminService,
    private _commonservice: CommonService,
    private _authService: AuthserviceService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.userId = this._authService.getUserID();
    this.dtOptions = {
      pagingType: 'full',
      pageLength: 10,
      processing: true,
      serverSide: false,
      retrieve: true,
      destroy: true,
      language: {
        search: '_INPUT_',
        searchPlaceholder: 'Search records',
      },
      scrollCollapse: false,
    };
    this.getUserPosts();
  }
  ngAfterViewInit(): void {
    // Trigger DataTable initialization only after the view has been initialized
    // this.dtTrigger.next(null);
  }
  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  getUserPosts() {
    this._adminService.getAllUserPost(this.userId).subscribe((res) => {
      this.posts = res;
      this.dtTrigger.next(null);
      this.cdr.detectChanges();
    });
  }
  approved(postId: number, IsApproved: number) {
    this._adminService.approved(postId, IsApproved).subscribe(
      (res) => {
        if (IsApproved == 1) {
          this._commonservice.successAlert('Success', 'Approved SucessFully');
        } else {
          this._commonservice.successAlert('Success', 'Cancel SucessFully');
        }
        this.getUserPosts();
      },
      (err) => {
        this._commonservice.error('Error', err);
      }
    );
  }



}
