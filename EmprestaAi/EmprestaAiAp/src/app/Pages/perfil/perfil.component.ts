import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ClienteService } from '../../core/services/cliente-service.service';
import { Cliente, EnderecoCliente } from '../../core/models/Cliente';
import Swal from 'sweetalert2';
import { EnderecoService } from '../../core/services/endereco.service';
import { Endereco } from '../../core/models/Endereco';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-perfil',
  imports: [UserInfoComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent implements OnInit{

  formularioPerfil!: FormGroup;
  formularioEndereco!: FormGroup;
  cliente: Cliente = new Cliente();
  enabled: boolean = true;
  enderecos: EnderecoCliente[] = [];
  cep: string = "";
  editarPerfil: boolean = false;
  deletarEndereco: boolean = false;
  modalAberto: boolean = false;
  possuiEnderecos: boolean = false;
  adicionaEndereco: boolean = false;

  constructor(private formBuilder: FormBuilder, private clienteService: ClienteService, private enderecoService: EnderecoService){}

  ngOnInit(): void {

    this.CreateFormPerfil();
    this.CreateFormEndereco();

    if(this.cliente.Nome !== undefined){
      this.formularioPerfil.patchValue(this.cliente);
    }else{
      this.GetUserData();
    }
  }

  public editarEndereco(endereco: EnderecoCliente): void {
    this.modalAberto = true;
    this.editarPerfil = true;
    this.formularioEndereco.patchValue(endereco);
  }

  public abrirModalEndereco(endereco: any = null) {
    this.modalAberto = true;
    this.editarPerfil = !!endereco;

    if (endereco) {
      this.formularioEndereco.patchValue(endereco);
    } else {
      this.formularioEndereco.reset();
    }
  }

  public fecharModalEndereco() {
    this.modalAberto = false;
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
      id: [''],
      enderecoId: [''],
      cep: [''],
      numero: [''],
      rua: [''],
      complemento: [''],
      bairro: [''],
      cidade: [''],
      uf: ['']
    });
  }

  private GetUserData(): void {
    this.clienteService.GetByToken().subscribe({
      next: (dados) => {
        this.cliente = dados.cliente;
        this.enderecos = dados.enderecos;

        if (this.cliente.DataNascimento) {
          this.cliente.DataNascimento = this.cliente.DataNascimento.split("T")[0];
        }

        this.formularioPerfil.patchValue(this.cliente);
      },
      error: (error) => {
        console.error("Erro ao selecionar clientes:", error);
      }
    });
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
                this.clienteService.UpdateClient(clienteEditado).subscribe(
                  (sucesso) => {
                    Swal.fire({
                      title: "Sucesso!",
                      text: "Os dados do cliente foram atualizados.",
                      icon: "success",
                      confirmButtonText: "Ok"
                    });
                    this.editarPerfil = false;
                    this.GetUserData();
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

  public DeletarEndereco(enderecoCliente: Endereco): void{
    Swal.fire({
      title: "Tem certeza?",
      text: "Deseja realmente excluir o endereço?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Sim, excluir!",
      cancelButtonText: "Cancelar"
    }).then((result) => {
      if (result.isConfirmed) {
        this.enderecoService.DeleteEnderecoCliente(enderecoCliente).subscribe({
          next: (retorno) => {
            Swal.fire({
              title: "Sucesso!",
              text: "O endereço foi excluído.",
              icon: "success",
              confirmButtonText: "Ok"
            });
            this.GetUserData();
          },
          error: (error) => {
            Swal.fire({
              title: "Erro!",
              text: "Erro ao comunicar com o servidor.",
              icon: "error",
              confirmButtonText: "Ok"
            });
            console.error("Erro ao excluir endereço:", error);
          }
        });
      }
    });
  }

  public OperacoesEndereco(): void {
    const enderecoCliente: Endereco = this.formularioEndereco?.value;

    if(this.editarPerfil){
      Swal.fire({
        title: "Tem certeza?",
        text: "Deseja realmente alterar os dados do endereço?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sim, editar!",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            this.enderecoService.UpdateEnderecoCliente(enderecoCliente).subscribe(
              (sucesso) => {
                Swal.fire({
                  title: "Sucesso!",
                  text: "Os dados do endereço foram atualizados.",
                  icon: "success",
                  confirmButtonText: "Ok"
                });
                this.fecharModalEndereco();
                this.GetUserData();
              },

              (erro) => {
                console.log("Erro ao atualizar endereço" + erro)
                this.editarPerfil = false;
              }
            );
        }
    });
    }
    else{
      this.enderecoService.CreateEnderecoCliente(enderecoCliente).subscribe({
        next: (retorno) => {
          Swal.fire({
            title: "Sucesso!",
            text: "O endereço foi cadastrado.",
            icon: "success",
            confirmButtonText: "Ok"
          });
          this.modalAberto = false;
          this.GetUserData();
        },
        error: (error) => {
          Swal.fire({
            title: "Erro!",
            text: "Erro ao comunicar com o servidor.",
            icon: "error",
            confirmButtonText: "Ok"
          });
          this.modalAberto = false;
          console.error("Erro ao cadastrar endereço:", error);
        }
      });
    }

  }

  public async getEnderecoViaCep(): Promise<void> {

    this.cep = this.formularioEndereco?.get("cep")?.value ;

    if(this.cep === "") return;

    (await this.enderecoService.GetEnderecoByViaCep(this.cep)).subscribe(
      (response) => {
        const enderecoResult: Endereco = {
          cep: response.cep,
          rua: response.rua,
          bairro: response.bairro,
          cidade: response.cidade,
          uf: response.uf
        };

        this.formularioEndereco.patchValue({
          cep: enderecoResult.cep,
          rua: enderecoResult.rua,
          bairro: enderecoResult.bairro,
          cidade: enderecoResult.cidade,
          uf: enderecoResult.uf
        });
      },

      (error) => {
        console.log("Error " + error)
      }
    );
  }

  public EditarClick(): void{
    this.editarPerfil = !this.editarPerfil;
    this.enabled = !this.enabled;
  }

  public adicionaEnderecoClick(): void{

    this.formularioEndereco.patchValue({
      cep: "",
      rua: "",
      bairro: "",
      complemento: "",
      numero: "",
      cidade: "",
      uf: ""
    });

    this.adicionaEndereco = !this.adicionaEndereco;
  }
}
