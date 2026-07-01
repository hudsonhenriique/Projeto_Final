       IDENTIFICATION DIVISION.
       PROGRAM-ID. CLIENT-QUERY.
       AUTHOR. HUDSON HENRIQUE.
      *--------------------------------------------------------------*
      * OBJETIVO: LER REQUISICAO TXT, CONSULTAR KSDS E GERAR TXT     *
      *--------------------------------------------------------------*

       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
      * Arquivo de entrada gerado pelo C#
           SELECT REQ-FILE ASSIGN TO "../data/req-consulta.txt"
               ORGANIZATION IS LINE SEQUENTIAL.

      * Meu banco de dados 
           SELECT CLIENT-FILE ASSIGN TO "../data/clientes.dat"
               ORGANIZATION IS INDEXED
               ACCESS MODE IS RANDOM
               RECORD KEY IS CLI-ID
               FILE STATUS IS WS-FILE-STATUS.

      * Arquivo de saida lido pelo C#
           SELECT RESP-FILE ASSIGN TO "../data/resp-consulta.txt"
               ORGANIZATION IS LINE SEQUENTIAL.

       DATA DIVISION.
       FILE SECTION.
       FD  REQ-FILE.
       COPY "../../shared/req-consulta.cpy".

       FD  CLIENT-FILE.
       01  CLIENT-RECORD.
           05 CLI-ID               PIC 9(06).
           05 CLI-NAME             PIC X(30).
           05 CLI-PHONE            PIC X(15).
           05 CLI-EMAIL            PIC X(40).

       FD  RESP-FILE.
       COPY "../../shared/resp-consulta.cpy".

       WORKING-STORAGE SECTION.
       01  WS-FILE-STATUS          PIC X(02) VALUE SPACES.
       01  WS-EOF-REQ              PIC X(01) VALUE 'N'.

       PROCEDURE DIVISION.
       0000-MAIN.
           PERFORM 1000-INIT.
           PERFORM 2000-PROCESS-QUERY.
           PERFORM 3000-END.
           STOP RUN.

       1000-INIT.
      * Prepara os arquivos para leitura e escrita
           OPEN INPUT REQ-FILE.
           OPEN INPUT CLIENT-FILE.
           OPEN OUTPUT RESP-FILE.

       2000-PROCESS-QUERY.
      * Processa a requisicao (stateless - uma por execucao)
           READ REQ-FILE
               AT END MOVE 'Y' TO WS-EOF-REQ.
           
           IF WS-EOF-REQ = 'N'
               PERFORM 2100-PROCESSAR-CLIENTE.

       2100-PROCESSAR-CLIENTE.
      * Tenta localizar o cliente no banco de dados
           MOVE REQ-CONS-CODIGO-CLIENTE TO CLI-ID.
           READ CLIENT-FILE
               INVALID KEY
                   PERFORM 2110-CLIENTE-NAO-ENCONTRADO
               NOT INVALID KEY
                   PERFORM 2120-CLIENTE-ENCONTRADO.
         
           WRITE AREA-RESPOSTA-CONSULTA.

       2110-CLIENTE-NAO-ENCONTRADO.
           MOVE "1" TO RESP-CONS-STATUS.
           MOVE REQ-CONS-CODIGO-CLIENTE TO RESP-CONS-CODIGO-CLIENTE.
           MOVE SPACES TO RESP-CONS-NOME-CLIENTE.
           MOVE SPACES TO RESP-CONS-TELEFONE.
           MOVE SPACES TO RESP-CONS-EMAIL.
           MOVE "CLIENT NOT FOUND" TO RESP-CONS-MENSAGEM.

       2120-CLIENTE-ENCONTRADO.
           MOVE "0" TO RESP-CONS-STATUS.
           MOVE CLI-ID TO RESP-CONS-CODIGO-CLIENTE.
           MOVE CLI-NAME TO RESP-CONS-NOME-CLIENTE.
           MOVE CLI-PHONE TO RESP-CONS-TELEFONE.
           MOVE CLI-EMAIL TO RESP-CONS-EMAIL.
           MOVE "SUCCESS" TO RESP-CONS-MENSAGEM.

       3000-END.
           CLOSE REQ-FILE.
           CLOSE CLIENT-FILE.
           CLOSE RESP-FILE.
