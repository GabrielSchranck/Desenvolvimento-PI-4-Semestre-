import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  imports: [ReactiveFormsModule],
  templateUrl: './text-input.component.html',
  styleUrl: './text-input.component.css'
})
export class TextInputComponent {
  @Input() label: string  = '';
  @Input() control: FormControl = new FormControl('');
  @Input() type: string = 'text';
  @Input() placeholder: string = '';
  @Input() name: string = '';
  @Input() required: boolean = false;
  @Input() error: string = '';
  @Input() src: string = '';
}
