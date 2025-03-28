import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ClienteService } from '../../core/services/cliente-service.service';
import { Cliente } from '../../core/models/Cliente';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-perfil',
  imports: [UserInfoComponent, ReactiveFormsModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent implements OnInit{
  
  formularioPerfil!: FormGroup;
  formularioEndereco!: FormGroup;
  cliente: Cliente = new Cliente();
  editarPerfil: boolean = false;
  possuiEnderecos: boolean = false;
  adicionaEndereco: boolean = false;

  constructor(private formBuilder: FormBuilder, private clienteService: ClienteService){}
  
  ngOnInit(): void {
    
    this.CreateFormPerfil();
    this.CreateFormEndereco();

    if(this.cliente.Nome !== undefined){
      this.formularioPerfil.patchValue(this.cliente);
    }else{
      this.GetUserData();
    }
  }

  private CreateFormPerfil(): void{
    this.formularioPerfil = this.formBuilder.group({
      nome: [''],
      cpf: [''],
      email: [''],
      dataNascimento: [''],
      contato: ['']
    });
  }

  private CreateFormEndereco(){
    this.formularioEndereco = this.formBuilder.group({
      cep: [''],
      numero: [''],
      rua: [''],
      complemento: [''],
      bairro: [''],
      cidade: [''],
      uf: ['']
    });
  }

  private GetUserData(): void{
    this.clienteService.GetByToken().subscribe(
      (dados) => {
        this.cliente = dados.cliente;
        this.formularioPerfil.patchValue(this.cliente);
      },

      (error) => {
        console.log("Erro ao selecionar clientes " + error);
      }
    );
  }

  public EditarCliente(): void {
    const clienteEditado: Cliente = this.formularioPerfil?.value;

    if (JSON.stringify(clienteEditado) !== JSON.stringify(this.cliente)) {
        Swal.fire({
            title: "Tem certeza?",
            text: "Deseja realmente alterar os dados do cliente?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sim, editar!",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                // Aqui você salva os dados
                this.clienteService.UpdateClient(clienteEditado).subscribe(
                  (sucesso) => {
                    Swal.fire({
                      title: "Sucesso!",
                      text: "Os dados do cliente foram atualizados.",
                      icon: "success",
                      confirmButtonText: "Ok"
                    });
                    this.editarPerfil = false;
                  },

                  (erro) => {
                    console.log("Erro ao atualizar cliente" + erro)
                    this.editarPerfil = false;
                  }
                );
            }
        });
      }
  }

  public createEndereco(): void{
    
  }

  public EditarClick(): void{
    this.editarPerfil = !this.editarPerfil;
  }

  public adicionaEnderecoClick(): void{
    this.adicionaEndereco = !this.adicionaEndereco;
  }
}
