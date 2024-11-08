import { AfterViewInit, Component } from '@angular/core';
import { FormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user/user.service';
import { CommonService } from '../../services/common/common.service';
import { Router } from '@angular/router';
import { BrandService } from '../../services/brand/brand.service';

@Component({
  selector: 'app-main-form',
  templateUrl: './main-form.component.html',
  styleUrls: ['./main-form.component.scss'],
})
export class MainFormComponent implements AfterViewInit {
  postForm: UntypedFormGroup;
  requestData!: any[];
  brandList!: any[];
  private animating = false;
  userID !: any;
  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private _commonservice: CommonService,
    private router: Router,
    private _brandService: BrandService,
  ) {
    this.postForm = fb.group({
      brandName: fb.control('', [Validators.required]),
      imageUrl: fb.control('', [Validators.required]),
      productName: fb.control('', [Validators.required]),
      postUrl: fb.control('', [Validators.required]),
      postedOn: fb.control('', [Validators.required]),
      isTag: fb.control('', [Validators.required]),
    });
  }

  ngOnInit(): void {
    this.getAllBrand();
  }
  ngAfterViewInit(): void {
    const fieldsets = Array.from(
      document.querySelectorAll('fieldset')
    ) as HTMLFieldSetElement[];
    const progressbarItems = Array.from(
      document.querySelectorAll('#progressbar li')
    );

    document.querySelectorAll('.next').forEach((button) => {
      button.addEventListener('click', () => {
        debugger;
        console.log('Next button clicked');
        if (this.animating) return;
        this.animating = true;

        const current_fs = button.parentElement as HTMLFieldSetElement;
        const next_fs = current_fs.nextElementSibling as HTMLFieldSetElement;

        if (!next_fs) return;

        const nextIndex = fieldsets.indexOf(next_fs);
        progressbarItems[nextIndex].classList.add('active');

        this.animateFieldsets(current_fs, next_fs, true);
      });
    });

    document.querySelectorAll('.previous').forEach((button) => {
      button.addEventListener('click', () => {
        if (this.animating) return;
        this.animating = true;

        const current_fs = button.parentElement as HTMLFieldSetElement;
        const previous_fs =
          current_fs.previousElementSibling as HTMLFieldSetElement;

        if (!previous_fs) return;

        const currentIndex = fieldsets.indexOf(current_fs);
        progressbarItems[currentIndex].classList.remove('active');
        debugger;
        this.animateFieldsets(current_fs, previous_fs, false);
      });
    });
  }

  private animateFieldsets(
    current_fs: HTMLFieldSetElement,
    target_fs: HTMLFieldSetElement,
    isNext: boolean
  ) {
    const duration = 800;
    let startTime: number;
    debugger;
    const animate = (timestamp: number) => {
      if (!startTime) startTime = timestamp;

      const progress = (timestamp - startTime) / duration;

      if (progress < 1) {
        this.stepAnimation(current_fs, target_fs, progress, isNext);
        requestAnimationFrame(animate);
      } else {
        this.stepAnimation(current_fs, target_fs, 1, isNext);
        this.completeAnimation(current_fs, target_fs, isNext);
      }
    };

    requestAnimationFrame(animate);
  }

  private stepAnimation(
    current_fs: HTMLFieldSetElement,
    target_fs: HTMLFieldSetElement,
    progress: number,
    isNext: boolean
  ) {
    const scale = isNext
      ? 1 - (1 - progress) * 0.2
      : 0.8 + (1 - progress) * 0.2;
    const left = isNext ? `${progress * 50}%` : `${(1 - progress) * 50}%`;
    const opacity = 1 - progress;

    if (isNext) {
      current_fs.style.transform = `scale(${scale})`;
      current_fs.style.position = 'absolute';
      // target_fs.style.left = left;
      target_fs.style.opacity = `${opacity}`;
    } else {
      // current_fs.style.left = left;
      current_fs.style.transform = `scale(${scale})`;
      current_fs.style.position = 'absolute';
      // target_fs.style.left = left;
      target_fs.style.opacity = `${opacity}`;
    }
  }

  private completeAnimation(
    current_fs: HTMLFieldSetElement,
    target_fs: HTMLFieldSetElement,
    isNext: boolean
  ) {
    if (isNext) {
      current_fs.classList.add('hidden');
      current_fs.classList.remove('visible');
      target_fs.classList.remove('hidden');
      target_fs.classList.add('visible');
    } else {
      current_fs.classList.add('hidden');
      current_fs.classList.remove('visible');
      target_fs.classList.remove('hidden');
      target_fs.classList.add('visible');
    }

    this.animating = false;
  }

  submit() {
    let data = {
      brandName: this.postForm.get('brandName')?.value,
      productName: this.postForm.get('productName')?.value,
      imageUrl: this.postForm.get('imageUrl')?.value,
      postUrl: this.postForm.get('postUrl')?.value,
      postedOn: this.postForm.get('postedOn')?.value,
      isTag: this.postForm.get('isTag')?.value,
    };
    this.userService.userPost(data).subscribe({
      next: (res) => {
        // this.open(res, 'OK');
        this.router.navigate(['/dashboard']);
        this._commonservice.successAlert('Success', 'Submit SuccessFul!');
      },
    });
  }

  getAllProductById(userId: any) {
    this._brandService.getAllProductById(userId).subscribe((res) => {
      this.requestData = res.data.filter((item: any) => item.IsApproved === true);
    });
  }

  getAllBrand(){
    this._brandService.getAllBrand().subscribe((res) => {
      this.brandList = res.data;
    });
  }

  OnChange($event: any): void {
    if ($event) {
      const id = isNaN(+($event.Id)) ? $event.Id : +($event.Id);
      this.userID = id;
      debugger
      this.postForm.controls["productName"].reset();
      this.getAllProductById(this.userID);
    }
  }
}
