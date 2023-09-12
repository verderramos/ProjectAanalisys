CREATE PROCEDURE Clienteincluir(
@Id varchar (50),
@Nome varchar (100),
@Email varchar (100),
@Telefone varchar (100),
@Endereco varchar (100)
)
as

INSERT INTO Cliente(Id,Nome,Email,Telefone,Endereco)
	VALUES(@Id, @Nome, @Email, @Telefone, @Endereco)
