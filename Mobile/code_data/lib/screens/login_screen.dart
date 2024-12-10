import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class LoginScreen extends StatefulWidget {
  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _usernameController = TextEditingController();
  final _passwordController = TextEditingController();
  final _formKey = GlobalKey<FormState>();
  final storage = FlutterSecureStorage();

  bool _rememberMe = false;
  bool _isPasswordVisible = false;

  @override
  void initState() {
    super.initState();
    _loadCredentials();
  }

  // Carrega as credenciais salvas, se houver
  Future<void> _loadCredentials() async {
    String savedUsername = await storage.read(key: 'username') ?? '';
    String savedPassword = await storage.read(key: 'password') ?? '';

    setState(() {
      _usernameController.text = savedUsername;
      _passwordController.text = savedPassword;
      _rememberMe = savedPassword.isNotEmpty;
    });
  }

  // Função de login
  Future<void> loginUser() async {
    final username = _usernameController.text.trim();
    final password = _passwordController.text.trim();

    final url = Uri.parse(
        'https://codedata-connection-b3g2d0e4dkcrdzcp.brazilsouth-01.azurewebsites.net/api/authentication/login');
    final headers = {'Content-Type': 'application/json; charset=utf-8'};
    final body = json.encode({"Username": username, "Password": password});

    try {
      final response = await http.post(url, headers: headers, body: body);

      if (response.statusCode == 200) {
        final data = json.decode(response.body);

        if (data.containsKey('token')) {
          await storage.write(key: 'token', value: data['token']);
          await storage.write(key: 'username', value: username);

          // Salva a senha se "lembrar senha" estiver marcado
          if (_rememberMe) {
            await storage.write(key: 'password', value: password);
          } else {
            await storage.delete(
                key: 'password'); // Limpa a senha se não for para lembrar
          }

          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('Login bem-sucedido!')),
          );
          Navigator.pushReplacementNamed(context, '/home');
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('Token não encontrado')),
          );
        }
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Credenciais inválidas')),
        );
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro ao conectar ao servidor: $e')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Container(
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [
              Color.fromARGB(255, 20, 87, 24),
              Color.fromARGB(255, 36, 112, 36),
            ],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ),
        ),
        child: Center(
          child: SingleChildScrollView(
            child: Padding(
              padding: const EdgeInsets.all(24.0),
              child: Form(
                key: _formKey,
                child: Container(
                  padding: EdgeInsets.all(24),
                  decoration: BoxDecoration(
                    color: Colors.white,
                    borderRadius: BorderRadius.circular(16),
                    boxShadow: [
                      BoxShadow(
                        color: Colors.black.withOpacity(0.1),
                        blurRadius: 15,
                        offset: Offset(0, 10),
                      ),
                    ],
                  ),
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      // Logotipo
                      Image.asset(
                        'img/imgCodeData.png', // Substitua pelo caminho da sua imagem
                        height: 120,
                      ),
                      SizedBox(height: 20),
                      Text(
                        'Faça login para continuar',
                        style: TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                          color: Colors.black54,
                        ),
                      ),
                      SizedBox(height: 30),
                      // Campos de texto
                      TextFormField(
                        controller: _usernameController,
                        decoration: InputDecoration(
                          labelText: 'Usuário',
                          prefixIcon: Icon(Icons.person),
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(12),
                          ),
                          focusedBorder: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(12),
                            borderSide: BorderSide(
                              color: Colors.greenAccent,
                              width: 2.0,
                            ),
                          ),
                        ),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Por favor, insira o nome de usuário';
                          }
                          return null;
                        },
                      ),
                      SizedBox(height: 16),
                      TextFormField(
                        controller: _passwordController,
                        obscureText: !_isPasswordVisible,
                        decoration: InputDecoration(
                          labelText: 'Senha',
                          prefixIcon: Icon(Icons.lock),
                          suffixIcon: IconButton(
                            icon: Icon(
                              _isPasswordVisible
                                  ? Icons.visibility
                                  : Icons.visibility_off,
                            ),
                            onPressed: () {
                              setState(() {
                                _isPasswordVisible = !_isPasswordVisible;
                              });
                            },
                          ),
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(12),
                          ),
                          focusedBorder: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(12),
                            borderSide: BorderSide(
                              color: Colors.greenAccent,
                              width: 2.0,
                            ),
                          ),
                        ),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Por favor, insira a senha';
                          }
                          return null;
                        },
                      ),
                      SizedBox(height: 24),
                      // Checkbox "Lembrar senha"
                      Row(
                        children: [
                          Checkbox(
                            value: _rememberMe,
                            onChanged: (bool? value) {
                              setState(() {
                                _rememberMe = value!;
                              });
                            },
                          ),
                          Text(
                            'Lembrar senha',
                            style: TextStyle(fontSize: 16),
                          ),
                        ],
                      ),
                      ElevatedButton(
                        onPressed: () {
                          if (_formKey.currentState!.validate()) {
                            loginUser();
                          }
                        },
                        style: ElevatedButton.styleFrom(
                          padding: EdgeInsets.symmetric(
                              vertical: 16, horizontal: 32),
                          backgroundColor:
                              const Color.fromARGB(255, 20, 87, 24),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(12),
                          ),
                          shadowColor: Colors.black.withOpacity(0.3),
                          elevation: 10,
                        ),
                        child: Text(
                          'Entrar',
                          style: TextStyle(fontSize: 18, color: Colors.white),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}
