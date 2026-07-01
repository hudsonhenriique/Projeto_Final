       IDENTIFICATION DIVISION.
       PROGRAM-ID. CLIENT-UPDATE.
       AUTHOR. HUDSON HENRIQUE.
      *--------------------------------------------------------------*
      * OBJETIVO: LER REQUISICAO TXT, ATUALIZAR KSDS E GERAR TXT     *
      * COMPATIVEL COM PADRAO ANSI-74                                *
      *--------------------------------------------------------------*

       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
      * Arquivo de entrada 
           SELECT REQ-FILE ASSIGN TO "../data/req-atualizacao.txt"
               ORGANIZATION IS LINE SEQUENTIAL.

      * Banco de dados aberto 
           SELECT CLIENT-FILE ASSIGN TO "../data/clientes.dat"
               ORGANIZATION IS INDEXED
               ACCESS MODE IS RANDOM
               RECORD KEY IS CLI-ID
               FILE STATUS IS WS-FILE-STATUS.

      * Arquivo de resposta 
           SELECT RESP-FILE ASSIGN TO "../data/resp-atualizacao.txt"
               ORGANIZATION IS LINE SEQUENTIAL.

       DATA DIVISION.
       FILE SECTION.
       FD  REQ-FILE.
       COPY "../../shared/req-atualizacao.cpy".

       FD  CLIENT-FILE.
       01  CLIENT-RECORD.
           05 CLI-ID               PIC 9(06).
           05 CLI-NAME             PIC X(30).
           05 CLI-PHONE            PIC X(15).
           05 CLI-EMAIL            PIC X(40).

       FD  RESP-FILE.
       COPY "../../shared/resp-atualizacao.cpy".

       WORKING-STORAGE SECTION.
       01  WS-FILE-STATUS          PIC X(02) VALUE SPACES.
       01  WS-EOF-REQ              PIC X(01) VALUE 'N'.

       PROCEDURE DIVISION.
       0000-MAIN.
           PERFORM 1000-INIT.
           PERFORM 2000-PROCESS-UPDATE.
           PERFORM 3000-END.
           STOP RUN.

       1000-INIT.
           OPEN INPUT REQ-FILE.
           OPEN I-O CLIENT-FILE.
           OPEN OUTPUT RESP-FILE.

       2000-PROCESS-UPDATE.
           READ REQ-FILE
               AT END MOVE 'Y' TO WS-EOF-REQ.
           
           IF WS-EOF-REQ = 'N'
               PERFORM 2100-PROCESSAR-CLIENTE.

       2100-PROCESSAR-CLIENTE.
           MOVE REQ-ATUA-CODIGO-CLIENTE TO CLI-ID.
           READ CLIENT-FILE
               INVALID KEY
                   PERFORM 2110-CLIENTE-NAO-ENCONTRADO
               NOT INVALID KEY
                   PERFORM 2120-ATUALIZAR-REGISTRO.
         
           WRITE AREA-RESPOSTA-ATUALIZACAO.

       2110-CLIENTE-NAO-ENCONTRADO.
           MOVE "1" TO RESP-ATUA-STATUS.
           MOVE "CLIENTE NAO ENCONTRADO NO BANCO DE DADOS" 
             TO RESP-ATUA-MENSAGEM.

       2120-ATUALIZAR-REGISTRO.
      * Move apenas os dados que podem ser alterados
           MOVE REQ-ATUA-NOVO-TELEFONE TO CLI-PHONE.
           MOVE REQ-ATUA-NOVO-EMAIL TO CLI-EMAIL.
           
      * REWRITE substitui o dado do arquivo utilizando a chave primaria
           REWRITE CLIENT-RECORD
               INVALID KEY
                   MOVE "9" TO RESP-ATUA-STATUS
                   MOVE "ERRO INTERNO AO ATUALIZAR REGISTRO" 
                     TO RESP-ATUA-MENSAGEM
               NOT INVALID KEY
                   MOVE "0" TO RESP-ATUA-STATUS
                   MOVE "CLIENTE ATUALIZADO COM SUCESSO" 
                     TO RESP-ATUA-MENSAGEM.

       3000-END.
           CLOSE REQ-FILE.
           CLOSE CLIENT-FILE.
           CLOSE RESP-FILE.
           