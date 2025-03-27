import { Endereco } from './../../core/models/Cliente';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { cpf } from 'cpf-cnpj-validator';
import { Cliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente.service';
import { EnderecoService } from '../../core/services/endereco.service';

@Component({
  selector: 'app-perfil',
  imports: [ReactiveFormsModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent implements OnInit {

  formularioPerfil: FormGroup = new FormGroup({});
  formularioEndereco: FormGroup = new FormGroup({});
  cliente: Cliente = new Cliente();
  clienteLocal: any;
  editarPerfil: boolean = false;
  dadosCarregados: boolean = false;
  adicionaEndereco: boolean = false;
  possuiEnderecos: boolean = false;

  constructor(private clienteService: ClienteService, private enderecoService: EnderecoService) {
    this.clienteLocal = JSON.parse(localStorage.getItem("clienteDatas") || '{}');
    this.criaFormulario();
  }

  async ngOnInit(): Promise<void> {

    if(this.clienteLocal && JSON.stringify(this.clienteLocal) !== '{}'){
      this.cliente = this.clienteLocal.cliente;
      this.atualizarFormulario();
    }
    else{
      this.dadosCarregados = false;
      await this.getPerfil();
      this.atualizarFormulario();
    }
    this.dadosCarregados = true;
  }
  private criaFormulario(): void {
    this.formularioPerfil = new FormGroup({
      nome: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      cpf: new FormControl(''),
      dataNascimento: new FormControl(''),
      contato: new FormControl(''),
    });

    this.formularioEndereco = new FormGroup({
      cep: new FormControl('', [Validators.required]),
      rua: new FormControl('', [Validators.required]),
      numero: new FormControl('', [Validators.required]),
      complemento: new FormControl(''),
      bairro: new FormControl('', [Validators.required]),
      cidade: new FormControl('', [Validators.required]),
      estado: new FormControl('', [Validators.required]),
    });
  }
  private async getPerfil(): Promise<void> {
    this.clienteService.GetByToken().subscribe({
      next: (retorno) => {
        if(retorno){
          localStorage.setItem("clienteDatas", JSON.stringify(retorno));
        }
        else{
          console.log("Erro ao obter cliente");
        }
      },
      error: (erro) => {
        console.log("Erro ao conectar a API: " + erro);
      }
    })
  }
  private atualizarFormulario(): void {
    if (!this.clienteLocal) return;

    console.log(this.clienteLocal.enderecos);

    this.formularioPerfil.patchValue({
      nome: this.clienteLocal.cliente.nome || '',
      email: this.clienteLocal.cliente.email || '',
      cpf: this.clienteLocal.cliente.cpf || '',
      dataNascimento: this.clienteLocal.cliente.dataNascimento ? this.clienteLocal.cliente.dataNascimento.split('T')[0] : '',
      contato: this.clienteLocal.cliente.contato || '',
    });

    if(this.clienteLocal.enderecos.length > 0){
      this.possuiEnderecos = true;

    }
  }
  public adicionaEnderecoClick(): void {
    this.adicionaEndereco = !this.adicionaEndereco;
  }
  public async createEndereco(): Promise<void> {
    const endereco: Endereco = this.formularioEndereco.value;

    console.log("Endereco: " + endereco)

    this.enderecoService.createEndereco(endereco).subscribe({
        next: (resposta) => {
            if (!this.cliente.Enderecos) {
                this.cliente.Enderecos = [];
            }
            this.cliente.Enderecos.push(resposta.endereco);
        },
        error: (erro) => {
            console.error("Erro ao criar endereço:", erro);
        }
    });
}

}

