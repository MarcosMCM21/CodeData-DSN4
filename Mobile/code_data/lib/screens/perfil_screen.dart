import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:intl/intl.dart';

class PerfilScreen extends StatefulWidget {
  @override
  _PerfilScreenState createState() => _PerfilScreenState();
}

class _PerfilScreenState extends State<PerfilScreen> {
  final storage = FlutterSecureStorage();
  String? firstName;
  String? lastName;
  String? email;
  String? userName;
  DateTime? dataCadastro;

  @override
  void initState() {
    super.initState();
    _loadUserData();
  }

  Future<void> _loadUserData() async {
    userName = await storage.read(key: 'username');

    if (userName != null) {
      final url = Uri.parse('http://localhost:3000/users/$userName');

      try {
        final response = await http.get(url);

        if (response.statusCode == 200) {
          final data = json.decode(response.body);

          setState(() {
            firstName = data['FirstName'];
            lastName = data['LastName']; // Cargo
            email = data['Email'];
            if (data['DataCadastro'] != null) {
              dataCadastro = DateTime.parse(data['DataCadastro']);
            }
          });
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('Erro ao carregar os dados do usuário')),
          );
        }
      } catch (e) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Erro ao conectar ao servidor: $e')),
        );
      }
    }
  }

  Future<void> _logout() async {
    await storage.deleteAll();
    Navigator.pushReplacementNamed(
        context, '/'); // Redireciona para a tela de login
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: CustomScrollView(
        slivers: [
          SliverAppBar(
            expandedHeight: 80.0,
            floating: false,
            pinned: false,
            flexibleSpace: FlexibleSpaceBar(
              background: Container(
                color: Colors.transparent,
              ),
            ),
          ),
          SliverList(
            delegate: SliverChildListDelegate([
              Padding(
                padding: EdgeInsets.all(16.0),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    _buildProfileHeader(),
                    SizedBox(height: 20),
                    _buildProfileDetailCard(
                      icon: Icons.person,
                      title: 'Nome',
                      content: firstName ?? 'Nome não disponível',
                    ),
                    SizedBox(height: 10),
                    _buildProfileDetailCard(
                      icon: Icons.badge,
                      title: 'Cargo',
                      content: lastName ?? 'Cargo não disponível',
                    ),
                    SizedBox(height: 10),
                    _buildProfileDetailCard(
                      icon: Icons.email,
                      title: 'Email',
                      content: email ?? 'Email não disponível',
                    ),
                    SizedBox(height: 10),
                    _buildProfileDetailCard(
                      icon: Icons.account_circle,
                      title: 'Usuário',
                      content: userName ?? 'Usuário não disponível',
                    ),
                    SizedBox(height: 10),
                    _buildProfileDetailCard(
                      icon: Icons.calendar_today,
                      title: 'Data de Cadastro',
                      content: dataCadastro != null
                          ? DateFormat('dd/MM/yyyy').format(dataCadastro!)
                          : 'Data não disponível',
                    ),
                    Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: ElevatedButton(
                        onPressed: _logout, // Chama o método de logout
                        style: ElevatedButton.styleFrom(
                          padding: EdgeInsets.symmetric(vertical: 16),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(10),
                          ),
                          backgroundColor:
                              Colors.red, // Cor vermelha para o botão
                        ),
                        child: Text(
                          'Logout',
                          style: TextStyle(fontSize: 18, color: Colors.white),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ]),
          ),
        ],
      ),
    );
  }

  Widget _buildProfileHeader() {
    IconData cargoIcon = Icons.person; // Ícone padrão
    Color cargoColor = Colors.grey; // Cor padrão

    if (lastName == 'Gerente') {
      cargoIcon = Icons.work_outline;
      cargoColor = Colors.blue;
    } else if (lastName == 'Vendedor') {
      cargoIcon = Icons.shopping_cart;
      cargoColor = Colors.green;
    }

    return Container(
      padding: EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Color(0xFF0E6600),
        borderRadius: BorderRadius.circular(10),
      ),
      child: Column(
        children: [
          CircleAvatar(
            radius: 50,
            backgroundColor: Colors.white,
            child: Icon(
              cargoIcon,
              color: cargoColor,
              size: 50,
            ),
          ),
          SizedBox(height: 10),
          Text(
            firstName ?? 'Usuário',
            style: TextStyle(
              fontSize: 22,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
          Text(
            lastName ?? 'Cargo não disponível',
            style: TextStyle(
              fontSize: 16,
              fontWeight: FontWeight.w500,
              color: Colors.white70,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildProfileDetailCard(
      {required IconData icon,
      required String title,
      required String content}) {
    return Card(
      elevation: 3,
      shadowColor: Colors.grey[200],
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(10),
      ),
      child: Padding(
        padding: EdgeInsets.all(16),
        child: Row(
          children: [
            Icon(
              icon,
              color: Color(0xFF0E6600),
              size: 30,
            ),
            SizedBox(width: 16),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    title,
                    style: TextStyle(
                      fontSize: 14,
                      fontWeight: FontWeight.bold,
                      color: Colors.grey[600],
                    ),
                  ),
                  SizedBox(height: 5),
                  Text(
                    content,
                    style: TextStyle(
                      fontSize: 16,
                      color: Colors.black87,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
