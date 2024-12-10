import 'package:flutter/material.dart';
import '../models/equipamento.dart';
import '../services/api_service.dart';

class EquipamentoProvider with ChangeNotifier {
  List<Equipamento> _equipamentos = [];
  bool _equipamentosCarregados = false;  // Flag para verificar se já foram carregados
  bool _isLoading = false; // Flag para controlar o carregamento
  final ApiService apiService = ApiService();

  List<Equipamento> get equipamentos => _equipamentos;

  // Getter público para _equipamentosCarregados
  bool get equipamentosCarregados => _equipamentosCarregados;

  // Getter para isLoading
  bool get isLoading => _isLoading;

  // Método para carregar os equipamentos, mas apenas se ainda não forem carregados
  Future<void> carregarEquipamentos() async {
    if (_equipamentosCarregados) return; // Não carrega se já foi carregado

    _isLoading = true; // Inicia o carregamento
    notifyListeners(); // Notifica os ouvintes para atualizar a UI com o estado de carregamento

    try {
      _equipamentos = await apiService.fetchEquipamentos();
      _equipamentosCarregados = true; // Marca como carregado
    } catch (error) {
      print("Erro ao carregar equipamentos: $error");
    } finally {
      _isLoading = false; // Finaliza o carregamento
      notifyListeners(); // Notifica os ouvintes para atualizar a UI após o carregamento
    }
  }

  // Método para adicionar um equipamento
  void adicionarEquipamento(Equipamento equipamento) {
    _equipamentos.add(equipamento);
    notifyListeners();
  }

  // Método para filtrar equipamentos por ID do estoque
  List<Equipamento> equipamentosPorEstoque(int estoqueId) {
    return _equipamentos.where((eqp) => eqp.estoqueId == estoqueId).toList();
  }

  // Método para buscar um equipamento por ID
  Equipamento? buscarEquipamentoPorId(String id) {
    try {
      return _equipamentos.firstWhere((eqp) => eqp.id.toString() == id);
    } catch (e) {
      return null;
    }
  }
}
