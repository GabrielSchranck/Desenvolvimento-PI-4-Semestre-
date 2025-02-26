import { Component } from '@angular/core';
import { UseracessComponent } from "../../components/useracess/useracess.component";
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [UseracessComponent, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

}
