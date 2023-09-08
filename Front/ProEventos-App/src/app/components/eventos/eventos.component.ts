import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '../../models/Evento';
import { EventoService } from '../../services/evento.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  modalRef = {} as BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public larguraImagem: number = 150;
  public margemImagem: number = 2;
  public exibirImagem: boolean = true;
  private filtroListado: string = '';

  public get filtroLista() : string {
    return this.filtroListado;
  }

  public set filtroLista(value: string){
    this.filtroListado = value;

    this.eventosFiltrados = this.filtroListado ? this.filtrarEventos(this.filtroListado) : this.eventos;
  }

  public filtrarEventos (filtrarPor: string) : Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();

    return this.eventos.filter(
      (evento : Evento) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
    ) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  }

  public alterarImagem(){
    this.exibirImagem = !this.exibirImagem;
  }

  public getEventos() : void{
    this.eventoService.getEventos().subscribe({
      next: (eventosResp: Evento[]) => {
        this.eventos = eventosResp;
        this.eventosFiltrados = this.eventos
      } ,
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os Eventos','Erro')
      },
      complete: () => this.spinner.hide()
  });
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm() : void {
    this.modalRef.hide();
    this.toastr.success('O Evento foi deletado com Sucesso!', 'Deletado!');
  }

  decline(): void {
    this.modalRef.hide();
  }
}
