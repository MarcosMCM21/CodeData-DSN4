import 'package:flutter/material.dart';
import '../models/equipamento.dart';
import '../services/api_service.dart';
import 'package:intl/intl.dart';

class HomologacaoScreen extends StatefulWidget {
  @override
  _HomologacaoScreenState createState() => _HomologacaoScreenState();
}

class _HomologacaoScreenState extends State<HomologacaoScreen> {
  List<Equipamento> equipamentosHomologados = [];
  Map<String, List<Equipamento>> equipamentosPorCliente = {}; // Mapa para agrupar por cliente
  String pesquisa = "";
  bool isLoading = true; // Controla o estado de carregamento

  Future<void> buscarEquipamentosHomologados() async {
    try {
      // Busca todos os equipamentos da API
      List<Equipamento> lista = await ApiService().fetchEquipamentos();

      setState(() {
        // Filtra equipamentos homologados (tipo 0)
        equipamentosHomologados = lista
            .where((equipamento) => equipamento.tipoSolicitacao == 0)
            .toList();

        // Agrupar os equipamentos por cliente
        equipamentosPorCliente = {};
        for (var equipamento in equipamentosHomologados) {
          final cliente = equipamento.nomeCliente ?? 'Cliente não identificado';
          if (!equipamentosPorCliente.containsKey(cliente)) {
            equipamentosPorCliente[cliente] = [];
          }
          equipamentosPorCliente[cliente]!.add(equipamento);
        }

        // Após o carregamento dos dados, altera o estado para não estar mais carregando
        isLoading = false;
      });

      if (equipamentosHomologados.isEmpty) {
        print('Nenhum equipamento homologado disponível.');
      }
    } catch (error) {
      setState(() {
        isLoading = false; // Caso ocorra erro, o carregamento é finalizado
      });
      print("Erro ao buscar equipamentos homologados: $error");
    }
  }

  List<Equipamento> aplicarPesquisa() {
    return equipamentosHomologados.where((equipamento) {
      final cliente = (equipamento.nomeCliente ?? '').toLowerCase();
      final modelo = (equipamento.modelo ?? '').toLowerCase();
      final termo = pesquisa.toLowerCase();

      return cliente.contains(termo) || modelo.contains(termo);
    }).toList();
  }

  @override
  void initState() {
    super.initState();
    buscarEquipamentosHomologados();
  }

  @override
  Widget build(BuildContext context) {
    final equipamentosFiltrados = aplicarPesquisa();
    final equipamentosAgrupados = <String, List<Equipamento>>{};

    // Agrupar os equipamentos filtrados por cliente
    for (var equipamento in equipamentosFiltrados) {
      final cliente = equipamento.nomeCliente ?? 'Cliente não identificado';
      if (!equipamentosAgrupados.containsKey(cliente)) {
        equipamentosAgrupados[cliente] = [];
      }
      equipamentosAgrupados[cliente]!.add(equipamento);
    }

    return Scaffold(
      appBar: PreferredSize(
        preferredSize:
            Size.fromHeight(90), // Defina uma altura fixa para a AppBar
        child: Container(
          color: Color(0xFF0E6600),
          child: Padding(
            padding: const EdgeInsets.only(
                top: 50), // Ajuste o espaçamento de forma consistente
            child: Align(
              alignment: Alignment.center,
              child: Text(
                'EQUIPAMENTOS HOMOLOGADOS',
                style: TextStyle(
                  fontWeight: FontWeight.bold,
                  fontSize: 24,
                  color: Colors.white,
                ),
              ),
            ),
          ),
        ),
      ),
      body: Column(
        children: [
          // Barra de pesquisa estilizada
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: TextField(
              onChanged: (value) {
                setState(() {
                  pesquisa = value;
                });
              },
              decoration: InputDecoration(
                hintText: 'Pesquisar por cliente ou modelo',
                filled: true,
                fillColor: Colors.grey[200], // Fundo cinza claro
                border: OutlineInputBorder(
                  borderRadius:
                      BorderRadius.circular(30.0), // Bordas arredondadas
                  borderSide: BorderSide.none, // Remove a borda padrão
                ),
                prefixIcon: Icon(
                  Icons.search,
                  color: Colors.grey[600], // Cor do ícone
                ),
                contentPadding: const EdgeInsets.symmetric(
                    vertical: 15.0), // Ajusta o tamanho interno
              ),
              style: TextStyle(
                fontSize: 16.0,
              ),
            ),
          ),
          // Se estiver carregando, exibe a barra de carregamento
          isLoading
              ? Expanded(
                  child: Center(
                    child: CircularProgressIndicator(),
                  ),
                )
              : Expanded(
                  child: equipamentosAgrupados.isEmpty
                      ? Center(
                          child: Text(
                            'Nenhum equipamento homologado disponível.',
                            style: TextStyle(fontSize: 18, color: Colors.grey),
                          ),
                        )
                      : ListView.builder(
                          itemCount: equipamentosAgrupados.keys.length,
                          itemBuilder: (ctx, i) {
                            final cliente = equipamentosAgrupados.keys.elementAt(i);
                            final equipamentos = equipamentosAgrupados[cliente]!;

                            return Card(
                              margin: EdgeInsets.symmetric(vertical: 8, horizontal: 16),
                              child: Padding(
                                padding: const EdgeInsets.all(16.0),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      cliente,
                                      style: TextStyle(
                                        fontWeight: FontWeight.bold,
                                        fontSize: 18,
                                        color: Colors.blueGrey,
                                      ),
                                    ),
                                    Divider(height: 20, thickness: 1),
                                    ...equipamentos.map((equipamento) {
                                      return Column(
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: [
                                          SizedBox(height: 8), // Espaçamento entre equipamentos
                                          Center(
                                            child: Text(
                                              equipamento.modelo ?? 'Modelo não disponível',
                                              style: TextStyle(
                                                fontWeight: FontWeight.bold,
                                                fontSize: 16,
                                              ),
                                            ),
                                          ),
                                          SizedBox(height: 8),
                                          Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: [
                                              Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: [
                                                  Text('Serial: ${equipamento.serialNumber ?? 'Serial não disponível'}'),
                                                  Text('Código: ${equipamento.codigo ?? 'Código não disponível'}'),
                                                ],
                                              ),
                                              Column(
                                                crossAxisAlignment: CrossAxisAlignment.end,
                                                children: [
                                                  Text('Marca: ${equipamento.marca ?? 'Marca não disponível'}'),
                                                ],
                                              ),
                                            ],
                                          ),
                                          SizedBox(height: 8),
                                          Row(
                                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                            children: [
                                              Text('Início: ${equipamento.dataInicio != null ? DateFormat('dd/MM/yyyy').format(equipamento.dataInicio!) : 'Data não disponível'}'),
                                              Text('Fim: ${equipamento.dataFim != null ? DateFormat('dd/MM/yyyy').format(equipamento.dataFim!) : 'Data não disponível'}'),
                                            ],
                                          ),
                                          Divider(
                                            height: 20,
                                            thickness: 1.5,
                                            color: Colors.grey[300],
                                          ),
                                        ],
                                      );
                                    }).toList(),
                                  ],
                                ),
                              ),
                            );
                          },
                        ),
                ),
        ],
      ),
    );
  }
}
