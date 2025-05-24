import { Component, Input } from '@angular/core';
import { AbstractControl, FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-inputs',
  imports: [ReactiveFormsModule],
  templateUrl: './inputs.component.html',
  styleUrl: './inputs.component.css'
})
export class InputsComponent {
  @Input() label: string  = '';
  @Input() set control(value: AbstractControl | null) {
    this._control = value as FormControl;
  }
  get control(): FormControl {
    return this._control;
  }
  private _control: FormControl = new FormControl('');
  @Input() type: string = 'text';
  @Input() placeholder: string = '';
  @Input() name: string = '';
  @Input() required: boolean = false;
  @Input() error: string = '';
  @Input() src: string = '';
}
