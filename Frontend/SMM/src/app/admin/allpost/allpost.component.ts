import { Component } from '@angular/core';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { AdminService } from '../../services/admin/admin.service';
import { CommonService } from '../../services/common/common.service';


@Component({
  selector: 'app-allpost',
  templateUrl: './allpost.component.html',
  styleUrl: './allpost.component.scss'
})

export class AllpostComponent {
  dtOptions: Config={};
  dtTrigger: Subject<any> = new Subject<any>();
  posts!: any[];
  
  constructor(private _adminService: AdminService, private _commonservice: CommonService,) {
    
    
  }

  ngOnInit(): void {
    this.getUserPosts();
    this.dtOptions = {
      pagingType: 'full',
      // pageLength: 10
    };
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  getUserPosts(){
    this._adminService.getAllUserPost().subscribe(res => {
      this.posts= res;
      this.dtTrigger.next(null);
    });
    
  }
  approved(postId: number, IsApproved: number) {
    this._adminService.approved(postId, IsApproved).subscribe(
      (res) => {
        if (IsApproved == 1) {
          
          this._commonservice.successAlert("Success", "Approved SucessFully");
        }
        else{
          this._commonservice.successAlert("Success", "Cancel SucessFully"); 
        }
      },
      (err) => {
        this._commonservice.error("Error", err); 
      }
    );
  }
  

}
