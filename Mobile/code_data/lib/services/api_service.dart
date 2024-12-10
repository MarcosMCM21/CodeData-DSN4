import 'dart:typed_data'; // Importar o pacote correto
import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:path_provider/path_provider.dart'; // Para manipular diretórios no dispositivo
import 'dart:io';
import '../../models/equipamento.dart'; // Certifique-se de importar o modelo Equipamento

class ApiService {
  final String baseUrl = "http://localhost:3000";

  // Método para login do usuário
  Future<bool> loginUser(String firstName, String passwordHash) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/login'),
        headers: {'Content-Type': 'application/json'},
        body: json.encode({
          'FirstName': firstName,
          'PasswordHash': passwordHash,
        }),
      );

      if (response.statusCode == 200) {
        return true;
      } else if (response.statusCode == 401) {
        print('Credenciais inválidas');
        return false;
      } else {
        throw Exception('Erro ao autenticar usuário: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Erro ao conectar à API: $e');
    }
  }

  // Método para buscar todos os equipamentos
  Future<List<Equipamento>> fetchEquipamentos() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/equipamentos'));

      if (response.statusCode == 200) {
        List<dynamic> jsonResponse = json.decode(response.body);
        return jsonResponse.map((data) => Equipamento.fromJson(data)).toList();
      } else {
        throw Exception('Falha ao carregar equipamentos: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Erro ao conectar à API: $e');
    }
  }

  // Método para adicionar um novo equipamento
  Future<void> adicionarEquipamento(Equipamento equipamento) async {
    final response = await http.post(
      Uri.parse('$baseUrl/equipamentos'),
      headers: {'Content-Type': 'application/json'},
      body: json.encode(equipamento.toJson()),
    );

    if (response.statusCode != 200) {
      throw Exception('Falha ao adicionar equipamento');
    }
  }

  // Método para buscar a URL do documento
  Future<String?> fetchDocumentoUrl(String documentoId) async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/documento/$documentoId'));

      if (response.statusCode == 200) {
        return response.body; // Retorna a URL do documento
      } else {
        throw Exception('Erro ao carregar a URL do documento');
      }
    } catch (e) {
      throw Exception('Erro ao conectar à API: $e');
    }
  }

  // Método para buscar o usuário atual
  Future<Map<String, dynamic>> fetchUsuarioAtual() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/usuario/atual'));

      if (response.statusCode == 200) {
        return json.decode(response.body);
      } else {
        throw Exception('Falha ao buscar usuário atual: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Erro ao conectar à API: $e');
    }
  }

  // Método para salvar documento localmente
  Future<void> salvarDocumentoLocalmente(Uint8List documentBytes) async {
    try {
      final tempDir = await getTemporaryDirectory();
      final tempFile = File('${tempDir.path}/documento.pdf');
      await tempFile.writeAsBytes(documentBytes);
      print('Documento salvo em: ${tempFile.path}');
    } catch (e) {
      print('Erro ao salvar o documento: $e');
    }
  }
}
