import { Component } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { LivrosDTO } from '../../core/models/Livros';

@Component({
  selector: 'app-livros',
  imports: [UserInfoComponent],
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.css'
})
export class LivrosComponent {
  livros: LivrosDTO[] = [];
  livrosEmprestados: LivrosDTO[] = [];
}
