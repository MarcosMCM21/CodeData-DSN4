class Equipamento {
  int? id;
  String? codigo;
  String? modelo;
  String? descricao;
  String? marca;
  String? serialNumber;
  String? partNumber;
  bool condicao;
  int? estoqueId;
  String? nomeEstoque;
  int? documentoId;
  int? tipoSolicitacao;
  DateTime? dataInicio;
  DateTime? dataFim;
  String? nomeCliente;
  String? firstName;
  String? lastName;
  DateTime? dataCadastro;
  String? userName;
  String? email;

  Equipamento({
    this.id,
    this.codigo,
    this.modelo,
    this.descricao,
    this.marca,
    this.serialNumber,
    this.partNumber,
    required this.condicao,
    this.estoqueId,
    this.nomeEstoque,
    this.documentoId,
    this.tipoSolicitacao,
    this.dataInicio,
    this.dataFim,
    this.nomeCliente,
    this.firstName,
    this.lastName,
    this.dataCadastro,
    this.userName,
    this.email,

  });

  factory Equipamento.fromJson(Map<String, dynamic> json) {
    final dataInicio =
        json['dataInicio'] != null ? DateTime.parse(json['dataInicio']) : null;
    final dataFim =
        json['dataFim'] != null ? DateTime.parse(json['dataFim']) : null;
    final dataCadastro =
        json['DataCadastro'] != null ? DateTime.parse(json['DataCadastro']) : null;

    print('Data Início: $dataInicio');
    print('Data Fim: $dataFim');



    return Equipamento(
      id: json['Id'],
      codigo: json['Codigo']?.toString(),
      modelo: json['Modelo']?.toString(),
      descricao: json['Descricao']?.toString(),
      marca: json['Marca']?.toString(),
      serialNumber: json['SerialNumber']?.toString(),
      partNumber: json['PartNumber']?.toString(),
      condicao: json['Condicao'] == 1,
      estoqueId: json['EstoqueId'],
      nomeEstoque: json['NomeEstoque']?.toString(),
      documentoId: json['DocumentoId'],
      tipoSolicitacao: json['tipoSolicitacao'],
      dataInicio: dataInicio,
      dataFim: dataFim,
      nomeCliente: json['nomeCliente']?.toString(),
      firstName: json['FirstName']?.toString(),
      lastName: json['LastName']?.toString(),
      dataCadastro: dataCadastro,
      userName: json['UserName']?.toString(),
      email: json['Email']?.toString(),

    );
  }

  Map<String, dynamic> toJson() {
    return {
      'Id': id,
      'Codigo': codigo,
      'Modelo': modelo,
      'Descricao': descricao,
      'Marca': marca,
      'SerialNumber': serialNumber,
      'PartNumber': partNumber,
      'Condicao': condicao ? 1 : 0,
      'EstoqueId': estoqueId,
      'NomeEstoque': nomeEstoque,
      'DocumentoId': documentoId,
      'tipoSolicitacao': tipoSolicitacao,
      'DataInicio': dataInicio?.toIso8601String(),
      'DataFinal': dataFim?.toIso8601String(),
      'nomeCliente': nomeCliente,
      'FirstName': firstName,
      'LastName': lastName,
      'DataCadastro': dataCadastro?.toIso8601String(),
      'UserName': userName,
      'Email': email,

    };
  }
}

class Documento {
  int? id;
  String? nome;
  String? anexo;
  String? tipo;

  Documento({
    this.id,
    this.nome,
    this.anexo,
    this.tipo,
  });

  factory Documento.fromJson(Map<String, dynamic> json) {
    return Documento(
      id: json['Id'],
      nome: json['Nome']?.toString(),
      anexo: json['Anexo']?.toString(),
      tipo: json['Tipo']?.toString(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'Id': id,
      'Nome': nome,
      'Anexo': anexo,
      'Tipo': tipo,
    };
  }
} 