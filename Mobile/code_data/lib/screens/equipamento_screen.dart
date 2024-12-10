import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../providers/equipamento_provider.dart';

class EquipamentoScreen extends StatefulWidget {
  @override
  _EquipamentoScreenState createState() => _EquipamentoScreenState();
}

class _EquipamentoScreenState extends State<EquipamentoScreen> {
  String _searchQuery = "";

  @override
  void initState() {
    super.initState();
    // Carregar equipamentos apenas se ainda não foram carregados
    final provider = Provider.of<EquipamentoProvider>(context, listen: false);
    if (!provider.equipamentosCarregados) {
      provider.carregarEquipamentos();
    }
  }

  @override
  Widget build(BuildContext context) {
    final provider = Provider.of<EquipamentoProvider>(context);
    // Filtra apenas equipamentos que não estão associados a nenhuma solicitação
    final equipamentos = provider.equipamentos
        .where((equipamento) => equipamento.tipoSolicitacao == null)
        .toList();

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
                'EQUIPAMENTOS EM ESTOQUE',
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
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            // Barra de pesquisa
            Padding(
              padding: const EdgeInsets.symmetric(
                  horizontal: 16.0), // Ajuste de padding
              child: TextField(
                onChanged: (value) {
                  setState(() {
                    _searchQuery = value.toLowerCase();
                  });
                },
                decoration: InputDecoration(
                  hintText: 'Pesquisar... ',
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
                      vertical: 12.0), // Menos padding vertical
                ),
                style: TextStyle(
                  fontSize: 16.0,
                ),
              ),
            ),
            SizedBox(height: 20),
            // Verifica se os equipamentos estão sendo carregados
            provider.isLoading
                ? Expanded(
                    child: Center(
                      child: CircularProgressIndicator(),
                    ),
                  )
                : Expanded(
                    child: equipamentos.isEmpty
                        ? Center(
                            child: Text(
                              'Nenhum equipamento encontrado.',
                              style: TextStyle(fontSize: 18, color: Colors.grey),
                            ),
                          )
                        : ListView.builder(
                            itemCount: equipamentos.length,
                            itemBuilder: (ctx, i) {
                              final equipamento = equipamentos[i];
                              // Verifica a pesquisa
                              if (_searchQuery.isEmpty ||
                                  (equipamento.modelo
                                          ?.toLowerCase()
                                          .contains(_searchQuery) ??
                                      false)) {
                                return Card(
                                  elevation: 4,
                                  margin: EdgeInsets.symmetric(vertical: 10.0),
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(15.0),
                                  ),
                                  child: Padding(
                                    padding: const EdgeInsets.all(16.0),
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.center,
                                      children: [
                                        // Nome do equipamento centralizado
                                        Text(
                                          equipamento.modelo?.isNotEmpty == true
                                              ? equipamento.modelo!
                                              : 'Modelo não disponível',
                                          textAlign: TextAlign.center,
                                          style: TextStyle(
                                            fontWeight: FontWeight.bold,
                                            fontSize: 18,
                                            color: Color(0xFF0E6600),
                                          ),
                                        ),
                                        SizedBox(height: 10),
                                        // Informações restantes
                                        Column(
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: [
                                            Text(
                                                'Código: ${equipamento.codigo ?? 'N/D'}',
                                                style: TextStyle(
                                                    color: Colors.grey[700])),
                                            Text(
                                                'Marca: ${equipamento.marca ?? 'N/D'}',
                                                style: TextStyle(
                                                    color: Colors.grey[700])),
                                            Text(
                                                'Serial: ${equipamento.serialNumber ?? 'N/D'}',
                                                style: TextStyle(
                                                    color: Colors.grey[700])),
                                          ],
                                        ),
                                        SizedBox(height: 10),
                                        // Nome do estoque
                                        Text(
                                          'Estoque: ${equipamento.nomeEstoque ?? 'Nome do estoque não disponível'}',
                                          textAlign: TextAlign.center,
                                          style: TextStyle(
                                            color: Colors.grey[700],
                                            fontWeight: FontWeight.bold,
                                          ),
                                        ),
                                      ],
                                    ),
                                  ),
                                );
                              } else {
                                return Container(); // Retorna vazio se não corresponder à pesquisa
                              }
                            },
                          ),
                  ),
          ],
        ),
      ),
    );
  }
}
